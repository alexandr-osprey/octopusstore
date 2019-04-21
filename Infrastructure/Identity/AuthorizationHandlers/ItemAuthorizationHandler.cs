using System.Threading.Tasks;
using ApplicationCore.Entities;
using ApplicationCore.Interfaces;
using Infrastructure.Data;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Identity.AuthorizationHandlers
{
    public class ItemAuthorizationHandler: StoreEntityAuthorizationHandler<Item>
    {
        public ItemAuthorizationHandler(UserManager<ApplicationUser> userManager, StoreContext storeContext, IAppLogger<IAuthorziationHandler<Item>> appLogger)
           : base(userManager, storeContext, appLogger)
        {
        }

        protected override Task<int> GetStoreIdAsync(Item item) => Task.FromResult(item.StoreId);
    }
}
