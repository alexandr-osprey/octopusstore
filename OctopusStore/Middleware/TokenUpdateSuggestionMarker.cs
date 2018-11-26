using ApplicationCore.Identity;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace OctopusStore.Middleware
{
    public class TokenUpdateSuggestionMarker
    {
        private readonly RequestDelegate next;

        public TokenUpdateSuggestionMarker(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task Invoke(HttpContext context, UserManager<ApplicationUser> userManager)
        {
            context.Response.OnStarting(async state =>
            {
                string tokenString = GetTokenFromAuthorizationString(context.Request.Headers["Authorization"]);
                var tokenHandler = new JwtSecurityTokenHandler();
                if (tokenHandler.CanReadToken(tokenString) && context.User != null)
                {
                    var token = tokenHandler.ReadJwtToken(tokenString);
                    //var id = token.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name) ?? throw new Exception($"Name claim not found at token {tokenString}");
                    var user = await userManager.FindByIdAsync(token.Subject) ?? throw new Exception($"User {token.Subject} not found");
                    var actualClaims = await userManager.GetClaimsAsync(user);
                    var httpContext = (HttpContext)state;
                    httpContext.Response.Headers.Add("Claims-Changed", "true");
                }
            }, context);
            await next(context);
        }

        private bool CheckClaimSets(IEnumerable<Claim> superset, IEnumerable<Claim> subset)
        {
            bool result = true;
            var comparer = new ClaimComparer();
            foreach(var s in subset)
            {
                result = superset.Contains(s, comparer);
                if (!result)
                    break;
            }
            return result;
        }
        private string GetTokenFromAuthorizationString(string authorization)
        {
            string result = string.Empty;
            if (authorization != null && authorization.Length > 8)
                result = authorization.Substring(7);
            //int bearerPrefixLenght = 7;
            return result;
        }
    }
}
