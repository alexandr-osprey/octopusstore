using ApplicationCore.Entities;
using ApplicationCore.Exceptions;
using ApplicationCore.Identity;
using ApplicationCore.Interfaces;
using ApplicationCore.Specifications;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Security.Authentication;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class CartItemService : Service<CartItem>, ICartItemService
    {
        protected string _ownerId { get; }

        public CartItemService(
            StoreContext context,
            IIdentityService identityService,
            IScopedParameters scopedParameters,
            IAuthoriationParameters<CartItem> authoriationParameters,
            IAppLogger<Service<CartItem>> logger)
            : base(context, identityService, scopedParameters, authoriationParameters, logger)
        {
            // disabling authorization requirements, because these checks are unnecessary
            // only owner of cart item may view, update and delete it (it's tied to OwnerId)
            _ownerId = _scopedParameters.ClaimsPrincipal.Identity.Name;
            if (_ownerId == null)
                throw new AuthorizationException("User is not signed in");
            _authoriationParameters.CreateAuthorizationRequired = false;
            _authoriationParameters.ReadAuthorizationRequired = false;
            _authoriationParameters.UpdateAuthorizationRequired = false;
            _authoriationParameters.DeleteAuthorizationRequired = false;
        }

        public async Task<CartItem> AddToCartAsync(int itemVariantId, int number)
        {
            var cartItem = await _context.ReadSingleBySpecAsync(_logger, new Specification<CartItem>(ci => ci.OwnerId == _ownerId && ci.ItemVariantId == itemVariantId), false);
            if (cartItem == null)
                cartItem = new CartItem()
                {
                    OwnerId = _ownerId,
                    ItemVariantId = itemVariantId,
                    Number = 0
                };
            cartItem.Number += number;
            if (cartItem.Id == 0)
                await CreateAsync(cartItem);
            else
                await UpdateAsync(cartItem);
            return cartItem;
        }
        public async Task<IEnumerable<CartItem>> EnumerateCartItemsAsync()
        {
            return await EnumerateAsync(new Specification<CartItem>(ci => ci.OwnerId == _ownerId));
        }
        public async Task RemoveFromCartAsync(int itemVariantId, int number)
        {
            var cartItem = await _context.ReadSingleBySpecAsync(_logger, new Specification<CartItem>(ci => ci.OwnerId == _ownerId && ci.ItemVariantId == itemVariantId), false);
            if (cartItem == null)
                return;
            cartItem.Number -= number;
            if (cartItem.Number <= 0)
                await DeleteSingleAsync(cartItem);
            else
                await UpdateAsync(cartItem);
        }
        public async Task ClearCartAsync()
        {
            await DeleteAsync(new Specification<CartItem>(ci => ci.OwnerId == _ownerId));
        }
        public override async Task ValidateCreateWithExceptionAsync(CartItem cartItem)
        {
            if (!await _context.ExistsBySpecAsync(_logger, new EntitySpecification<ItemVariant>(cartItem.ItemVariantId)))
                throw new BadRequestException($"Item variant {cartItem.ItemVariantId} does not exist");
            if (cartItem.Number < 0)
                throw new EntityValidationException($"Number can't be negative");
        }
        public override async Task ValidateUpdateWithExceptionAsync(CartItem cartItem)
        {
            if (cartItem.Number < 0)
                throw new EntityValidationException($"Number can't be negative");
            await base.ValidateUpdateWithExceptionAsync(cartItem);
        }
    }
}
