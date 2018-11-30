using System.Threading.Tasks;
using ApplicationCore.Entities;
using ApplicationCore.Specifications;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ApplicationCore.ViewModels;
using ApplicationCore.Interfaces;
using System.Collections.Generic;
using System.Linq;
using ApplicationCore.Interfaces.Services;
using ApplicationCore.Interfaces.Controllers;

namespace OctopusStore.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class CategoriesController: CRUDController<ICategoryService, Category, CategoryViewModel>, ICategoriesController
    {
        public CategoriesController(
            ICategoryService service,
            IActivatorService activatorService,
            IScopedParameters scopedParameters,
            IAppLogger<IController<Category,CategoryViewModel>> logger)
           : base(service, activatorService, scopedParameters, logger)
        {
        }

        [HttpPost]
        public override async Task<CategoryViewModel> CreateAsync([FromBody]CategoryViewModel categoryViewModel) => await base.CreateAsync(categoryViewModel);

        [AllowAnonymous]
        [HttpGet("{id:int}")]
        public override async Task<CategoryViewModel> ReadAsync(int id) => await base.ReadAsync(id);

        [AllowAnonymous]
        [HttpGet("root")]
        public async Task<CategoryViewModel> ReadRootAsync() => await base.ReadAsync(Service.RootCategory.Id);

        // GET: api/<controller>
        [AllowAnonymous]
        [HttpGet]
        public async Task<IndexViewModel<CategoryViewModel>> IndexAsync(
            [FromQuery(Name = "categoryId")]int? categoryId,
            [FromQuery(Name = "storeId")]int? storeId)
        {
            categoryId = categoryId ?? Service.RootCategory.Id;
            var categories = await Service.EnumerateHierarchyAsync(new EntitySpecification<Category>(c => c.Id == categoryId.Value));
            if (storeId.HasValue)
            {
                var storeCategories = await IndexByStoreIdAsync(storeId.Value);
                categories = (from c in categories where storeCategories.Contains(c) select c).OrderBy(c => c.Id);
            }
            return GetNotPagedIndexViewModel(categories);
        }
        //[AllowAnonymous]
        //[HttpGet("/api/stores/{id:int}/categories")]
        //public async Task<CategoryIndexViewModel> IndexByStoreId(int id)
        //{
        //    var spec = new Specification<Item>((i => i.StoreId == id), (i => i.Category));
        //    spec.Description = $"Items with StoreId={id} includes Category";
        //    return await base.IndexByRelatedNotPagedAsync(_service.EnumerateByItemAsync, spec);
        //}

        [HttpPut]
        public override async Task<CategoryViewModel> UpdateAsync([FromBody]CategoryViewModel categoryViewModel) => await base.UpdateAsync(categoryViewModel);

        [HttpGet("{id:int}/checkUpdateAuthorization")]
        public override async Task<Response> CheckUpdateAuthorizationAsync(int id) => await base.CheckUpdateAuthorizationAsync(id);

        protected async Task<IEnumerable<Category>> IndexByStoreIdAsync(int storeId)
        {
            var spec = new Specification<Item>((i => i.StoreId == storeId), (i => i.Category))
            {
                Description = $"Items with StoreId={storeId} includes Category"
            };
            return await Service.EnumerateParentCategoriesAsync(spec);
        }
    }
}
