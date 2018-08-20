using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ApplicationCore.Interfaces;
using Infrastructure.Data;
using Infrastructure.Identity;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace OctopusStore
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateWebHostBuilder(args);

            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var logger = services.GetRequiredService<IAppLogger<StoreContext>>();
                try
                {
                    var storeContext = services.GetRequiredService<StoreContext>();
                    StoreContextSeed.SeedStoreAsync(storeContext, logger).Wait();

                    var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
                    AppIdentityDbContextSeed.SeedAsync(userManager).Wait();
                }
                catch (Exception ex)
                {
                    logger.Error(ex, "Error seeding database");
                }
            }
            host.Run();
        }

        public static IWebHost CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .Build();
    }
}
