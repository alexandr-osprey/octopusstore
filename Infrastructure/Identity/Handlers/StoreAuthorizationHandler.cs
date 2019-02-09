using System.Threading.Tasks;
using ApplicationCore.Entities;
using ApplicationCore.Interfaces;
using Infrastructure.Data;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Identity
{
    public class StoreAuthorizationHandler: StoreEntityAuthorizationHandler<Store>
    {
        public StoreAuthorizationHandler(UserManager<ApplicationUser> userManager, StoreContext storeContext, IAppLogger<IAuthorziationHandler<Store>> appLogger)
           : base(userManager, storeContext, appLogger)
        {
        }

        protected override Task<int> GetStoreIdAsync(Store store) => Task.FromResult(store.Id);
    }
}
