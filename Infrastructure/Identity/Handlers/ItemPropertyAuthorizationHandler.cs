using System.Linq;
using System.Threading.Tasks;
using ApplicationCore.Entities;
using ApplicationCore.Interfaces;
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

        protected override async Task<int> GetStoreIdAsync(ItemProperty entity)
        {
            var itemVariant = await _storeContext
                .Set<ItemVariant>()
                .Include(v => v.Item)
                .FirstAsync(v => v.Id == entity.ItemVariantId);
            return itemVariant.Item.StoreId;
        }
    }
}
