using Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace UnitTests
{
    public static class AppIdentityMock
    {
        public async static Task<UserManager<ApplicationUser>> ConfigureIdentity(IServiceCollection services)
        {
            services.AddDbContext<AppIdentityDbContext>(options =>
                options.UseInMemoryDatabase(Guid.NewGuid().ToString()));
            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<AppIdentityDbContext>()
                .AddDefaultTokenProviders();
            var serviceProvider = services.BuildServiceProvider();
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            await SeedIdentityAsync(userManager);
            return userManager;
        }

        private static async Task SeedIdentityAsync(UserManager<ApplicationUser> userManager)
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
