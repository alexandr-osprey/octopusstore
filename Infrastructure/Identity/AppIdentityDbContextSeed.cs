using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Infrastructure.Identity
{
    public class AppIdentityDbContextSeed
    {
        public static async Task SeedAsync(UserManager<ApplicationUser> userManager)
        {
            var defaultUser = new ApplicationUser { UserName = "demouser@microsoft.com", Email = "demouser@microsoft.com" };
            await userManager.CreateAsync(defaultUser, "Pass@word1");

            var users = new List<ApplicationUser>
            {
                new ApplicationUser { UserName = "john@mail.com", Email = "john@mail.com" },
                new ApplicationUser { UserName = "jennifer@mail.com", Email = "jennifer@mail.com" },
                new ApplicationUser { UserName = "user3@mail.com", Email = "user3@mail.com" },
                new ApplicationUser { UserName = "user4@mail.com", Email = "user4@mail.com" },
                new ApplicationUser { UserName = "user5@mail.com", Email = "user5@mail.com" },
                new ApplicationUser { UserName = "user6@mail.com", Email = "user6@mail.com" },
            };

            users.ForEach(async u => await userManager.CreateAsync(u, "Password1"));
        }
    }
}
