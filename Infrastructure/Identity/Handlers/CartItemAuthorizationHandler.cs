using System.Threading.Tasks;
using ApplicationCore.Entities;
using ApplicationCore.Interfaces;
using ApplicationCore.Specifications;
using Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Identity
{
    public class CartItemAuthorizationHandler : StoreEntityAuthorizationHandler<CartItem>
    {
        public CartItemAuthorizationHandler(UserManager<ApplicationUser> userManager, StoreContext storeContext, IAppLogger<IAuthorziationHandler<CartItem>> appLogger): base(userManager, storeContext, appLogger)
        {
            ValidateRightsOnEnityProperties = false;
        }

        protected override async Task<int> GetStoreIdAsync(CartItem cartItem)
        {
            var itemVariant = await _storeContext.ReadSingleBySpecAsync(_logger, new Specification<ItemVariant>(v => v.Id == cartItem.ItemVariantId, v => v.Item), true);
            return itemVariant.Item.StoreId;
        }

        protected override Task<bool> CreateAsync(AuthorizationHandlerContext context, CartItem cartItem) => Task.FromResult(IsAuthenticated(context));
        protected override async Task<bool> ReadAsync(AuthorizationHandlerContext context, CartItem cartItem) => IsAuthenticated(context) && (IsOwner(context, cartItem) || await IsStoreAdministratorAsync(context, cartItem));
        protected override async Task<bool> UpdateAsync(AuthorizationHandlerContext context, CartItem cartItem) => IsAuthenticated(context) && (IsOwner(context, cartItem) || await IsStoreAdministratorAsync(context, cartItem));
        protected override async Task<bool> DeleteAsync(AuthorizationHandlerContext context, CartItem cartItem) => IsAuthenticated(context) && (IsOwner(context, cartItem) || await IsStoreAdministratorAsync(context, cartItem));
    }
}
