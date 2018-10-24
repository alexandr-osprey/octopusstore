using Microsoft.AspNetCore.Authorization.Infrastructure;

namespace ApplicationCore.Identity
{
    /// <summary>
    /// Describes authorization requirements for entities: whether authorization should be checked and what requirement against
    /// (for example, perform read operation with evaluating Update rights)
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IAuthoriationParameters<T> where T: class
    {
        /// <summary>
        /// Specifies if authorization for Create operation should be checked
        /// </summary>
        bool CreateAuthorizationRequired { get; set; }
        /// <summary>
        /// Specifies if authorization for Read operation should be checked
        /// </summary>
        bool ReadAuthorizationRequired { get; set; }
        /// <summary>
        /// Specifies if authorization for Update operation should be checked
        /// </summary>
        bool UpdateAuthorizationRequired { get; set; }
        /// <summary>
        /// Specifies if authorization for Delete operation should be checked
        /// </summary>
        bool DeleteAuthorizationRequired { get; set; }
        /// <summary>
        /// Requirement to check during Create operation. Create requirement by default.
        /// </summary>
        OperationAuthorizationRequirement CreateOperationRequirement { get; set; }
        /// <summary>
        /// Requirement to check during Read operation. Read requirement by default.
        /// </summary>
        OperationAuthorizationRequirement ReadOperationRequirement { get; set; }
        /// <summary>
        /// Requirement to check during Update operation. Update requirement by default.
        /// </summary>
        OperationAuthorizationRequirement UpdateOperationRequirement { get; set; }
        /// <summary>
        /// Requirement to check during Delete operation. Delete requirement by default.
        /// </summary>
        OperationAuthorizationRequirement DeleteOperationRequirement { get; set; }
    }
}
