using ApplicationCore.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Infrastructure.Identity
{
    public class AppIdentityDbContextSeed
    {
        public static string Password = "Password1!";
        public static async Task SeedAsync(IServiceProvider serviceProvider, UserManager<ApplicationUser> userManager)
        {
            var defaultUser = new ApplicationUser { UserName = "demouser@microsoft.com", Email = "demouser@microsoft.com" };
            await userManager.CreateAsync(defaultUser, "Passw@rd1");
            List<string> emails = new List<string>()
            {
                "user3@mail.com",
                "user4@mail.com",
                "user5@mail.com",
                "user6@mail.com",
            };
            emails.ForEach(async e =>
            {
                var user = await EnsureUser(serviceProvider, Password, e, e);
                var cl = new Claim(CustomClaimTypes.Buyer, user.Id);
                //await userManager.AddClaimAsync(user, cl);
            });
            var john = await EnsureUser(serviceProvider, Password, "john@mail.com", "john@mail.com");
            //await userManager.AddClaimAsync(john, new Claim(CustomClaimTypes.Seller, john.Id));
            var jennifer = await EnsureUser(serviceProvider, Password, "jennifer@mail.com", "jennifer@mail.com");
            //await userManager.AddClaimAsync(jennifer, new Claim(CustomClaimTypes.Seller, jennifer.Id));
            var admin = await EnsureUser(serviceProvider, Password, "admin@mail.com", "admin@mail.com");
        }
        private static async Task<ApplicationUser> EnsureUser(IServiceProvider serviceProvider,
                                            string testUserPw, string email, string username = "")
        {
            var userManager = serviceProvider.GetService<UserManager<ApplicationUser>>();
            var user = await userManager.FindByEmailAsync(email);
            if (user == null)
            {
               user = new ApplicationUser { Email = email, UserName = username, Id = email };
               var res =  await userManager.CreateAsync(user, testUserPw);
                await userManager.AddClaimAsync(user, new Claim(ClaimTypes.NameIdentifier, email));
                await userManager.AddClaimAsync(user, new Claim(ClaimTypes.Name, email));
            }
            return await userManager.FindByEmailAsync(email);
        }

        public static async Task AddClaim(UserManager<ApplicationUser> userManager, Claim claim, params string[] userIds)
        {
            foreach(var user in userIds)
            {
                await userManager.AddClaimAsync(await userManager.FindByIdAsync(user), claim);
            }
             
        }
        private static async Task<IdentityResult> EnsureRole(IServiceProvider serviceProvider,
                                                                      string email, string role)
        {
            IdentityResult IR = null;
            var roleManager = serviceProvider.GetService<RoleManager<IdentityRole>>();
            if (roleManager == null)
                throw new Exception("roleManager null");
            if (!await roleManager.RoleExistsAsync(role))
                IR = await roleManager.CreateAsync(new IdentityRole(role));
            var userManager = serviceProvider.GetService<UserManager<ApplicationUser>>();
            var user = await userManager.FindByEmailAsync(email);
            IR = await userManager.AddToRoleAsync(user, role);
            return IR;
        }
    }
}
