using ApplicationCore.Entities;
using ApplicationCore.Identity;
using ApplicationCore.Interfaces;
using Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace Infrastructure.Identity
{
    public abstract class StoreEntityAuthorizationHandler<T>
       : DefaultAuthorizationHandler<T> where T: Entity
    {
        protected StoreContext _storeContext;

        public StoreEntityAuthorizationHandler(UserManager<ApplicationUser> userManager, StoreContext storeContext, IAppLogger<IAuthorziationHandler<T>> logger)
           : base(userManager, logger)
        {
            _storeContext = storeContext;
        }

        protected async override Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            OperationAuthorizationRequirement requirement,
            T entity)
        {
            await base.HandleRequirementAsync(context, requirement, entity);
            if (!context.HasSucceeded)
            {
                if (await IsStoreAdministrator(context, requirement, entity))
                {
                    context.Succeed(requirement);
                }
            }
        }

        public async Task<bool> IsStoreAdministrator(
            AuthorizationHandlerContext context,
            OperationAuthorizationRequirement requirement,
            T entity)
        {
            if (context.User == null)
                return false;

            Store store = await GetStoreEntityAsync(entity);
            if (context.User.HasClaim(CustomClaimTypes.StoreAdministrator, store.Id.ToString()))
                return true;

            return false;
        }

        protected abstract Task<Store> GetStoreEntityAsync(T entity);

        protected override async Task<bool> ValidateRightsOnEntityPropertiesAsync(AuthorizationHandlerContext context,
                                    OperationAuthorizationRequirement requirement,
                                    T entity)
        {
            if (!(context.User != null && entity != null))
                return false;
            bool result = false;
            Store store = await GetStoreEntityAsync(entity);
            if (store.OwnerId == context.User.Identity.Name || context.User.HasClaim(CustomClaimTypes.StoreAdministrator, store.Id.ToString()))
                result = true;

            return result;
        }
    }
}
