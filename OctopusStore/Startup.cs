using ApplicationCore.Interfaces;
using Infrastructure.Data;
using Infrastructure.Logging;
using Infrastructure.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using Microsoft.Extensions.Logging;
using OctopusStore.Middleware;
using Microsoft.AspNetCore.Mvc.Authorization;
//using Infrastructure.Identity;
using Infrastructure;
using ApplicationCore.Identity;
using Infrastructure.Identity;
using ApplicationCore.Entities;
using ApplicationCore.Interfaces.Services;

namespace OctopusStore
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        private IServiceCollection _services;

        public void ConfigureDevelopmentServices(IServiceCollection services)
        {
            DbContextOptions<StoreContext> storeContextOptions =
                new DbContextOptionsBuilder<StoreContext>()
                    //.UseLazyLoadingProxies()
                    .UseInMemoryDatabase("Store").Options;
            services.AddSingleton(storeContextOptions);
            IdentityConfiguration.ConfigureTesting(services);
            ConfigureCommonServices(services);
        }

        //public void ConfigureTestingServices(IServiceCollection services)
        //{

        //}

        public void ConfigureProductionServices(IServiceCollection services)
        {
            try
            {
                DbContextOptions<StoreContext> storeContextOptions =
                    new DbContextOptionsBuilder<StoreContext>()
                        .UseSqlServer(Configuration.GetConnectionString("StoreConnection")).Options;
                services.AddSingleton(storeContextOptions);
                IdentityConfiguration.ConfigureProduction(services, Configuration);
            }
            catch (Exception ex)
            {
                var message = ex.Message;
            }

            //services.AddDbContext<AppIdentityDbContext>(options =>
            //    options.UseSqlServer(Configuration.GetConnectionString("IdentityConnection")));

            ConfigureCommonServices(services);
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureCommonServices(IServiceCollection services)
        {

            ConfigureDI(services);
            IdentityConfiguration.ConfigureServices(services, Configuration);

            services.AddMemoryCache();
            services.AddMvc(
                config =>
                {
                    config.Filters.Add(new AuthorizeFilter(IdentityConfiguration.AuthorizationPolicy));
                })
                .AddJsonOptions(
                    options => options.SerializerSettings.ReferenceLoopHandling
                        = Newtonsoft.Json.ReferenceLoopHandling.Ignore);

            // In production, the Angular files will be served from this directory
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/dist";
            });
            _services = services;
        }

        public static void ConfigureDI(IServiceCollection services)
        {
            services.AddDbContext<StoreContext>();
            services.AddSingleton<IActivatorService, ActivatorService>();
            services.AddScoped(typeof(IAppLogger<>), typeof(AppLogger<>));
            services.AddScoped<IScopedParameters, ScopedParameters>();
            services.AddScoped<IBrandService, BrandService>();
            services.AddScoped<IMeasurementUnitService, MeasurementUnitService>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<IItemVariantService, ItemVariantService>();
            services.AddScoped<IStoreService, StoreService>();
            services.AddScoped<IItemService, ItemService>();
            services.AddScoped<IItemPropertyService, ItemPropertyService>();
            services.AddScoped<IItemImageService, ItemImageService>();
            services.AddScoped<ICharacteristicService, CharacteristicService>();
            services.AddScoped<ICharacteristicValueService, CharacteristicValueService>();
            services.AddScoped<ICartItemService, CartItemService>();
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<IOrderItemService, OrderItemService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseSpaStaticFiles();

            app.UseMiddleware(typeof(ErrorHandlingMiddleware));
            app.UseAuthentication();
            app.UseMiddleware(typeof(TokenUpdateSuggestionMarker));
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller}/{action=Index}/{id?}");
            });
            

            app.UseSpa(spa =>
            {
                // To learn more about options for serving an Angular SPA from ASP.NET Core,
                // see https://go.microsoft.com/fwlink/?linkid=864501

                spa.Options.SourcePath = "ClientApp";

                if (env.IsDevelopment())
                {
                    spa.UseAngularCliServer(npmScript: "start");
                }
            });
        }
    }
}
