using ApplicationCore.Entities;
using ApplicationCore.Interfaces;
using Infrastructure.Data;
using Infrastructure.Identity;
using Infrastructure.Logging;
using Infrastructure.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using OctopusStore.Controllers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Xunit.Abstractions;

namespace UnitTests
{
    public abstract class TestBase<TEntity> where TEntity: Entity
    {
        public static JsonSerializerSettings jsonSettings = new JsonSerializerSettings()
        {
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
        };
        protected static ServiceCollection services = new ServiceCollection();
        protected static DbContextOptions<StoreContext> contextOptions =
            new DbContextOptionsBuilder<StoreContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
        static TestBase() {
            ConfigureDI();
        }

        protected UserManager<ApplicationUser> _userManager;
        protected ITestOutputHelper _output;
        protected int _maxTake = 200;
        protected StoreContext context;

        public TestBase(ITestOutputHelper output)
        {
            var logger = Resolve<IAppLogger<StoreContext>>();
            using (var context = Resolve<StoreContext>())
            {
                StoreContextSeed.SeedStoreAsync(context, logger).Wait();
            }
            NLog.LogManager.Configuration = new NLog.Config.XmlLoggingConfiguration("NLog.config", false);
            _output = output;
            context = Resolve<StoreContext>();
            
        }
        protected static void ConfigureDI()
        {
            services.AddSingleton(_ => contextOptions);
            services.AddDbContext<StoreContext>();
            services.AddScoped(typeof(IAppLogger<>), typeof(LoggerAdapter<>));
            services.AddScoped(typeof(IAsyncRepository<>), implementationType: typeof(EfRepository<>));
            services.AddScoped(typeof(IAppLogger<>), typeof(LoggerAdapter<>));
            services.AddScoped<IBrandService, BrandService>();
            services.AddScoped<IMeasurementUnitService, MeasurementUnitService>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<IItemVariantService, ItemVariantService>();
            services.AddScoped<IStoreService, StoreService>();
            services.AddScoped<IItemService, ItemService>();
            services.AddScoped<IItemVariantCharacteristicValueService, ItemVariantCharacteristicValueService>();
            services.AddScoped<IItemImageService, ItemImageService>();
            services.AddScoped<ICharacteristicService, CharacteristicService>();
            services.AddScoped<ICharacteristicValueService, CharacteristicValueService>();

            services.AddScoped<BrandController>();
            services.AddScoped<ItemController>();
            services.AddScoped<ItemImageController>();
            services.AddScoped<ItemVariantController>();
            services.AddScoped<ItemVariantCharacteristicValueController>();
            services.AddScoped<StoreController>();
            services.AddScoped<MeasurementUnitController>();
            services.AddScoped<CategoryController>();
            services.AddScoped<CharacteristicController>();
            services.AddScoped<CharacteristicValueController>();
        }
        protected T Resolve<T>()
        {
            var serviceProvider = services.BuildServiceProvider();
            return serviceProvider.GetRequiredService<T>();
        }
        protected virtual IQueryable<TEntity> GetQueryable(StoreContext context)
        {
            return context.Set<TEntity>().AsNoTracking();
        }

        protected async Task GetCategoryHierarchyAsync(int id, List<Category> hierarchy)
        {
            var category = await context.Categories
                .AsNoTracking()
                .Where(c => c.Id == id)
                .FirstOrDefaultAsync();
            if (category != null)
                await GetCategoryHierarchyAsync(category, hierarchy);
        }
        protected async Task GetCategoryHierarchyAsync(Category category, List<Category> hierarchy)
        {
            if (category != null)
            {
                if (!hierarchy.Contains(category))
                {
                    hierarchy.Add(category);
                    if (category.ParentCategoryId != 0)
                    {
                        await GetCategoryHierarchyAsync(category.ParentCategoryId, hierarchy);
                    }
                }
            }
        }
        protected async Task CreateItemImageCopy(ItemImage itemImage)
        {
            string fileCopy = itemImage.FullPath + "_copy";
            File.Copy(itemImage.FullPath, fileCopy);
            itemImage.FullPath = fileCopy;
            context.ItemImages.Update(itemImage);
            await context.SaveChangesAsync();
        }
    }
}
