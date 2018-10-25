using ApplicationCore.Identity;
using Microsoft.AspNetCore.Authorization.Infrastructure;

namespace Infrastructure.Identity
{
    public class AuthorizationParameters<T>: IAuthorizationParameters<T> where T: class
    {
        public virtual bool CreateAuthorizationRequired { get; set; } = true;
        public virtual bool ReadAuthorizationRequired { get; set; } = true;
        public virtual bool UpdateAuthorizationRequired { get; set; } = true;
        public virtual bool DeleteAuthorizationRequired { get; set; } = true;
        public OperationAuthorizationRequirement CreateOperationRequirement { get; set; } = OperationAuthorizationRequirements.Create;
        public OperationAuthorizationRequirement ReadOperationRequirement { get; set; } = OperationAuthorizationRequirements.Read;
        public OperationAuthorizationRequirement UpdateOperationRequirement { get; set; } = OperationAuthorizationRequirements.Update;
        public OperationAuthorizationRequirement DeleteOperationRequirement { get; set; } = OperationAuthorizationRequirements.Delete;
    }
}
