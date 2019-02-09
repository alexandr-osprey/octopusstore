using ApplicationCore.Entities;
using ApplicationCore.Exceptions;
using ApplicationCore.Interfaces;
using ApplicationCore.Interfaces.Controllers;
using ApplicationCore.Interfaces.Services;
using ApplicationCore.Specifications;
using ApplicationCore.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace OctopusStore.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class CartItemsController : CRUDController<ICartItemService, CartItem, CartItemViewModel>, ICartItemsController
    {
        public CartItemsController(
            ICartItemService cartItemService,
            IActivatorService activatorService,
            IScopedParameters scopedParameters,
            IAppLogger<IController<CartItem, CartItemViewModel>> logger)
           : base(cartItemService, activatorService, scopedParameters, logger)
        {
        }

        [HttpPost]
        public override async Task<CartItemViewModel> CreateAsync([FromBody]CartItemViewModel cartItemViewModel) => await base.CreateAsync(cartItemViewModel);

        [HttpPut("addToCart")]
        public async Task<CartItemThumbnailViewModel> AddToCartAsync([FromBody]CartItemViewModel cartItemViewModel)
        {
            if (cartItemViewModel == null)
                throw new BadRequestException("Cart item not provided");
            var added = await Service.AddToCartAsync(
                ScopedParameters.CurrentUserId,
                cartItemViewModel.ItemVariantId, cartItemViewModel.Number);
            //return new Response($"{cartItemViewModel.Number} of item variant {cartItemViewModel.ItemVariantId} added to a cart");
            return await ReadAsync<CartItemThumbnailViewModel>(new CartItemThumbnailSpecification(added.Id));
        }

        [HttpPut("removeFromCart")]
        public async Task<Response> RemoveFromCartAsync([FromBody]CartItemViewModel cartItemViewModel)
        {
            if (cartItemViewModel == null)
                throw new BadRequestException("Cart item not provided");
            await Service.RemoveFromCartAsync(
                ScopedParameters.CurrentUserId,
                cartItemViewModel.ItemVariantId, cartItemViewModel.Number);
            return new Response($"{cartItemViewModel.Number} of item variant {cartItemViewModel.ItemVariantId} removed from a cart");
        }

        [HttpGet]
        public async Task<IndexViewModel<CartItemViewModel>> IndexAsync() => await base.IndexNotPagedAsync(
            new CartItemSpecification(ScopedParameters.CurrentUserId));

        [HttpGet("thumbnails/")]
        public async Task<IndexViewModel<CartItemThumbnailViewModel>> IndexThumbnailsAsync() => await base.IndexNotPagedAsync<CartItemThumbnailViewModel>(
            new CartItemThumbnailSpecification(ScopedParameters.CurrentUserId));

        [HttpGet("{id:int}")]
        public override async Task<CartItemViewModel> ReadAsync(int id) => await base.ReadAsync(id);

        [HttpPut]
        public override async Task<CartItemViewModel> UpdateAsync([FromBody]CartItemViewModel cartItemViewModel) => await base.UpdateAsync(cartItemViewModel);

        [HttpDelete("{id:int}")]
        public override async Task<Response> DeleteAsync(int id) => await base.DeleteSingleAsync(new EntitySpecification<CartItem>(id));

        [HttpDelete]
        public async Task<Response> DeleteAsync() => await base.DeleteAsync(new CartItemSpecification(ScopedParameters.CurrentUserId));

        [HttpGet("{id:int}/checkUpdateAuthorization")]
        public override async Task<Response> CheckUpdateAuthorizationAsync(int id) => await base.CheckUpdateAuthorizationAsync(id);
    }
}
