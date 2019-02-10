using System.Security.Claims;

namespace ApplicationCore.Interfaces
{
    /// <summary>
    /// Parameters (including authorization and authentication) to execute a particular request on entity
    /// </summary>
    public interface IScopedParameters
    {
        /// <summary>
        /// Specifies the user for whom authorization for operation should be checked
        /// </summary>
        ClaimsPrincipal ClaimsPrincipal { get; set; }
        string CurrentUserId { get; }
    }
}
