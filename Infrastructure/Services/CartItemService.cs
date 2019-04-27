using ApplicationCore.Entities;
using ApplicationCore.Exceptions;
using ApplicationCore.Identity;
using ApplicationCore.Interfaces;
using ApplicationCore.Interfaces.Services;
using ApplicationCore.Specifications;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class CartItemService: Service<CartItem>, ICartItemService
    {
        public CartItemService(
            StoreContext context,
            IIdentityService identityService,
            IScopedParameters scopedParameters,
            IAuthorizationParameters<CartItem> authoriationParameters,
            IAppLogger<Service<CartItem>> logger)
           : base(context, identityService, scopedParameters, authoriationParameters, logger)
        {
        }

        public async Task<CartItem> AddToCartAsync(string ownerId, int itemVariantId, int number)
        {
            var cartItem = await _сontext.ReadSingleBySpecAsync(_logger, new CartItemSpecification(ownerId, itemVariantId), false);
            cartItem = cartItem ?? new CartItem() { OwnerId = ownerId, ItemVariantId = itemVariantId, Number = 0 };
            cartItem.Number += number;
            return cartItem.Id == 0 ? await CreateAsync(cartItem) : await UpdateAsync(cartItem);
        }

        public async Task RemoveFromCartAsync(string ownerId, int itemVariantId, int number)
        {
            var cartItem = await _сontext.ReadSingleBySpecAsync(_logger, new CartItemSpecification(ownerId, itemVariantId), false);
            if (cartItem == null)
                return;
            cartItem.Number -= number;
            if (cartItem.Number <= 0)
                await DeleteSingleAsync(cartItem);
            else
                await UpdateAsync(cartItem);
        }

        protected override async Task ValidateCustomUniquinessWithException(CartItem cartItem)
        {
            await base.ValidateCustomUniquinessWithException(cartItem);
            if (await _сontext.ExistsBySpecAsync(_logger, new CartItemSpecification(cartItem.OwnerId, cartItem.ItemVariantId)))
                throw new EntityAlreadyExistsException($"Cart item with variant {cartItem.ItemVariantId} already exists");
        }

        protected override async Task ValidateWithExceptionAsync(EntityEntry<CartItem> entityEntry)
        {
            await base.ValidateWithExceptionAsync(entityEntry);
            var cartItem = entityEntry.Entity;
            if (IsPropertyModified(entityEntry, c => c.ItemVariantId, false) 
                && !await _сontext.ExistsBySpecAsync(_logger, new EntitySpecification<ItemVariant>(cartItem.ItemVariantId)))
                throw new EntityValidationException($"Item variant {cartItem.ItemVariantId} does not exist");
            if (cartItem.Number <= 0)
                throw new EntityValidationException($"Number can't be negative");
        }
    }
}
