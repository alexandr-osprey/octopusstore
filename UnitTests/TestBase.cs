using ApplicationCore.Entities;
using ApplicationCore.Identity;
using ApplicationCore.Interfaces;
using Infrastructure;
using Infrastructure.Data;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Moq;
using Newtonsoft.Json;
using OctopusStore;
using OctopusStore.Controllers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace UnitTests
{
    public abstract class TestBase<TEntity> where TEntity : Entity
    {
        public static JsonSerializerSettings jsonSettings = new JsonSerializerSettings()
        {
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
        };
        protected static ServiceCollection services = new ServiceCollection();


        protected static DbContextOptions<StoreContext> storeContextOptions =
            new DbContextOptionsBuilder<StoreContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
        protected static DbContextOptions<AppIdentityDbContext> identityContextOptions =
            new DbContextOptionsBuilder<AppIdentityDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
        static TestBase()
        {
            ConfigureDI();
        }


        protected IAppLogger<TestBase<TEntity>> _logger;
        protected UserManager<ApplicationUser> _userManager;
        protected ITestOutputHelper _output;
        protected int _maxTake = 200;
        protected StoreContext context;
        protected AppIdentityDbContext _identityContext;
        protected static string johnId = "john@mail.com";

        public TestBase(ITestOutputHelper output)
        {
            var logger = Resolve<IAppLogger<StoreContext>>();
            _logger = Resolve<IAppLogger<TestBase<TEntity>>>();
            context = Resolve<StoreContext>();
            StoreContextSeed.SeedStoreAsync(Resolve<StoreContext>(), logger).Wait();
            NLog.LogManager.Configuration = new NLog.Config.XmlLoggingConfiguration("NLog.config", false);
            _output = output;
            _identityContext = Resolve<AppIdentityDbContext>();
            _userManager = Resolve<UserManager<ApplicationUser>>();
        }

        protected static async Task ConfigureIdentity()
        {
            //IdentityConfiguration.ConfigureTesting(services, configuration);
            services.AddSingleton(identityContextOptions);
            services.AddSingleton(_ => new TokenValidationParameters());
            services.AddDbContext<AppIdentityDbContext>();
            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<AppIdentityDbContext>()
                .AddDefaultTokenProviders();
            services.AddScoped<IIdentityService, IdentityService>();
            services.AddScoped<SignInManager<ApplicationUser>>();
            services.AddScoped<IAuthorizationService, DefaultAuthorizationService>();
            services.AddScoped<IAuthorizationPolicyProvider, DefaultAuthorizationPolicyProvider>();
            services.AddScoped<IAuthorizationHandlerProvider, DefaultAuthorizationHandlerProvider>();
            services.AddScoped<IAuthorizationHandlerContextFactory, DefaultAuthorizationHandlerContextFactory>();
            services.AddScoped<IAuthorizationEvaluator, DefaultAuthorizationEvaluator>();

            var serviceProvider = services.BuildServiceProvider();

            string adminId = "admin@mail.com";
            AppIdentityDbContextSeed.SeedAsync(serviceProvider, Resolve<UserManager<ApplicationUser>>()).Wait();
            await AppIdentityDbContextSeed.AddClaim(Resolve<UserManager<ApplicationUser>>(),
                new Claim(CustomClaimTypes.Administrator, CustomClaimValues.Content),
                adminId);
            var signInManager = Resolve<SignInManager<ApplicationUser>>();
            signInManager.Context = new DefaultHttpContext();

            var scopedParameters = new ScopedParameters()
            {
                ClaimsPrincipal = new ClaimsPrincipal(
                    new ClaimsIdentity(
                        new List<Claim>()
                        {
                            new Claim(ClaimTypes.NameIdentifier, johnId),
                            new Claim(ClaimTypes.Name, johnId)
                        }
                   )
                )
            };
            services.AddSingleton<IScopedParameters>(scopedParameters);

            //await signInManager.SignInAsync(
            //    await Resolve<UserManager<ApplicationUser>>().FindByIdAsync(adminId), false);
        }
        protected static void ConfigureDI()
        {
            // db options
            services.AddSingleton(storeContextOptions);

            Startup.ConfigureDI(services);
            services.AddScoped(typeof(IAuthoriationParameters<>), typeof(AuthoriationParametersWithoutAuthorization<>));

            var conf = new Mock<IConfiguration>();
            services.AddSingleton(conf.Object);
            services.AddScoped<BrandsController>();
            services.AddScoped<IdentityController>();
            services.AddScoped<ItemsController>();
            services.AddScoped<ItemImagesController>();
            services.AddScoped<ItemVariantsController>();
            services.AddScoped<ItemVariantCharacteristicValuesController>();
            services.AddScoped<StoresController>();
            services.AddScoped<MeasurementUnitsController>();
            services.AddScoped<CategoriesController>();
            services.AddScoped<CharacteristicsController>();
            services.AddScoped<CharacteristicValuesController>();

            ConfigureIdentity().Wait();
        }
        protected static T Resolve<T>()
        {
            ServiceProvider serviceProvider = services.BuildServiceProvider();
            return serviceProvider.GetRequiredService<T>();
        }
        protected virtual IQueryable<TEntity> GetQueryable(DbContext context)
        {
            return context.Set<TEntity>().AsNoTracking();
        }

        protected async Task GetCategoryHierarchyAsync(int id, HashSet<Category> hierarchy)
        {
            await GetCategoryParentsAsync(id, hierarchy);
            await GetCategorySubcategoriesAsync(id, hierarchy);
        }
        protected async Task GetCategoryParentsAsync(int categoryId, HashSet<Category> hierarchy)
        {
            var category = await context
                .Set<Category>()
                .AsNoTracking()
                .Where(c => c.Id == categoryId)
                .FirstOrDefaultAsync();
            if (category != null)
            {
                hierarchy.Add(category);
                if (category.ParentCategoryId != 0)
                    await GetCategoryParentsAsync(category.ParentCategoryId, hierarchy);
            }
        }
        public async Task GetCategorySubcategoriesAsync(int categoryId, HashSet<Category> hierarchy)
        {
            var category = await context
                .Set<Category>()
                .AsNoTracking()
                .Where(c => c.Id == categoryId)
                .Include(c => c.Subcategories)
                .FirstOrDefaultAsync();
            hierarchy.Add(category);

            foreach (var subCategory in category.Subcategories)
            {
                await GetCategorySubcategoriesAsync(subCategory.Id, hierarchy);
            }
        }

        protected void Equal<T>(IEnumerable<T> expected, IEnumerable<T> actual)
        {
            Assert.Equal(
                JsonConvert.SerializeObject(expected, Formatting.None, jsonSettings),
                JsonConvert.SerializeObject(actual, Formatting.None, jsonSettings));
        }

        protected void Equal<T>(T expected, T actual)
        {
            Assert.Equal(
                JsonConvert.SerializeObject(expected, Formatting.None, jsonSettings),
                JsonConvert.SerializeObject(actual, Formatting.None, jsonSettings));
        }
        protected async Task CreateItemImageCopy(ItemImage itemImage)
        {
            string fileCopy = itemImage.FullPath + "_copy";
            if (!File.Exists(fileCopy))
                File.Copy(itemImage.FullPath, fileCopy);
            itemImage.FullPath = fileCopy;
            context.Set<ItemImage>().Update(itemImage);
            await context.SaveChangesAsync();
        }
    }
}
