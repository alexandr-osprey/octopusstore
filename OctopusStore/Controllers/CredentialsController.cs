using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using ApplicationCore.ViewModels;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

namespace OctopusStore.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class CredentialsController : Controller
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _configuration;
        private readonly ILogger _logger;

        public CredentialsController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IConfiguration configuration, 
            ILogger<CredentialsController> logger
            )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _configuration = configuration;
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> PostAsync([FromBody]Credentials accountCreateViewModel)
        {
            var user = new ApplicationUser()
            {
                UserName = accountCreateViewModel.Email,
                Email = accountCreateViewModel.Email
            };
            var result = await _userManager.CreateAsync(user, accountCreateViewModel.Password);

            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(user, false);
                return Ok(new { token = GenerateJwtToken(accountCreateViewModel.Email, user) });
            }
            else
            {
                return BadRequest(result.Errors.ToString());
            }
        }


        private string GenerateJwtToken(string email, ApplicationUser user)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.Id)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["SigingKey"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.Now.AddDays(Convert.ToDouble(_configuration["ExpireDays"]));

            var token = new JwtSecurityToken(
                issuer: _configuration["Issuer"],
                audience: _configuration["Audience"],
                claims: claims,
                expires: expires,
                signingCredentials: creds);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        [HttpGet]
        [Authorize]
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
    }
}
