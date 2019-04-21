using ApplicationCore.Identity;
using Microsoft.AspNetCore.Authorization.Infrastructure;

namespace Infrastructure.Identity.AuthorizationParameters
{
    /// <summary>
    /// Base class for enabling and disabling authorization for certain types of operations
    /// Also allows to execute one operation with authorization requirement from other (like allow read only if allowed to update)
    /// Supposed to be derived and overridden for each entity
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class AuthorizationParameters<T>: IAuthorizationParameters<T> where T: class
    {
        public virtual bool CreateAuthorizationRequired { get; set; } = true;
        public virtual bool ReadAuthorizationRequired { get; set; } = false;
        public virtual bool UpdateAuthorizationRequired { get; set; } = true;
        public virtual bool DeleteAuthorizationRequired { get; set; } = true;
        public OperationAuthorizationRequirement CreateOperationRequirement { get; set; } = OperationAuthorizationRequirements.Create;
        public OperationAuthorizationRequirement ReadOperationRequirement { get; set; } = OperationAuthorizationRequirements.Read;
        public OperationAuthorizationRequirement UpdateOperationRequirement { get; set; } = OperationAuthorizationRequirements.Update;
        public OperationAuthorizationRequirement DeleteOperationRequirement { get; set; } = OperationAuthorizationRequirements.Delete;
    }
}
