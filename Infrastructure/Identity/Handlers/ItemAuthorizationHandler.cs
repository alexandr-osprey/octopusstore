using System.Threading.Tasks;
using ApplicationCore.Entities;
using ApplicationCore.Interfaces;
using Infrastructure.Data;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Identity
{
    public class ItemAuthorizationHandler: StoreEntityAuthorizationHandler<Item>
    {
        public ItemAuthorizationHandler(UserManager<ApplicationUser> userManager, StoreContext storeContext, IAppLogger<IAuthorziationHandler<Item>> appLogger)
           : base(userManager, storeContext, appLogger)
        {
        }

        protected override async Task<Store> GetStoreEntityAsync(Item item)
        {
            return await _storeContext.ReadByKeyAsync<Store, IAuthorziationHandler<Item>>(_logger, item.StoreId, true);
        }
    }
}
