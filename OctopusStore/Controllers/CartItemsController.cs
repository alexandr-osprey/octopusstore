using System.Threading.Tasks;
using ApplicationCore.Entities;
using ApplicationCore.Specifications;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ApplicationCore.ViewModels;
using ApplicationCore.Interfaces;
using ApplicationCore.Identity;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace OctopusStore.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class CartItemsController: CRUDController<ICartItemService, CartItem, CartItemViewModel>
    {
        public CartItemsController(
            ICartItemService cartItemService,
            IScopedParameters scopedParameters,
            IAppLogger<ICRUDController<CartItem>> logger)
           : base(cartItemService, scopedParameters, logger)
        {
        }

        [HttpPost]
        public async Task<CartItemViewModel> Post([FromBody]CartItemViewModel cartItemViewModel)
        {
            //cartItemViewModel.OwnerId = _scopedParameters.ClaimsPrincipal.Identity.Name;
            return await base.CreateAsync(cartItemViewModel);
        }
        [HttpGet]
        public async Task<IndexViewModel<CartItemViewModel>> Index() => await base.IndexNotPagedAsync(new CartItemSpecification(_scopedParameters.ClaimsPrincipal.Identity.Name));
        [HttpGet("{id:int}")]
        public async Task<CartItemViewModel> Get(int id) => await base.GetAsync(new EntitySpecification<CartItem>(id));
        [HttpPut]
        public async Task<CartItemViewModel> Put([FromBody]CartItemViewModel cartItemViewModel) => await base.UpdateAsync(cartItemViewModel);
        [HttpDelete("{id:int}")]
        public async Task<Response> Delete(int id) => await base.DeleteSingleAsync(new EntitySpecification<CartItem>(id));
        [HttpDelete]
        public async Task<Response> Delete() => await base.DeleteSingleAsync(new CartItemSpecification(_scopedParameters.ClaimsPrincipal.Identity.Name));
        [HttpGet("{id:int}/checkUpdateAuthorization")]
        public async Task<ActionResult> CheckUpdateAuthorization(int id) => await base.CheckUpdateAuthorizationAsync(id);
    }
}
