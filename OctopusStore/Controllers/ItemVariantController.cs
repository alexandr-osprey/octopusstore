using System.Threading.Tasks;
using ApplicationCore.Entities;
using ApplicationCore.Interfaces;
using ApplicationCore.Specifications;
using Microsoft.AspNetCore.Mvc;
using OctopusStore.Specifications;
using OctopusStore.ViewModels;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace OctopusStore.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class ItemVariantsController 
        : ReadWriteController<
            IItemVariantService, 
            ItemVariant, 
            ItemVariantViewModel, 
            ItemVariantDetailViewModel, 
            ItemVariantIndexViewModel>
    {
        public ItemVariantsController(IItemVariantService itemVariantService, IAppLogger<IEntityController<ItemVariant>> logger)
            : base(itemVariantService, logger)
        {  }

        [HttpGet]
        public async Task<ItemVariantIndexViewModel> Index([FromQuery(Name = "itemId")]int itemId)
        {
            return await IndexByItem(itemId);
        }
        [HttpGet("/api/items/{itemId:int}/itemVariants")]
        public async Task<ItemVariantIndexViewModel> IndexByItem(int itemId)
        {
            return await base.IndexNotPagedAsync(new ItemVariantByItemSpecification(itemId));
        }
        // GET api/<controller>/5
        [HttpGet("{id:int}")]
        public async Task<ItemVariantViewModel> Get(int id)
        {
            return await base.GetAsync(new Specification<ItemVariant>(id));
        }
        [HttpGet("{id:int}/details")]
        public async Task<ItemVariantDetailViewModel> GetDetail(int id)
        {
            return await base.GetDetailAsync(new ItemVariantDetailSpecification(id));
        }
        // POST api/<controller>
        [HttpPost]
        public async Task<ItemVariantViewModel> Post([FromBody]ItemVariantViewModel itemVariantViewModel)
        {
            return await base.PostAsync(itemVariantViewModel);
        }
        // PUT api/<controller>/5
        [HttpPut("{id:int}")]
        public async Task<ItemVariantViewModel> Put(int id, [FromBody]ItemVariantViewModel itemVariantViewModel)
        {
            itemVariantViewModel.Id = id;
            return await base.PutAsync(itemVariantViewModel);
        }
        // DELETE api/<controller>/5
        [HttpDelete("{id}", Name = "ItemVariantDelete")]
        public async Task<ActionResult> Delete(int id)
        {
            return await base.DeleteAsync(new ItemVariantDetailSpecification(id));
        }
    }
}
