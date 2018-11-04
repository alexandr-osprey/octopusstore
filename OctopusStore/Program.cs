using System;
using ApplicationCore.Interfaces;
using Infrastructure.Data;
using Infrastructure.Data.SampleData;
using Infrastructure.Identity;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

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
                    var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
                    AppIdentityDbContextSeed.SeedAsync(services, userManager).Wait();

                    var storeContext = services.GetRequiredService<StoreContext>();
                    //StoreContextSeed.SeedStoreAsync(storeContext, logger).Wait();
                    var sampleData = new TestSampleData(storeContext);
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
