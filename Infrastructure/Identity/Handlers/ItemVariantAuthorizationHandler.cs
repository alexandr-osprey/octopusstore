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
    public class ItemVariantAuthorizationHandler : StoreEntityAuthorizationHandler<ItemVariant>
    {
        public ItemVariantAuthorizationHandler(UserManager<ApplicationUser> userManager, StoreContext storeContext, IAppLogger<IAuthorziationHandler<ItemVariant>> appLogger)
            : base(userManager, storeContext, appLogger)
        {
        }

        protected override async Task<Store> GetStoreEntityAsync(ItemVariant itemVariant)
        {
            var item = await _storeContext.ReadSingleBySpecAsync(_logger, new EntitySpecification<Item>(i => i.Id == itemVariant.ItemId, (i => i.Store)), true);
            return item.Store;
        }
    }
}
