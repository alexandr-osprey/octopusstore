using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Authentication;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using ApplicationCore.Entities;
using ApplicationCore.Exceptions;
using ApplicationCore.Identity;
using ApplicationCore.Interfaces;
using ApplicationCore.Specifications;
using ApplicationCore.ViewModels;
using Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Infrastructure.Identity
{
    public class IdentityService: IIdentityService
    {
        protected readonly SignInManager<ApplicationUser> _signInManager;
        public UserManager<ApplicationUser> UserManager { get; }
        protected readonly IAuthorizationService _authorizationService;
        protected readonly IConfiguration _configuration;
        protected readonly IAppLogger<IIdentityService> _logger;
        protected readonly AppIdentityDbContext _identityContext;
        protected readonly StoreContext _storeContext;
        protected int _tokenLifetimeSeconds;
        private TokenValidationParameters _tokenValidationParameters;

        public IdentityService(
            UserManager<ApplicationUser> userManager,
            IAuthorizationService authorizationService,
            SignInManager<ApplicationUser> signInManager,
            AppIdentityDbContext identityContext,
            StoreContext storeContext,
            TokenValidationParameters tokenValidationParameters,
            IConfiguration configuration,
            IAppLogger<IIdentityService> logger)
        {
            UserManager = userManager;
            _authorizationService = authorizationService;
            _signInManager = signInManager;
            _logger = logger;
            _configuration = configuration;
            _identityContext = identityContext;
            _storeContext = storeContext;
            _tokenLifetimeSeconds = 10;
            _tokenValidationParameters = tokenValidationParameters;
        }

        public async Task<TokenPair> CreateIdentityAsync(Credentials credentials)
        {
            if (credentials == null)
                throw new ArgumentNullException(nameof(credentials));
            var user = new ApplicationUser()
            {
                UserName = credentials.Email,
                Email = credentials.Email
            };
            if (await CheckExistence(credentials.Email))
                throw new EntityValidationException($"User with email {credentials.Email} already exists");
            var result = await UserManager.CreateAsync(user, credentials.Password);
            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(user, false);
                return await GenerateTokenPair(user);
            }
            else
            {
                string errorMessages = "";
                foreach (var error in result.Errors)
                    errorMessages += error.Code + " " + error.Description + "\n";
                throw new EntityValidationException(errorMessages);
            }
        }

        public async Task<bool> CheckUserCredentialsAsync(Credentials credentials)
        {
            var user = await UserManager.FindByEmailAsync(credentials.Email);
            if (user == null)
                return false;
            var result = await UserManager.CheckPasswordAsync(user, credentials.Password);
            return result;
        }

        public async Task<TokenPair> SignInAsync(Credentials credentials)
        {
            var user = await UserManager.FindByEmailAsync(credentials.Email);
            await _signInManager.SignInAsync(user, false);
            return await GenerateTokenPair(user);
        }

        public async Task<TokenPair> CheckAndSignInAsync(Credentials credentials)
        {
            if (credentials == null)
                throw new ArgumentNullException("Credentials not provided");
            if (!await CheckUserCredentialsAsync(credentials))
                throw new AuthenticationException("Invalid credentials provided");
            return await SignInAsync(credentials);
        }

        public async Task<TokenPair> RefreshTokenAsync(string expiredToken, string refreshToken)
        {
            _tokenValidationParameters.ValidateLifetime = false;
            var principal = ValidateAndGetPrincipalFromToken(expiredToken, _tokenValidationParameters);
            var user = await UserManager.GetUserAsync(principal);
            var savedRefreshTokenSpec = new Specification<RefreshToken>(p => p.Token == refreshToken);
            var savedRefreshToken = await _identityContext.ReadSingleBySpecAsync(_logger, savedRefreshTokenSpec, false);
            if (savedRefreshToken == null)
                throw new AuthenticationException("Invalid refresh token!");
            try
            {
                _tokenValidationParameters.ValidateLifetime = true;
                ValidateAndGetPrincipalFromToken(refreshToken, _tokenValidationParameters);
            }
            catch (SecurityTokenException)
            {
                throw new SecurityTokenExpiredException("Refresh token expired");
            }
            await _identityContext.DeleteAsync(_logger, savedRefreshToken, false);
            return await GenerateTokenPair(user);
        }

        public async Task<bool> AuthorizeAsync(ClaimsPrincipal claimsPrincipal, object obj, OperationAuthorizationRequirement authorizationRequirement, bool throwException = false)
        {
            var result = await _authorizationService.AuthorizeAsync(claimsPrincipal, obj, authorizationRequirement);
            if (throwException && !result.Succeeded)
            {
                string message = $"{obj} {authorizationRequirement.Name} authorization failure";
                _logger.Trace(message);
                throw new AuthorizationException(message);
            }
            return result.Succeeded;
        }

        public async Task<bool> HasClaimAsync(string userId, Claim claim)
        {
            if (userId == null)
                throw new ArgumentNullException(nameof(userId));
            if (claim == null)
                throw new ArgumentNullException(nameof(claim));
            var user = await UserManager.FindByIdAsync(userId) ?? throw new EntityNotFoundException("user not found");
            var userClaims = await UserManager.GetClaimsAsync(user);
            bool result = userClaims.Where(uc => uc.Type == claim.Type && uc.Value == claim.Value).Any();
            return result;
        }

        public Task<bool> IsContentAdministratorAsync(string userId)
        {
            return HasClaimAsync(userId, new Claim(CustomClaimTypes.Administrator, CustomClaimValues.Content));
        }

        public Task<bool> IsStoreAdministratorAsync(string userId, int storeId)
        {
            return HasClaimAsync(userId, new Claim(CustomClaimTypes.StoreAdministrator, storeId.ToString()));
        }

        public async Task AddClaim(string id, Claim claim)
        {
            if (id == null)
                throw new ArgumentNullException("Id not provided");
            if (claim == null)
                throw new ArgumentNullException("claim not provided");
            var user = await UserManager.FindByIdAsync(id) ?? throw new EntityNotFoundException("user not found");
            user.ClaimsUpdatedAt = DateTime.UtcNow;
            await UserManager.UpdateAsync(user);
            await UserManager.AddClaimAsync(user, claim);
        }

        public async Task RemoveClaim(string id, Claim claim)
        {
            if (id == null)
                throw new ArgumentNullException("Id not provided");
            if (claim == null)
                throw new ArgumentNullException("claim not provided") ?? throw new EntityNotFoundException("user not found");
            var user = await UserManager.FindByIdAsync(id) ?? throw new EntityNotFoundException("user not found");
            user.ClaimsUpdatedAt = DateTime.UtcNow;
            await UserManager.UpdateAsync(user);
            await UserManager.RemoveClaimAsync(user, claim);
        }

        public async Task<IEnumerable<string>> EnumerateEmailsWithClaimAsync(Claim claim)
        {
            if (claim == null)
                throw new ArgumentNullException("claim not provided");
            return from u in await UserManager.GetUsersForClaimAsync(claim) select u.Email;
        }

        public async Task<string> GetUserId(string email)
        {
            if (email == null)
                throw new ArgumentNullException("email not provided");
            var user = await UserManager.FindByIdAsync(email) ?? throw new EntityNotFoundException("user not found");
            return user.Id;
        }

        public async Task RemoveFromUsersAsync(Claim claim)
        {
            var users = await UserManager.GetUsersForClaimAsync(claim ?? throw new ArgumentNullException("claim not provided"));
            foreach (var user in users)
            {
                await UserManager.RemoveClaimAsync(user, claim);
                user.ClaimsUpdatedAt = DateTime.UtcNow;
                await UserManager.UpdateAsync(user);
            }
        }

        protected async Task<bool> CheckExistence(string email) => await UserManager.FindByEmailAsync(email) != null;

        protected async Task<string> GenerateJwtToken(ApplicationUser user, int expirationTimeSeconds)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Name, user.Id)
            };
            claims.AddRange(await UserManager.GetClaimsAsync(user));
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetSection("SigningKey").Value));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires =
                DateTime.UtcNow.AddSeconds(expirationTimeSeconds);
            //DateTime.Now.AddDays(Convert.ToDouble(_configuration["ExpireDays"]));

            var cs = await UserManager.GetClaimsAsync(user);
            var token = new JwtSecurityToken(
                issuer: _configuration["Issuer"],
                audience: _configuration["Audience"],
                claims: claims,
                expires: expires,
                signingCredentials: creds,
                notBefore: DateTime.UtcNow);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        protected async Task<string> GenerateJwtToken(ApplicationUser user)
        {
            return await GenerateJwtToken(user, _tokenLifetimeSeconds);
        }

        protected async Task<string> GenerateAndSaveRefreshToken(ApplicationUser user)
        {
            var newRefreshToken = await GenerateJwtToken(user, _tokenLifetimeSeconds * 4);
            await _identityContext.CreateAsync(_logger, new RefreshToken { OwnerId = user.Id, Token = newRefreshToken }, false);
            return newRefreshToken;
        }

        protected async Task<TokenPair> GenerateTokenPair(ApplicationUser user)
        {
            return new TokenPair { Token = await GenerateJwtToken(user), RefreshToken = await GenerateAndSaveRefreshToken(user) };
        }

        protected ClaimsPrincipal ValidateAndGetPrincipalFromToken(string token, TokenValidationParameters tokenValidationParameters)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            ClaimsPrincipal principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);
            if (!(securityToken is JwtSecurityToken jwtSecurityToken) || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                throw new SecurityTokenException("Invalid or expired token");
            return principal;
        }
    }
}
