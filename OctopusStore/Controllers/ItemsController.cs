using System.Threading.Tasks;
using ApplicationCore.Entities;
using ApplicationCore.Specifications;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ApplicationCore.ViewModels;
using ApplicationCore.Interfaces;
using ApplicationCore.Interfaces.Services;
using ApplicationCore.Interfaces.Controllers;

namespace OctopusStore.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class ItemsController: CRUDController<IItemService, Item, ItemViewModel>, IItemsController
    {
        public ItemsController(
            IItemService service,
            IScopedParameters scopedParameters,
            IAuthorizationService authorizationService,
            IAppLogger<IController<Item, ItemViewModel>> logger)
           : base(service, scopedParameters, logger)
        {
        }

        [HttpPost]
        public override async Task<ItemViewModel> CreateAsync([FromBody]ItemViewModel itemViewModel) => await base.CreateAsync(itemViewModel);

        //[AllowAnonymous]
        [HttpGet("thumbnails/")]
        public async Task<IndexViewModel<ItemThumbnailViewModel>> IndexThumbnailsAsync(
            [FromQuery(Name = "page")] int? page,
            [FromQuery(Name = "pageSize")]int? pageSize,
            [FromQuery(Name = "title")]string title,
            [FromQuery(Name = "categoryId")]int? categoryId,
            [FromQuery(Name = "storeId")]int? storeId,
            [FromQuery(Name = "brandId")]int? brandId)
        {
            page = page ?? 1;
            pageSize = pageSize ?? _defaultTake;
            var spec = await _service.GetIndexSpecificationByParameters(page.Value, pageSize.Value, title, categoryId, storeId, brandId);
            return await base.IndexAsync<ItemThumbnailViewModel>(new ItemThumbnailIndexSpecification(spec));
        }

        //[AllowAnonymous]
        [HttpGet]
        public async Task<IndexViewModel<ItemViewModel>> IndexAsync(
            [FromQuery(Name = "page")] int? page,
            [FromQuery(Name = "pageSize")]int? pageSize,
            [FromQuery(Name = "title")]string title,
            [FromQuery(Name = "categoryId")]int? categoryId,
            [FromQuery(Name = "storeId")]int? storeId,
            [FromQuery(Name = "brandId")]int? brandId)
        {
            page = page ?? 1;
            pageSize = pageSize ?? _defaultTake;
            return await base.IndexAsync(await _service.GetIndexSpecificationByParameters(page.Value, pageSize.Value, title, categoryId, storeId, brandId));
        }

        //[AllowAnonymous]
        [HttpGet("{id:int}")]
        public override async Task<ItemViewModel> ReadAsync(int id) => await base.ReadAsync(id);

        //[AllowAnonymous]
        [HttpGet("{id:int}/detail")]
        public async Task<ItemDetailViewModel> ReadDetailAsync(int id) => await base.GetDetailAsync<ItemDetailViewModel>(new ItemDetailSpecification(id));
        
        [HttpPut("{id:int}")]
        public override async Task<ItemViewModel> UpdateAsync([FromBody]ItemViewModel itemViewModel) => await base.UpdateAsync(itemViewModel);

        [HttpDelete("{id:int}")]
        public override async Task<Response> DeleteAsync(int id) => await base.DeleteAsync(id);

        [HttpGet("{id:int}/checkUpdateAuthorization")]
        public override async Task<Response> CheckUpdateAuthorizationAsync(int id) => await base.CheckUpdateAuthorizationAsync(id);
    }
}
