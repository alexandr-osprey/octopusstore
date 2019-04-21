using ApplicationCore.Entities;
using ApplicationCore.ViewModels;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ApplicationCore.Identity
{
    /// <summary>
    /// Maintains full lifecycle of identities
    /// </summary>
    public interface IIdentityService
    {
        /// <summary>
        /// Creates identity based on credentials provided. 
        /// Throws AuthenticationException in case of invalid credentials.
        /// </summary>
        /// <param name="credentials"></param>
        /// <returns></returns>
        Task<TokenPair> CreateIdentityAsync(Credentials credentials);
        /// <summary>
        /// Checks validity of credentials provided for sign in.
        /// </summary>
        /// <param name="credentials"></param>
        /// <returns></returns>
        Task<bool> CheckUserCredentialsAsync(Credentials credentials);
        /// <summary>
        /// Signs in user based on credentials provided.
        /// Doesn't check credentials.
        /// </summary>
        /// <param name="credentials"></param>
        /// <returns></returns>
        Task<TokenPair> SignInAsync(Credentials credentials);
        /// <summary>
        /// Signs in a user with provided credetials.
        /// Throws AuthenticationException if CheckUserCredentialsAsync fails.
        /// </summary>
        /// <param name="credentials"></param>
        /// <returns></returns>
        Task<TokenPair> CheckAndSignInAsync(Credentials credentials);
        /// <summary>
        /// Generates new TokenPair based on old ones.
        /// Throws AuthenticationException if wrong refresh token provided.
        /// Throws SecurityTokenExpiredException if refresh token valid, but expired.
        /// </summary>
        /// <param name="expiredToken"></param>
        /// <param name="refreshToken"></param>
        /// <param name="tokenValidationParameters"></param>
        /// <returns></returns>
        Task<TokenPair> RefreshTokenAsync(string expiredToken, string refreshToken);
        /// <summary>
        /// Authrorizes a user to perform operation on an object
        /// </summary>
        /// <param name="user"></param>
        /// <param name="obj"></param>
        /// <param name="authorizationRequirement"></param>
        /// <returns></returns>
        Task<bool> AuthorizeAsync<T>(ClaimsPrincipal claimsPrincipal, OperationAuthorizationRequirement authorizationRequirement, T finalValue, bool throwException) where T : class;
        Task<bool> HasClaimAsync(string userId, Claim claim);
        Task<bool> HasClaimAsync(string userId, string claimType, string claimValue);
        Task<bool> IsStoreAdministratorAsync(string userId, int storeId);
        Task<bool> IsContentAdministratorAsync(string userId);
        Task AddClaim(string id, Claim claim);
        Task RemoveClaim(string id, Claim claim);
        Task<string> GetUserIdAsync(string email);
        Task<string> GetUserEmailAsync(string Id);
        Task<IEnumerable<string>> EnumerateEmailsWithClaimAsync(Claim claim);
        Task RemoveFromUsersAsync(Claim claim);
    }
}
