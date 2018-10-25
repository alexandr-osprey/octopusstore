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
    public class ItemVariantsController: CRUDController<IItemVariantService, ItemVariant, ItemVariantViewModel>
    {
        public ItemVariantsController(
            IItemVariantService itemVariantService,
            IScopedParameters scopedParameters,
            IAppLogger<ICRUDController<ItemVariant>> logger)
           : base(itemVariantService, scopedParameters, logger)
        {
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IndexViewModel<ItemVariantViewModel>> Index([FromQuery(Name = "itemId")]int itemId)
        {
            return await IndexByItem(itemId);
        }
        [AllowAnonymous]
        [HttpGet("/api/items/{itemId:int}/itemVariants")]
        public async Task<IndexViewModel<ItemVariantViewModel>> IndexByItem(int itemId)
        {
            return await base.IndexNotPagedAsync(new ItemVariantByItemSpecification(itemId));
        }
        // GET api/<controller>/5
        [AllowAnonymous]
        [HttpGet("{id:int}")]
        public async Task<ItemVariantViewModel> Get(int id)
        {
            return await base.GetAsync(new EntitySpecification<ItemVariant>(id));
        }
        [AllowAnonymous]
        [HttpGet("{id:int}/details")]
        public async Task<ItemVariantDetailViewModel> GetDetail(int id)
        {
            return await base.GetDetailAsync<ItemVariantDetailViewModel>(new ItemVariantDetailSpecification(id));
        }
        // POST api/<controller>
        [HttpPost]
        public async Task<ItemVariantViewModel> Post([FromBody]ItemVariantViewModel itemVariantViewModel)
        {
            if (itemVariantViewModel == null)
                throw new BadRequestException("Item variant to post not provided");
            return await base.CreateAsync(itemVariantViewModel);
        }
        // PUT api/<controller>/5
        [HttpPut("{id:int}")]
        public async Task<ItemVariantViewModel> Put(int id, [FromBody]ItemVariantViewModel itemVariantViewModel)
        {
            if (itemVariantViewModel == null)
                throw new BadRequestException("Item variant to put not provided");
            itemVariantViewModel.Id = id;
            return await base.UpdateAsync(itemVariantViewModel);
        }
        // DELETE api/<controller>/5
        [HttpDelete("{id}", Name = "ItemVariantDelete")]
        public async Task<Response> Delete(int id)
        {
            return await base.DeleteSingleAsync(new ItemVariantDetailSpecification(id));
        }
        [HttpGet("{id:int}/checkUpdateAuthorization")]
        public async Task<ActionResult> CheckUpdateAuthorization(int id) => await base.CheckUpdateAuthorizationAsync(id);
    }
}
