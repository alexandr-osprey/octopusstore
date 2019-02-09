using System.Threading.Tasks;
using ApplicationCore.Entities;
using ApplicationCore.Interfaces;
using ApplicationCore.Specifications;
using Infrastructure.Data;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Identity
{
    public class ItemVariantAuthorizationHandler: StoreEntityAuthorizationHandler<ItemVariant>
    {
        public ItemVariantAuthorizationHandler(UserManager<ApplicationUser> userManager, StoreContext storeContext, IAppLogger<IAuthorziationHandler<ItemVariant>> appLogger)
           : base(userManager, storeContext, appLogger)
        {
        }

        protected override async Task<int> GetStoreIdAsync(ItemVariant itemVariant)
        {
            var item = await _storeContext.ReadSingleBySpecAsync(_logger, new EntitySpecification<Item>(itemVariant.ItemId), true);
            return item.StoreId;
        }
    }
}
