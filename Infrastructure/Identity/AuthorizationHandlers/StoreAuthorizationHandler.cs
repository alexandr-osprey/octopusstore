using System.Threading.Tasks;
using ApplicationCore.Entities;
using ApplicationCore.Interfaces;
using Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Identity.AuthorizationHandlers
{
    public class StoreAuthorizationHandler: StoreEntityAuthorizationHandler<Store>
    {
        public StoreAuthorizationHandler(UserManager<ApplicationUser> userManager, StoreContext storeContext, IAppLogger<IAuthorziationHandler<Store>> appLogger)
           : base(userManager, storeContext, appLogger)
        {
            _validateRightsOnEnityProperties = false;
        }


        protected override async Task<bool> CreateAsync(AuthorizationHandlerContext context, Store entity)
        {
            return base.IsOwner(context, entity) || await base.CreateAsync(context, entity);
        }

        protected override Task<int> GetStoreIdAsync(Store store) => Task.FromResult(store.Id);
    }
}
