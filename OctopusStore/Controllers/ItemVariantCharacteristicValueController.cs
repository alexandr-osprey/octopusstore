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
    public class ItemVariantCharacteristicValuesController 
        : CRUDController<
            IItemVariantCharacteristicValueService,
            ItemVariantCharacteristicValue, 
            ItemVariantCharacteristicValueViewModel, 
            ItemVariantCharacteristicValueDetailViewModel, 
            ItemVariantCharacteristicValueIndexViewModel>
    {
        public ItemVariantCharacteristicValuesController(
            IItemVariantCharacteristicValueService itemVariantCharacteristicValueService,
            IScopedParameters scopedParameters,
            IAppLogger<ICRUDController<ItemVariantCharacteristicValue>> logger)
            : base(itemVariantCharacteristicValueService, scopedParameters, logger)
        {
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<ItemVariantCharacteristicValueIndexViewModel> Index(
            [FromQuery(Name = "itemVariantId")]int? itemVariantId,
            [FromQuery(Name = "itemId")]int? itemId)
        {
            
            if (itemVariantId.HasValue)
                return await IndexByItemVariant(itemVariantId.Value);
            else if (itemId.HasValue)
                return await IndexByItem(itemId.Value);
            return GetNotPagedIndexViewModel(new List<ItemVariantCharacteristicValue>());
        }
        [AllowAnonymous]
        [HttpGet("/api/itemVariants/{itemVariantId:int}/characteristicValues")]
        public async Task<ItemVariantCharacteristicValueIndexViewModel> IndexByItemVariant(int itemVariantId)
        {
            return await base.IndexNotPagedAsync(new ItemVariantCharacteristicValueSpecification(itemVariantId));
        }
        [AllowAnonymous]
        [HttpGet("/api/items/{itemId:int}/characteristicValues")]
        public async Task<ItemVariantCharacteristicValueIndexViewModel> IndexByItem(int itemId)
        {
            return await base.IndexByRelatedNotPagedAsync(_service.EnumerateByItemVariantAsync, new ItemVariantByItemSpecification(itemId));
        }
        [HttpGet("{id:int}/checkUpdateAuthorization")]
        public async Task<ActionResult> CheckUpdateAuthorization(int id)
        {
            return await base.CheckUpdateAuthorizationAsync(id);
        }
        // PUT api/<controller>/5
        [HttpPut("{id:int}")]
        public async Task<ItemVariantCharacteristicValueViewModel> Put(int id, [FromBody]ItemVariantCharacteristicValueViewModel itemVariantCharacteristicValueViewModel)
        {
            if (itemVariantCharacteristicValueViewModel == null) throw new BadRequestException("Item variant characteristic value not provided");
            itemVariantCharacteristicValueViewModel.Id = id;
            return await base.UpdateAsync(itemVariantCharacteristicValueViewModel);
        }
        [HttpPost]
        public async Task<ItemVariantCharacteristicValueViewModel> Post([FromBody]ItemVariantCharacteristicValueViewModel itemVariantCharacteristicValueViewModel)
        {
            if (itemVariantCharacteristicValueViewModel == null) throw new BadRequestException("Item variant characteristic value not provided");
            return await base.CreateAsync(itemVariantCharacteristicValueViewModel);
        }
        // DELETE api/<controller>/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            return await base.DeleteSingleAsync(new ItemVariantCharacteristicValueDetailSpecification(id));
        }
    }
}
