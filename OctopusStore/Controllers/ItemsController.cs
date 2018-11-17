using ApplicationCore.Entities;
using ApplicationCore.Extensions;
using ApplicationCore.Interfaces;
using ApplicationCore.Interfaces.Controllers;
using ApplicationCore.Interfaces.Services;
using ApplicationCore.Specifications;
using ApplicationCore.ViewModels;
using Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace OctopusStore.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class ItemsController : CRUDController<IItemService, Item, ItemViewModel>, IItemsController
    {
        public ItemsController(
            IItemService service,
            IActivatorService activatorService,
            IScopedParameters scopedParameters,
            IAuthorizationService authorizationService,
            IAppLogger<IController<Item, ItemViewModel>> logger)
           : base(service, activatorService, scopedParameters, logger)
        {
        }

        [HttpPost]
        public override async Task<ItemViewModel> CreateAsync([FromBody]ItemViewModel itemViewModel) => await base.CreateAsync(itemViewModel);

        [AllowAnonymous]
        [HttpGet("thumbnails/")]
        public async Task<IndexViewModel<ItemThumbnailViewModel>> IndexThumbnailsAsync(
            [FromQuery(Name = "page")] int? page,
            [FromQuery(Name = "pageSize")]int? pageSize,
            [FromQuery(Name = "title")]string title,
            [FromQuery(Name = "categoryId")]int? categoryId,
            [FromQuery(Name = "storeId")]int? storeId,
            [FromQuery(Name = "brandId")]int? brandId,
            [FromQuery(Name = "orderBy")]string orderBy,
            [FromQuery(Name = "orderByDescending")]bool? orderByDescending)
        {
            page = page ?? 1;
            pageSize = pageSize ?? DefaultTake;
            var spec = await Service.GetIndexSpecificationByParameters(page.Value, pageSize.Value, title, categoryId, storeId, brandId);
            ApplyOrderingToSpec(spec, orderBy, orderByDescending);
            return await base.IndexAsync<ItemThumbnailViewModel>(new ItemThumbnailIndexSpecification(spec));
            //var items = await Service.Context.EnumerateAsync(Logger, spec, spec.OrderByExpressions.First());
            //var vms = from i in items select new ItemThumbnailViewModel(i);
            //return IndexViewModel<ItemThumbnailViewModel>.FromEnumerableNotPaged(vms);
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IndexViewModel<ItemViewModel>> IndexAsync(
            [FromQuery(Name = "page")] int? page,
            [FromQuery(Name = "pageSize")]int? pageSize,
            [FromQuery(Name = "title")]string title,
            [FromQuery(Name = "categoryId")]int? categoryId,
            [FromQuery(Name = "storeId")]int? storeId,
            [FromQuery(Name = "brandId")]int? brandId,
            [FromQuery(Name = "orderBy")]string orderBy,
            [FromQuery(Name = "orderByDescending")]bool? orderByDescending)
        {
            page = page ?? 1;
            pageSize = pageSize ?? DefaultTake;
            var spec = await Service.GetIndexSpecificationByParameters(page.Value, pageSize.Value, title, categoryId, storeId, brandId);
            ApplyOrderingToSpec(spec, orderBy, orderByDescending);
            return await base.IndexAsync(spec);
        }

        [AllowAnonymous]
        [HttpGet("{id:int}")]
        public override async Task<ItemViewModel> ReadAsync(int id) => await base.ReadAsync(id);

        [AllowAnonymous]
        [HttpGet("{id:int}/detail")]
        public async Task<ItemDetailViewModel> ReadDetailAsync(int id) => await base.ReadDetailAsync<ItemDetailViewModel>(new ItemDetailSpecification(id));

        [HttpPut]
        public override async Task<ItemViewModel> UpdateAsync([FromBody]ItemViewModel itemViewModel) => await base.UpdateAsync(itemViewModel);

        [HttpDelete("{id:int}")]
        public override async Task<Response> DeleteAsync(int id) => await base.DeleteAsync(id);

        [HttpGet("{id:int}/checkUpdateAuthorization")]
        public override async Task<Response> CheckUpdateAuthorizationAsync(int id) => await base.CheckUpdateAuthorizationAsync(id);

        protected override void ApplyOrderingToSpec(Specification<Item> spec, string orderBy, bool? orderByDescending)
        {
            base.ApplyOrderingToSpec(spec, orderBy, orderByDescending);
            if (orderBy.EqualsCI("Title"))
                spec.OrderByExpressions.Add(i => i.Title);
            else if (orderBy.EqualsCI("Price"))
            {
                //spec.AddInclude(i => i.ItemVariants);
                if (spec.OrderByDesc)
                    spec.OrderByExpressions.Add(i => (from v in i.ItemVariants select v.Price).Max());
                else
                    spec.OrderByExpressions.Add(i => (from v in i.ItemVariants select v.Price).Min());
            }
        }
    }
}
