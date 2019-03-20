using ApplicationCore.Entities;
using ApplicationCore.Extensions;
using ApplicationCore.Identity;
using ApplicationCore.Interfaces;
using ApplicationCore.Interfaces.Controllers;
using ApplicationCore.Interfaces.Services;
using ApplicationCore.Specifications;
using ApplicationCore.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OspreyStore.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class ItemsController : CRUDController<IItemService, Item, ItemViewModel>, IItemsController
    {
        public ItemsController(
            IItemService service,
            IActivatorService activatorService,
            IScopedParameters scopedParameters,
            IIdentityService identityService,
            IAuthorizationService authorizationService,
            IAppLogger<IController<Item, ItemViewModel>> logger)
           : base(service, activatorService, identityService, scopedParameters, logger)
        {
        }

        [HttpPost]
        public override async Task<ItemViewModel> CreateAsync([FromBody]ItemViewModel itemViewModel) => await base.CreateAsync(itemViewModel);

        [AllowAnonymous]
        [HttpGet("thumbnails/")]
        public async Task<IndexViewModel<ItemThumbnailViewModel>> IndexThumbnailsAsync(
            int? page,
            int? pageSize,
            string searchValue,
            int? categoryId,
            int? storeId,
            int? brandId,
            string orderBy,
            string characteristicsFilter,
            bool? orderByDescending)
        {
            var spec = await Service.GetIndexSpecificationByParameters(page ?? 1, pageSize ?? DefaultTake, 
                searchValue, categoryId, storeId, brandId, ParseIds(characteristicsFilter));
            ApplyOrderingToSpec(spec, orderBy, orderByDescending);
            return await base.IndexAsync<ItemThumbnailViewModel>(new ItemThumbnailIndexSpecification(spec));
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IndexViewModel<ItemViewModel>> IndexAsync(
            int? page,
            int? pageSize,
            string searchValue,
            int? categoryId,
            int? storeId,
            int? brandId,
            string orderBy,
            string characteristicsFilter,
            bool? orderByDescending)
        {
            var spec = await Service.GetIndexSpecificationByParameters(page ?? 1, pageSize ?? DefaultTake, 
                searchValue, categoryId, storeId, brandId, ParseIds(characteristicsFilter));
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
                    spec.OrderByExpressions.Add(i => SafeMax(i.ItemVariants.Select(v => v.Price)));
                else
                    spec.OrderByExpressions.Add(i => SafeMin(i.ItemVariants.Select(v => v.Price)));
            }
        }

        protected decimal SafeMin(IEnumerable<decimal> seq)
        {
            return seq.Any() ? seq.Min() : 0;
        }
        protected decimal SafeMax(IEnumerable<decimal> seq)
        {
            return seq.Any() ? seq.Max() : 0;
        }
    }
}
