using System.Collections.Generic;
using System.Threading.Tasks;
using ApplicationCore.Entities;
using ApplicationCore.Exceptions;
using ApplicationCore.Specifications;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ApplicationCore.ViewModels;
using ApplicationCore.Interfaces;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace OctopusStore.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class ItemPropertiesController: CRUDController<IItemPropertyService, ItemProperty, ItemPropertyViewModel>
    {
        public ItemPropertiesController(
            IItemPropertyService itemPropertyService,
            IScopedParameters scopedParameters,
            IAppLogger<ICRUDController<ItemProperty>> logger)
           : base(itemPropertyService, scopedParameters, logger)
        {
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IndexViewModel<ItemPropertyViewModel>> Index(
            [FromQuery(Name = "itemVariantId")]int? itemVariantId,
            [FromQuery(Name = "itemId")]int? itemId)
        {
            
            if (itemVariantId.HasValue)
                return await IndexByItemVariant(itemVariantId.Value);
            else if (itemId.HasValue)
                return await IndexByItem(itemId.Value);
            return GetNotPagedIndexViewModel(new List<ItemProperty>());
        }
        [AllowAnonymous]
        [HttpGet("/api/itemVariants/{itemVariantId:int}/characteristicValues")]
        public async Task<IndexViewModel<ItemPropertyViewModel>> IndexByItemVariant(int itemVariantId)
        {
            return await base.IndexNotPagedAsync(new ItemPropertyByVariantSpecification(itemVariantId));
        }
        [AllowAnonymous]
        [HttpGet("/api/items/{itemId:int}/characteristicValues")]
        public async Task<IndexViewModel<ItemPropertyViewModel>> IndexByItem(int itemId)
        {
            return await base.IndexByRelatedNotPagedAsync(_service.EnumerateByItemVariantAsync, new ItemVariantByItemSpecification(itemId));
        }
        // PUT api/<controller>/5
        [HttpPut("{id:int}")]
        public async Task<ItemPropertyViewModel> Put(int id, [FromBody]ItemPropertyViewModel itemPropertyViewModel)
        {
            if (itemPropertyViewModel == null) throw new BadRequestException("Item variant characteristic value not provided");
            itemPropertyViewModel.Id = id;
            return await base.UpdateAsync(itemPropertyViewModel);
        }
        [HttpPost]
        public async Task<ItemPropertyViewModel> Post([FromBody]ItemPropertyViewModel itemPropertyViewModel)
        {
            if (itemPropertyViewModel == null) throw new BadRequestException("Item variant characteristic value not provided");
            return await base.CreateAsync(itemPropertyViewModel);
        }
        // DELETE api/<controller>/5
        [HttpDelete("{id}")]
        public async Task<Response> Delete(int id) => await base.DeleteSingleAsync(new EntitySpecification<ItemProperty>(id));

        [HttpGet("{id:int}/checkUpdateAuthorization")]
        public async Task<ActionResult> CheckUpdateAuthorization(int id) => await base.CheckUpdateAuthorizationAsync(id);
    }
}
