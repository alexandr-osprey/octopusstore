using System.Collections.Generic;
using System.Security.Authentication;
using System.Threading.Tasks;
using ApplicationCore.Exceptions;
using ApplicationCore.Identity;
using ApplicationCore.Interfaces;
using ApplicationCore.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace OctopusStore.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class IdentityController: Controller
    {
        private IAppLogger<IdentityController> _logger { get; }
        protected IIdentityService _service { get; }
        //protected IConfiguration _configuration { get;  }
        //private readonly int _tokenLifetimeSeconds;

        public IdentityController(
            IIdentityService identityService,
            TokenValidationParameters tokenValidationParameters,
            IAppLogger<IdentityController> logger)
        {
            _logger = logger;
            _service = identityService;
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<TokenPair> CreateIdentity([FromBody]Credentials credentials)
        {
            if (credentials == null) throw new BadRequestException("Credentials not provided");
            return await _service.CreateIdentityAsync(credentials);
        }
        [AllowAnonymous]
        [HttpPost("signIn")]
        public async Task<TokenPair> SignInAsync([FromBody]Credentials credentials)
        {
            if (credentials == null) throw new BadRequestException("Credentials not provided");
            return await _service.CheckAndSignInAsync(credentials);
        }
        [AllowAnonymous]
        [HttpPost("refreshToken")]
        public async Task<TokenPair> RefreshToken([FromHeader]string authorization, [FromHeader]string refreshToken)
        {
            if (authorization == null) throw new AuthenticationException("Authorization token not provided");
            if (refreshToken == null) throw new AuthenticationException("Refresh token not provided");
            string expiredToken = GetTokenFromAuthorizationString(authorization);
            return await _service.RefreshTokenAsync(expiredToken, refreshToken);
        }
        
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }
        [HttpGet("anon")]
        [AllowAnonymous]
        public IEnumerable<string> GetAnon()
        {
            return new string[] { "value1Anon", "value2Anon" };
        }
        private string GetTokenFromAuthorizationString(string authorization)
        {
            //int bearerPrefixLenght = 7;
            return authorization.Substring(7);
        }
    }
}
