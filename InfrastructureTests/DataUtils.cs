using ApplicationCore.Entities;
using Infrastructure.Data;
using Infrastructure.Extensions;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfrastructureTests
{
    public static class DataUtils
    {
        public static StoreContext GetStoreContext()
        {
            var options = new DbContextOptionsBuilder<StoreContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            var context = new StoreContext(options);
            SeedStoreAsync(context).Wait();
            return context;
        }

        public static async Task SeedStoreAsync(StoreContext storeContext, int? retry = 0)
        {
            int retryForAvailabiltiy = retry.Value;
            try
            {
                if (!storeContext.MeasurementUnits.Any())
                {
                    storeContext.MeasurementUnits.AddRange(GetPreconfiguredMeasurementUnits());
                    await storeContext.SaveChangesAsync();
                }
                if (!storeContext.Brands.Any())
                {
                    storeContext.Brands.AddRange(GetPreconfiguredBrands());
                    await storeContext.SaveChangesAsync();
                }
                if (!storeContext.Categories.Any())
                {
                    storeContext.Categories.AddRange(GetPreconfiguredCategories());
                    await storeContext.SaveChangesAsync();
                }
                if (!storeContext.Items.Any())
                {
                    storeContext.Items.AddRange(GetPreconfiguredItems());
                    await storeContext.SaveChangesAsync();
                }
                if (!storeContext.Stores.Any())
                {
                    storeContext.Stores.AddRange(GetPreconfiguredStores());
                    await storeContext.SaveChangesAsync();
                }
                if (!storeContext.ItemsImages.Any())
                {
                    storeContext.ItemsImages.AddRange(GetPreconfiguredItemImagesAsync().Result);
                    await storeContext.SaveChangesAsync();
                }

            }
            catch (Exception ex)
            {
            }
        }
        static IEnumerable<MeasurementUnit> GetPreconfiguredMeasurementUnits()
        {
            return new List<MeasurementUnit>()
            {
                new MeasurementUnit { Title = "m" },
                new MeasurementUnit { Title = "kg" },
                new MeasurementUnit { Title = "pcs" }
            };
        }
        static IEnumerable<Brand> GetPreconfiguredBrands()
        {
            return new List<Brand>
            {
                new Brand { Title = "Apple" },
                new Brand { Title = "Samsung" },
                new Brand { Title = "CK" },
                new Brand { Title = "Armani" }
            };
        }
        static IEnumerable<Category> GetPreconfiguredCategories()
        {
            return new List<Category>
            {
                new Category { Title = "root", CanHaveItems = false },
                new Category { Title = "Electronics", CanHaveItems = false, ParentCategoryId = 1 },
                new Category { Title = "Clothes", CanHaveItems = false, ParentCategoryId = 1}
            };
        }
        static IEnumerable<Store> GetPreconfiguredStores()
        {
            return new List<Store>
            {
                new Store { Title = "John's store", Address = "NY", Description = "Electronics best deals", SellerId = "john@mail.com" },
                new Store { Title = "Jennifer's store", Address = "Sydney", Description = "Fashion", SellerId = "jennifer@mail.com" }
            };
        }
        static IEnumerable<Item> GetPreconfiguredItems()
        {
            return new List<Item>()
            {
                new Item { Title = "iPhone 6", BrandId = 1, CategoryId = 2, MeasurementUnitId = 3, Price = 1000, StoreId = 1 },
                new Item { Title = "Samsung 7", BrandId = 2, CategoryId = 2, MeasurementUnitId = 3, Price = 700, StoreId = 1 },
                new Item { Title = "Samsung 8", BrandId = 2, CategoryId = 2, MeasurementUnitId = 3, Price = 800, StoreId = 1 },

                new Item { Title = "Shoes", BrandId = 3, CategoryId = 3, MeasurementUnitId = 3, Price = 500, StoreId = 2  },
                new Item { Title = "Jacket", BrandId = 4, CategoryId = 3, MeasurementUnitId = 3, Price = 800, StoreId = 2  },
            };
        }
        static async Task<IEnumerable<FileDetails<Item>>> GetPreconfiguredItemImagesAsync()
        {
            string contentType = @"image/jpg";
            var images = new List<FileDetails<Item>>
            {
                new FileDetails<Item> ("john@mail.com", contentType, 1) { Title = "iPhone 6" },
                new FileDetails<Item> ("john@mail.com", contentType, 2) { Title = "Samsung 7" },
                new FileDetails<Item> ("john@mail.com", contentType, 3) { Title = "Samsung 8" },
                new FileDetails<Item> ("jennifer@mail.com", contentType, 4) { Title = "Shoes" },
                new FileDetails<Item> ("jennifer@mail.com", contentType, 5) { Title = "Jacket" }
            };
            foreach (var image in images) { await image.SaveFileAsync(File.OpenRead(image.FullPath)); };
            return images;
        }


        public static AppIdentityDbContext GetIdentityContext()
        {
            var options = new DbContextOptionsBuilder<AppIdentityDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            var context = new AppIdentityDbContext(options);
            var validator = new UserValidator<AppIdentityDbContext>();
            var validators = new List<UserValidator<AppIdentityDbContext>> { validator };
            IPasswordHasher<AppIdentityDbContext> hasher = new PasswordHasher<AppIdentityDbContext>();

            var userStore = new UserStore();
            var userManager = new UserManager<AppIdentityDbContext>(context, null, hasher, validators, null, null, null, null, null);
            SeedIdentityAsync(context).Wait();
            return context;
        }
        public static async Task SeedIdentityAsync(UserManager<ApplicationUser> userManager)
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
