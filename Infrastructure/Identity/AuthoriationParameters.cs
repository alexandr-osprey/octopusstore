using ApplicationCore.Identity;
using ApplicationCore.Interfaces;
using Microsoft.AspNetCore.Authorization.Infrastructure;

namespace Infrastructure.Identity
{
    public class AuthoriationParameters<T> : IAuthoriationParameters<T> where T: class
    {
        public bool CreateAuthorizationRequired { get; set; } = true;
        public bool ReadAuthorizationRequired { get; set; } = true;
        public bool UpdateAuthorizationRequired { get; set; } = true;
        public bool DeleteAuthorizationRequired { get; set; } = true;
        public OperationAuthorizationRequirement CreateOperationRequirement { get; set; } = OperationAuthorizationRequirements.Create;
        public OperationAuthorizationRequirement ReadOperationRequirement { get; set; } = OperationAuthorizationRequirements.Read;
        public OperationAuthorizationRequirement UpdateOperationRequirement { get; set; } = OperationAuthorizationRequirements.Update;
        public OperationAuthorizationRequirement DeleteOperationRequirement { get; set; } = OperationAuthorizationRequirements.Delete;
    }
}
