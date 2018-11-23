using Microsoft.AspNetCore.Http;
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

        public async Task Invoke(HttpContext context)
        {
            await next(context);
            string token = GetTokenFromAuthorizationString(context.Request.Headers["Authorization"]);
            if (!string.IsNullOrWhiteSpace(token) && context.User != null)
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                tokenHandler.ReadJwtToken(token);
            }
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
