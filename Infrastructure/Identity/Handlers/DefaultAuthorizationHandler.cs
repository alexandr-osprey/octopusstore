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
    public abstract class DefaultAuthorizationHandler<T> 
       : AuthorizationHandler<OperationAuthorizationRequirement, T> where T: Entity
    {
        protected UserManager<ApplicationUser> _userManager;
        protected IAppLogger<IAuthorziationHandler<T>> _logger;

        public DefaultAuthorizationHandler(UserManager<ApplicationUser> userManager, IAppLogger<IAuthorziationHandler<T>> logger)
        {
            _userManager = userManager;
            _logger = logger;
        }

        protected override async Task
            HandleRequirementAsync(AuthorizationHandlerContext context,
                                    OperationAuthorizationRequirement requirement,
                                    T entity)
        {
            if (DefaultChecks(context, requirement, entity))
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

        protected bool DefaultChecks(AuthorizationHandlerContext context,
                                    OperationAuthorizationRequirement requirement,
                                    T entity)
        {
            return ReadAny(context, requirement)
                    || CreateAuthorized(context, requirement)
                    || UpdateDeleteOwner(context, requirement, entity)
                    || UpdateDeleteContentAdministrator(context, requirement);
        }

        public bool ReadAny(AuthorizationHandlerContext context, 
            OperationAuthorizationRequirement requirement)
        {
            if (requirement.Name == Operations.Read)
                return true;
            return false;
        }

        public bool CreateAuthorized(AuthorizationHandlerContext context,
                                    OperationAuthorizationRequirement requirement)
        {
            if (requirement.Name == Operations.Create 
                && context.User != null)
                return true;
            return false;
        }

        public bool UpdateDeleteOwner(AuthorizationHandlerContext context,
                                    OperationAuthorizationRequirement requirement,
                                    T entity)
        {
            if (!(requirement.Name == Operations.Update || requirement.Name == Operations.Delete)
                || context.User == null)
                return false;
            if (entity.OwnerId == _userManager.GetUserId(context.User))
                return true;
            return false;
        }

        public bool UpdateDeleteContentAdministrator(AuthorizationHandlerContext context,
                                    OperationAuthorizationRequirement requirement)
        {
            if (!(requirement.Name == Operations.Update || requirement.Name == Operations.Delete)
                || context.User == null)
                return false;
            if (context.User.HasClaim(CustomClaimTypes.Administrator, CustomClaimValues.Content))
                return true;
            return false;
        }

        /// <summary>
        /// Ensures that a user allowed set properites of an entity.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="requirement"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        protected virtual async Task<bool> ValidateRightsOnEntityPropertiesAsync(AuthorizationHandlerContext context,
                                    OperationAuthorizationRequirement requirement,
                                    T entity)
        {
            return await Task.FromResult(true);
        }
    }
}
