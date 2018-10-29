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
    public class ItemPropertyAuthorizationHandler: StoreEntityAuthorizationHandler<ItemProperty>
    {
        public ItemPropertyAuthorizationHandler(UserManager<ApplicationUser> userManager, StoreContext storeContext, IAppLogger<IAuthorziationHandler<ItemProperty>> appLogger)
           : base(userManager, storeContext, appLogger)
        {
        }

        protected override async Task<Store> GetStoreEntityAsync(ItemProperty entity)
        {
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
