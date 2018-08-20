using System.Collections.Generic;
using System.Threading.Tasks;
using ApplicationCore.Entities;
using ApplicationCore.Interfaces;
using Microsoft.AspNetCore.Mvc;
using OctopusStore.Specifications;
using OctopusStore.ViewModels;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace OctopusStore.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class ItemVariantCharacteristicValuesController 
        : ReadWriteController<
            IItemVariantCharacteristicValueService, 
            ItemVariantCharacteristicValue, 
            ItemVariantCharacteristicValueViewModel, 
            ItemVariantCharacteristicValueDetailViewModel, 
            ItemVariantCharacteristicValueIndexViewModel>
    {
        public ItemVariantCharacteristicValuesController(
            IItemVariantCharacteristicValueService service,
            IAppLogger<IEntityController<ItemVariantCharacteristicValue>> logger)
            : base(service, logger)
        {  }

        [HttpGet]
        public async Task<ItemVariantCharacteristicValueIndexViewModel> Index(
            [FromQuery(Name = "itemVariantId")]int? itemVariantId,
            [FromQuery(Name = "itemId")]int? itemId)
        {
            
            if (itemVariantId.HasValue)
                return await IndexByItemVariant(itemVariantId.Value);
            else if (itemId.HasValue)
                return await IndexByItem(itemId.Value);
            return new ItemVariantCharacteristicValueIndexViewModel(1, 0, 0, new List<ItemVariantCharacteristicValue>());
        }
        [HttpGet("/api/itemVariants/{itemVariantId:int}/characteristicValues")]
        public async Task<ItemVariantCharacteristicValueIndexViewModel> IndexByItemVariant(int itemVariantId)
        {
            var spec = new ItemVariantCharacteristicValueSpecification(itemVariantId);
            return await base.IndexNotPagedAsync(spec);
        }
        [HttpGet("/api/items/{itemId:int}/characteristicValues")]
        public async Task<ItemVariantCharacteristicValueIndexViewModel> IndexByItem(int itemId)
        {
            return await base.IndexByRelatedNotPagedAsync(
                    _serivce.ListByItemVariantAsync,
                    new ItemVariantByItemSpecification(itemId));
        }
        // PUT api/<controller>/5
        [HttpPut("{id:int}")]
        public async Task<ItemVariantCharacteristicValueViewModel> Put(int id, [FromBody]ItemVariantCharacteristicValueViewModel itemVariantCharacteristicValueViewModel)
        {
            itemVariantCharacteristicValueViewModel.Id = id;
            return await base.PutAsync(itemVariantCharacteristicValueViewModel);
        }
        [HttpPost]
        public async Task<ItemVariantCharacteristicValueViewModel> Post([FromBody]ItemVariantCharacteristicValueViewModel itemVariantCharacteristicValueViewModel)
        {
            return await base.PostAsync(itemVariantCharacteristicValueViewModel);
        }
        // DELETE api/<controller>/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            return await base.DeleteAsync(new ItemVariantCharacteristicValueDetailSpecification(id));
        }
    }
}
