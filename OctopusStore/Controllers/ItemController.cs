using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ApplicationCore.Entities;
using ApplicationCore.Interfaces;
using ApplicationCore.Specifications;
using Microsoft.AspNetCore.Mvc;
using OctopusStore.Specifications;
using OctopusStore.ViewModels;

namespace OctopusStore.Controllers
{
    [Produces("application/json")]
    [Route("api/Items")]
    public class ItemController
        : ReadWriteController<
            IItemService,
            Item,
            ItemViewModel,
            ItemDetailViewModel,
            ItemIndexViewModel>
    {
        protected readonly ICategoryService _categoryService;

        public ItemController(IItemService itemService, ICategoryService categoryService, IAppLogger<IEntityController<Item>> logger)
            : base(itemService, logger)
        {
            _categoryService = categoryService;
        }

        // GET: api/Items
        [HttpGet("thumbnails/")]
        public async Task<ItemThumbnailIndexViewModel> IndexThumbnails(
            [FromQuery(Name = "page")] int? page,
            [FromQuery(Name = "pageSize")]int? pageSize,
            [FromQuery(Name = "title")]string title,
            [FromQuery(Name = "categoryId")]int? categoryId,
            [FromQuery(Name = "storeId")]int? storeId,
            [FromQuery(Name = "brandId")]int? brandId)
        {
            var spec = await GetIndexSpecAsync<ItemThumbnailIndexSpecification>(page, pageSize, title, categoryId, storeId, brandId);
            return await base.IndexAsync<ItemThumbnailIndexViewModel, ItemThumbnailViewModel>(spec);
        }
        [HttpGet]
        public async Task<ItemIndexViewModel> Index(
            [FromQuery(Name = "page")] int? page,
            [FromQuery(Name = "pageSize")]int? pageSize,
            [FromQuery(Name = "title")]string title,
            [FromQuery(Name = "categoryId")]int? categoryId,
            [FromQuery(Name = "storeId")]int? storeId,
            [FromQuery(Name = "brandId")]int? brandId)
        {
            var spec = await GetIndexSpecAsync<ItemIndexSpecification>(page, pageSize, title, categoryId, storeId, brandId);
            return await base.IndexAsync(spec);
        }
        [HttpGet("{id:int}")]
        public async Task<ItemViewModel> Get(int id)
        {
            return await base.GetAsync(new Specification<Item>(id));
        }
        [HttpGet("{id:int}/details")]
        public async Task<ItemDetailViewModel> GetDetail(int id)
        {
            return await base.GetDetailAsync(new ItemDetailSpecification(id));
        }
        //
        // POST: api/Items
        [HttpPost]
        public async Task<ItemViewModel> Post([FromBody]ItemViewModel itemViewModel)
        {
            return await base.PostAsync(itemViewModel);
        }
        // PUT: api/Items/5
        [HttpPut("{id:int}")]
        public async Task<ItemViewModel> Put(int id, [FromBody]ItemViewModel itemViewModel)
        {
            itemViewModel.Id = id;
            return await base.PutAsync(itemViewModel);
        }
        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            return await base.DeleteAsync(new ItemDetailSpecification(id));
        }

        private async Task<TItemIndexSpecification> GetIndexSpecAsync<TItemIndexSpecification>(int? page, int? pageSize, string title, int? categoryId, int? storeId, int? brandId) where TItemIndexSpecification : ISpecification<Item>
        {
            page = page ?? 1;
            pageSize = pageSize ?? _defaultTake;

            IEnumerable<Category> categories = new List<Category>();
            if (categoryId.HasValue)
            {
                categoryId = categoryId ?? _categoryService.RootCategoryId;
                var categorySpec = new CategoryDetailSpecification(categoryId.Value);
                categories = await _categoryService.ListSubcategoriesAsync(categorySpec);
            }
            return (TItemIndexSpecification)Activator.CreateInstance(typeof(TItemIndexSpecification), page.Value, pageSize.Value, title, categories, storeId, brandId);
        }
    }
}
