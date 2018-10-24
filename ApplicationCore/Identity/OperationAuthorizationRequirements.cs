using Microsoft.AspNetCore.Authorization.Infrastructure;

namespace ApplicationCore.Identity
{
    /// <summary>
    /// Authorization requirements for different types of database operations
    /// </summary>
    public class OperationAuthorizationRequirements
    {
        public static OperationAuthorizationRequirement Create =
            new OperationAuthorizationRequirement { Name = Operations.Create };
        public static OperationAuthorizationRequirement Read =
            new OperationAuthorizationRequirement { Name = Operations.Read };
        public static OperationAuthorizationRequirement Update =
            new OperationAuthorizationRequirement { Name = Operations.Update };
        public static OperationAuthorizationRequirement Delete =
            new OperationAuthorizationRequirement { Name = Operations.Delete };
    }
}
