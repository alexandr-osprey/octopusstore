using ApplicationCore.Entities;
using ApplicationCore.Identity;
using ApplicationCore.Interfaces;
using Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace Infrastructure.Identity.AuthorizationHandlers
{
    public abstract class StoreEntityAuthorizationHandler<T>: DefaultAuthorizationHandler<T> where T: Entity
    {
        protected StoreContext _storeContext;

        public StoreEntityAuthorizationHandler(UserManager<ApplicationUser> userManager, StoreContext storeContext, IAppLogger<IAuthorziationHandler<T>> logger)
           : base(userManager, logger)
        {
            _validateRightsOnEnityProperties = true;
            _storeContext = storeContext;
        }

        protected override async Task<bool> CreateAsync(AuthorizationHandlerContext context, T entity) => IsAuthenticated(context) && (await IsStoreAdministratorAsync(context, entity) || IsContentAdministrator(context));
        protected override async Task<bool> UpdateAsync(AuthorizationHandlerContext context, T entity) => IsAuthenticated(context) && (await IsStoreAdministratorAsync(context, entity) || IsContentAdministrator(context));
        protected override async Task<bool> DeleteAsync(AuthorizationHandlerContext context, T entity) => IsAuthenticated(context) && (await IsStoreAdministratorAsync(context, entity) || IsContentAdministrator(context));

        public async Task<bool> IsStoreAdministratorAsync(
            AuthorizationHandlerContext context,
            T entity)
        {
            if (context.User == null)
                return false;

            int storeId = await GetStoreIdAsync(entity);
            if (context.User.HasClaim(CustomClaimTypes.StoreAdministrator, storeId.ToString()))
                return true;

            return false;
        }

        protected abstract Task<int> GetStoreIdAsync(T entity);

        protected override async Task<bool> ValidateRightsOnEntityPropertiesAsync(AuthorizationHandlerContext context,
                                    OperationAuthorizationRequirement requirement,
                                    T entity)
        {
            if (base.IsContentAdministrator(context.User))
                return true;
            bool result = false;
            int storeId = await GetStoreIdAsync(entity);
            if (context.User.HasClaim(CustomClaimTypes.StoreAdministrator, storeId.ToString()))
                result = true;

            return result;
        }
    }
}
