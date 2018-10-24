using System.Linq;
using System.Threading.Tasks;
using ApplicationCore.Entities;
using ApplicationCore.Interfaces;
using ApplicationCore.Specifications;
using Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Identity
{
    public class ItemVariantCharacteristicValueAuthorizationHandler : StoreEntityAuthorizationHandler<ItemVariantCharacteristicValue>
    {
        public ItemVariantCharacteristicValueAuthorizationHandler(UserManager<ApplicationUser> userManager, StoreContext storeContext, IAppLogger<IAuthorziationHandler<ItemVariantCharacteristicValue>> appLogger)
            : base(userManager, storeContext, appLogger)
        {
        }

        protected override async Task<Store> GetStoreEntityAsync(ItemVariantCharacteristicValue entity)
        {
            await _storeContext.ExistsBySpecAsync(_logger, new EntitySpecification<ItemVariant>(entity.ItemVariantId), true);
            var itemVariant = await _storeContext
                .NoTrackingSet<ItemVariant>()
                .Where(v => v.Id == entity.ItemVariantId)
                .Include(v => v.Item)
                        .ThenInclude(i => i.Store)
                .FirstAsync();
            return itemVariant.Item.Store;
        }
    }
}
