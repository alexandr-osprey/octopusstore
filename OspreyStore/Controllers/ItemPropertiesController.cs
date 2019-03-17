using System.Collections.Generic;
using System.Threading.Tasks;
using ApplicationCore.Entities;
using ApplicationCore.Exceptions;
using ApplicationCore.Specifications;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ApplicationCore.ViewModels;
using ApplicationCore.Interfaces;
using ApplicationCore.Interfaces.Services;
using ApplicationCore.Interfaces.Controllers;

namespace OspreyStore.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class ItemPropertiesController: CRUDController<IItemPropertyService, ItemProperty, ItemPropertyViewModel>, IItemPropertiesController
    {
        public ItemPropertiesController(
            IItemPropertyService itemPropertyService,
            IActivatorService activatorService,
            IScopedParameters scopedParameters,
            IAppLogger<IController<ItemProperty, ItemPropertyViewModel>> logger)
           : base(itemPropertyService, activatorService, scopedParameters, logger)
        {
        }

        [HttpPost]
        public override async Task<ItemPropertyViewModel> CreateAsync([FromBody]ItemPropertyViewModel itemPropertyViewModel) => await base.CreateAsync(itemPropertyViewModel ?? throw new BadRequestException("Item variant characteristic value not provided"));

        [AllowAnonymous]
        [HttpGet]
        public async Task<IndexViewModel<ItemPropertyViewModel>> IndexAsync(
            [FromQuery(Name = "itemVariantId")]int? itemVariantId,
            [FromQuery(Name = "itemId")]int? itemId)
        {
            if (itemVariantId.HasValue)
                return await IndexByItemVariantAsync(itemVariantId.Value);
            else if (itemId.HasValue)
                return await IndexByItemAsync(itemId.Value);
            return await GetNotPagedIndexViewModelAsync(new List<ItemProperty>());
        }

        [AllowAnonymous]
        [HttpGet("/api/itemVariants/{itemVariantId:int}/characteristicValues")]
        public async Task<IndexViewModel<ItemPropertyViewModel>> IndexByItemVariantAsync(int itemVariantId) => await base.IndexNotPagedAsync(new ItemPropertyByVariantSpecification(itemVariantId));

        [AllowAnonymous]
        [HttpGet("/api/items/{itemId:int}/characteristicValues")]
        public async Task<IndexViewModel<ItemPropertyViewModel>> IndexByItemAsync(int itemId) => await base.IndexByRelatedNotPagedAsync(Service.EnumerateByItemVariantAsync, new ItemVariantByItemSpecification(itemId));

        [HttpPut]
        public override async Task<ItemPropertyViewModel> UpdateAsync([FromBody]ItemPropertyViewModel itemPropertyViewModel) => await base.UpdateAsync(itemPropertyViewModel ?? throw new BadRequestException("Item variant characteristic value not provided"));

        [HttpDelete("{id}")]
        public override async Task<Response> DeleteAsync(int id) => await base.DeleteAsync(id);

        [HttpGet("{id:int}/checkUpdateAuthorization")]
        public override async Task<Response> CheckUpdateAuthorizationAsync(int id) => await base.CheckUpdateAuthorizationAsync(id);
    }
}
