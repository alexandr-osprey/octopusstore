using ApplicationCore.Entities;
using ApplicationCore.Identity;
using ApplicationCore.Interfaces;
using ApplicationCore.Interfaces.Controllers;
using Infrastructure;
using Infrastructure.Data;
using Infrastructure.Data.SampleData;
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
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace UnitTests
{
    public abstract class TestBase<TEntity> where TEntity: Entity
    {
        public static JsonSerializerSettings jsonSettings = new JsonSerializerSettings()
        {
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
        };
        protected static ServiceCollection Services { get; } = new ServiceCollection();


        protected static DbContextOptions<StoreContext> StoreContextOptions { get; } =
            new DbContextOptionsBuilder<StoreContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
        protected static DbContextOptions<AppIdentityDbContext> IdentityContextOptions { get; } =
            new DbContextOptionsBuilder<AppIdentityDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;

        static TestBase()
        {
            ConfigureDI();
        }


        protected IAppLogger<TestBase<TEntity>> Logger { get; }
        protected UserManager<ApplicationUser> UserManager { get; }
        protected ITestOutputHelper Output { get; }
        protected int MaxTake { get; } = 200;
        protected StoreContext Context { get; }
        protected AppIdentityDbContext IdentityContext { get; }
        protected TestSampleData Data { get; }

        public TestBase(ITestOutputHelper output)
        {
            var logger = Resolve<IAppLogger<StoreContext>>();
            Logger = Resolve<IAppLogger<TestBase<TEntity>>>();
            Context = Resolve<StoreContext>();
            
            Data = new TestSampleData(Resolve<StoreContext>());
            //Thread.Sleep(1000);
            //_context.DetachAllEntities();
            NLog.LogManager.Configuration = new NLog.Config.XmlLoggingConfiguration("NLog.config", false);
            Output = output;
            IdentityContext = Resolve<AppIdentityDbContext>();
            UserManager = Resolve<UserManager<ApplicationUser>>();
        }

        protected static async Task ConfigureIdentity()
        {
            Services.AddSingleton(IdentityContextOptions);
            //IdentityConfiguration.ConfigureTesting(services, configuration);

            Services.AddSingleton(_ => new TokenValidationParameters());
            Services.AddDbContext<AppIdentityDbContext>();
            Services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<AppIdentityDbContext>()
                .AddDefaultTokenProviders();
            Services.AddScoped<IIdentityService, IdentityService>();
            Services.AddScoped<SignInManager<ApplicationUser>>();
            Services.AddScoped<IAuthorizationService, DefaultAuthorizationService>();
            Services.AddScoped<IAuthorizationPolicyProvider, DefaultAuthorizationPolicyProvider>();
            Services.AddScoped<IAuthorizationHandlerProvider, DefaultAuthorizationHandlerProvider>();
            Services.AddScoped<IAuthorizationHandlerContextFactory, DefaultAuthorizationHandlerContextFactory>();
            Services.AddScoped<IAuthorizationEvaluator, DefaultAuthorizationEvaluator>();

            var serviceProvider = Services.BuildServiceProvider();

            AppIdentityDbContextSeed.SeedAsync(serviceProvider, Resolve<UserManager<ApplicationUser>>()).Wait();
            await AppIdentityDbContextSeed.AddClaim(Resolve<UserManager<ApplicationUser>>(),
                new Claim(CustomClaimTypes.Administrator, CustomClaimValues.Content),
                Users.AdminId);
            var signInManager = Resolve<SignInManager<ApplicationUser>>();
            signInManager.Context = new DefaultHttpContext();

            var scopedParameters = new ScopedParameters()
            {
                ClaimsPrincipal = new ClaimsPrincipal(
                    new ClaimsIdentity(
                        new List<Claim>()
                        {
                            new Claim(ClaimTypes.NameIdentifier, Users.JohnId),
                            new Claim(ClaimTypes.Name, Users.JohnId)
                        }
                   )
                )
            };
            Services.AddSingleton<IScopedParameters>(scopedParameters);

            //await signInManager.SignInAsync(
            //    await Resolve<UserManager<ApplicationUser>>().FindByIdAsync(adminId), false);
        }
        protected static void ConfigureDI()
        {
            // db options
            Services.AddSingleton(StoreContextOptions);
            //services.AddDbContext<StoreContext>(options => options.UseInMemoryDatabase(Guid.NewGuid().ToString()));

            Startup.ConfigureDI(Services);
            Services.AddSingleton(typeof(IAuthorizationParameters<>), typeof(AuthoriationParametersWithoutAuthorization<>));
            Services.AddSingleton<IAuthorizationParameters<CartItem>, AuthoriationParametersWithoutAuthorization<CartItem>>();

            var conf = new Mock<IConfiguration>();
            Services.AddSingleton(conf.Object);
            Services.AddScoped<IBrandsController, BrandsController>();
            Services.AddScoped<ICartItemsController, CartItemsController>();
            Services.AddScoped<IdentityController>();
            Services.AddScoped<IItemsController, ItemsController>();
            Services.AddScoped<IItemImagesController, ItemImagesController>();
            Services.AddScoped<IItemVariantsController, ItemVariantsController>();
            Services.AddScoped<IItemPropertiesController, ItemPropertiesController>();
            //services.AddScoped<StoresController>();
            //services.AddScoped<MeasurementUnitsController>();
            Services.AddScoped<ICategoriesController, CategoriesController>();
            Services.AddScoped<ICharacteristicsController, CharacteristicsController>();
            Services.AddScoped<ICharacteristicValuesController, CharacteristicValuesController>();

            ConfigureIdentity().Wait();
        }
        protected static T Resolve<T>()
        {
            ServiceProvider serviceProvider = Services.BuildServiceProvider();
            return serviceProvider.GetRequiredService<T>();
        }
        protected virtual IQueryable<TEntity> GetQueryable()
        {
            return Context.Set<TEntity>().AsNoTracking();
        }

        protected async Task GetCategoryHierarchyAsync(int id, HashSet<Category> hierarchy)
        {
            await GetCategoryParentsAsync(id, hierarchy);
            await GetCategorySubcategoriesAsync(id, hierarchy);
        }
        protected async Task GetCategoryParentsAsync(int categoryId, HashSet<Category> hierarchy)
        {
            var category = await Context
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
            var category = await Context
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
            Context.Set<ItemImage>().Update(itemImage);
            await Context.SaveChangesAsync();
        }
    }
}
