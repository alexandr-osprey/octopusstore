using ApplicationCore.Entities;
using ApplicationCore.Identity;
using ApplicationCore.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Infrastructure.Identity
{
    public abstract class DefaultAuthorizationHandler<T>: AuthorizationHandler<OperationAuthorizationRequirement, T> where T: Entity
    {
        protected UserManager<ApplicationUser> _userManager;
        protected IAppLogger<IAuthorziationHandler<T>> _logger;

        public DefaultAuthorizationHandler(UserManager<ApplicationUser> userManager, IAppLogger<IAuthorziationHandler<T>> logger)
        {
            _userManager = userManager;
            _logger = logger;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, OperationAuthorizationRequirement requirement, T entity)
        {
            if (await DefaultChecksAsync(context, requirement, entity))
            {
                // check entity validity on create and update operations
                if ((requirement.Name == Operations.Create || requirement.Name == Operations.Update))
                {
                    if (await ValidateRightsOnEntityPropertiesAsync(context, requirement, entity))
                        context.Succeed(requirement);
                }
                else
                {
                    context.Succeed(requirement);
                }
            }
        }

        protected async Task<bool> DefaultChecksAsync(AuthorizationHandlerContext context, OperationAuthorizationRequirement requirement, T entity)
        {
            switch(requirement.Name)
            {
                case Operations.Read:
                    return await ReadAsync(context, entity);
                case Operations.Create:
                    return await CreateAsync(context, entity);
                case Operations.Update:
                    return await UpdateAsync(context, entity);
                case Operations.Delete:
                    return await DeleteAsync(context, entity);
                default:
                    return false;
            }
        }

        protected virtual Task<bool> ReadAsync(AuthorizationHandlerContext context, T entity) => Task.FromResult(true);
        protected virtual Task<bool> CreateAsync(AuthorizationHandlerContext context, T entity) => Task.FromResult(IsAuthenticated(context));
        protected virtual Task<bool> UpdateAsync(AuthorizationHandlerContext context, T entity) => Task.FromResult(IsAuthenticated(context) && (IsOwner(context, entity) || IsContentAdministrator(context)));
        protected virtual Task<bool> DeleteAsync(AuthorizationHandlerContext context, T entity) => Task.FromResult(IsAuthenticated(context) && (IsOwner(context, entity) || IsContentAdministrator(context)));

        protected virtual bool ReadAny(AuthorizationHandlerContext context, OperationAuthorizationRequirement requirement) => true;

        protected virtual bool IsAuthenticated(AuthorizationHandlerContext context) => context?.User != null;

        protected virtual bool IsOwner(AuthorizationHandlerContext context, T entity)
        {
            if (context?.User == null)
                return false;
            if (entity.OwnerId == _userManager.GetUserId(context.User))
                return true;
            return false;
        }

        protected virtual bool IsContentAdministrator(AuthorizationHandlerContext context) => IsContentAdministrator(context?.User);

        /// <summary>
        /// Ensures that a user allowed set properites of an entity.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="requirement"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        protected virtual async Task<bool> ValidateRightsOnEntityPropertiesAsync(AuthorizationHandlerContext context, OperationAuthorizationRequirement requirement, T entity) => await Task.FromResult(true);

        protected bool IsContentAdministrator(ClaimsPrincipal claimsPrincipal)
        {
            if (claimsPrincipal != null && claimsPrincipal.HasClaim(CustomClaimTypes.Administrator, CustomClaimValues.Content))
                return true;
            return false;
        }
    }
}
