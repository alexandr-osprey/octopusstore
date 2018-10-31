//using System.Threading.Tasks;
//using ApplicationCore.Entities;
//using ApplicationCore.Specifications;
//using Microsoft.AspNetCore.Mvc;
//using ApplicationCore.ViewModels;
//using ApplicationCore.Interfaces;
//using ApplicationCore.Exceptions;
//using ApplicationCore.Interfaces.Services;

//namespace OctopusStore.Controllers
//{
//    [Produces("application/json")]
//    [Route("api/[controller]")]
//    public class CartItemsController: CRUDController<ICartItemService, CartItem, CartItemViewModel>
//    {
//        public CartItemsController(
//            ICartItemService cartItemService,
//            IScopedParameters scopedParameters,
//            IAppLogger<ICRUDController<CartItem>> logger)
//           : base(cartItemService, scopedParameters, logger)
//        {
//        }

//        [HttpPost]
//        public async Task<CartItemViewModel> Post([FromBody]CartItemViewModel cartItemViewModel) => await base.CreateAsync(cartItemViewModel);
//        [HttpPost("addToCart")]
//        public async Task<CartItemViewModel> AddToCart([FromBody]CartItemViewModel cartItemViewModel)
//        {
//            if (cartItemViewModel == null)
//                throw new BadRequestException("Cart item not provided");
//            return base.GetViewModel<CartItemViewModel>(await _service.AddToCartAsync(_scopedParameters.ClaimsPrincipal.Identity.Name, cartItemViewModel.ItemVariantId, cartItemViewModel.Number));
//        }
//        [HttpDelete("removeFromCart")]
//        public async Task<Response> RemoveFromCart([FromBody]CartItemViewModel cartItemViewModel)
//        {
//            if (cartItemViewModel == null)
//                throw new BadRequestException("Cart item not provided");
//            await _service.RemoveFromCartAsync(_scopedParameters.ClaimsPrincipal.Identity.Name, cartItemViewModel.ItemVariantId, cartItemViewModel.Number);
//            return new Response($"{cartItemViewModel.Number} of item variant {cartItemViewModel.ItemVariantId} removed from a cart");
//        }
//        [HttpGet]
//        public async Task<IndexViewModel<CartItemViewModel>> Index() => await base.IndexNotPagedAsync(new CartItemSpecification(_scopedParameters.ClaimsPrincipal.Identity.Name));
//        [HttpGet("{id:int}")]
//        public async Task<CartItemViewModel> Get(int id) => await base.GetAsync(new EntitySpecification<CartItem>(id));
//        [HttpPut]
//        public async Task<CartItemViewModel> Put([FromBody]CartItemViewModel cartItemViewModel) => await base.UpdateAsync(cartItemViewModel);
//        [HttpDelete("{id:int}")]
//        public async Task<Response> Delete(int id) => await base.DeleteSingleAsync(new EntitySpecification<CartItem>(id));
//        [HttpDelete]
//        public async Task<Response> Delete() => await base.DeleteAsync(new CartItemSpecification(_scopedParameters.ClaimsPrincipal.Identity.Name));
//        [HttpGet("{id:int}/checkUpdateAuthorization")]
//        public async Task<ActionResult> CheckUpdateAuthorization(int id) => await base.CheckUpdateAuthorizationAsync(id);
//    }
//}
