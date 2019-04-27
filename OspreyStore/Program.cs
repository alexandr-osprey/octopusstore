using System;
using ApplicationCore.Interfaces;
using Infrastructure.Data;
using Infrastructure.Data.SampleData;
using Infrastructure.Identity;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace OspreyStore
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateWebHost(args);

            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var logger = services.GetRequiredService<IAppLogger<StoreContext>>();
                var configuration = services.GetRequiredService<IConfiguration>();
                try
                {
                    var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
                    AppIdentityDbContextSeed.SeedAsync(services, userManager).Wait();

                    var storeContext = services.GetRequiredService<StoreContext>();
                    //StoreContextSeed.SeedStoreAsync(storeContext, logger).Wait();
                    var sampleData = new TestSampleData(storeContext, configuration);
                }
                catch (Exception ex)
                {
                    logger.Error(ex, "Error seeding database");
                    throw;
                }
            }
            host.Run();
        }

        public static IWebHost CreateWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .Build();
    }
}
