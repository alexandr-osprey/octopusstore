using System.Threading.Tasks;
using ApplicationCore.Entities;
using ApplicationCore.Specifications;
using Microsoft.AspNetCore.Mvc;
using ApplicationCore.ViewModels;
using ApplicationCore.Interfaces;
using ApplicationCore.Exceptions;
using ApplicationCore.Interfaces.Services;
using ApplicationCore.Interfaces.Controllers;

namespace OctopusStore.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class CartItemsController: CRUDController<ICartItemService, CartItem, CartItemViewModel>, ICartItemsController
    {
        public CartItemsController(
            ICartItemService cartItemService,
            IScopedParameters scopedParameters,
            IAppLogger<IController<CartItem, CartItemViewModel>> logger)
           : base(cartItemService, scopedParameters, logger)
        {
        }

        [HttpPost]
        public override async Task<CartItemViewModel> CreateAsync([FromBody]CartItemViewModel cartItemViewModel) => await base.CreateAsync(cartItemViewModel);

        [HttpPost("addToCart")]
        public async Task<CartItemViewModel> AddToCartAsync([FromBody]CartItemViewModel cartItemViewModel)
        {
            if (cartItemViewModel == null)
                throw new BadRequestException("Cart item not provided");
            return base.GetViewModel<CartItemViewModel>(
                await Service.AddToCartAsync(
                    ScopedParameters.ClaimsPrincipal.Identity.Name, 
                    cartItemViewModel.ItemVariantId, cartItemViewModel.Number));
        }

        [HttpDelete("removeFromCart")]
        public async Task<Response> RemoveFromCartAsync([FromBody]CartItemViewModel cartItemViewModel)
        {
            if (cartItemViewModel == null)
                throw new BadRequestException("Cart item not provided");
            await Service.RemoveFromCartAsync(
                ScopedParameters.ClaimsPrincipal.Identity.Name, 
                cartItemViewModel.ItemVariantId, cartItemViewModel.Number);
            return new Response($"{cartItemViewModel.Number} of item variant {cartItemViewModel.ItemVariantId} removed from a cart");
        }

        [HttpGet]
        public async Task<IndexViewModel<CartItemViewModel>> IndexAsync() => await base.IndexNotPagedAsync(
            new CartItemSpecification(ScopedParameters.ClaimsPrincipal.Identity.Name));

        [HttpGet("thumbnails/")]
        public async Task<IndexViewModel<CartItemThumbnailViewModel>> IndexThumbnailsAsync() => await base.IndexNotPagedAsync<CartItemThumbnailViewModel>(
            new CartItemThumbnailSpecification(ScopedParameters.ClaimsPrincipal.Identity.Name));

        [HttpGet("{id:int}")]
        public override async Task<CartItemViewModel> ReadAsync(int id) => await base.ReadAsync(id);

        [HttpPut]
        public override async Task<CartItemViewModel> UpdateAsync([FromBody]CartItemViewModel cartItemViewModel) => await base.UpdateAsync(cartItemViewModel);

        [HttpDelete("{id:int}")]
        public override async Task<Response> DeleteAsync(int id) => await base.DeleteSingleAsync(new EntitySpecification<CartItem>(id));

        [HttpDelete]
        public async Task<Response> DeleteAsync() => await base.DeleteAsync(new CartItemSpecification(ScopedParameters.ClaimsPrincipal.Identity.Name));

        [HttpGet("{id:int}/checkUpdateAuthorization")]
        public override async Task<Response> CheckUpdateAuthorizationAsync(int id) => await base.CheckUpdateAuthorizationAsync(id);
    }
}
