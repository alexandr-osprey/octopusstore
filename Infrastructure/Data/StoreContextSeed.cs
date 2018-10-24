using ApplicationCore.Entities;
using ApplicationCore.Interfaces;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Data
{
    public class StoreContextSeed
    {
        /// <summary>
        /// Filling test data in context
        /// </summary>
        /// <param name="storeContext"></param>
        /// <param name="logger"></param>
        /// <returns></returns>
        public static async Task SeedStoreAsync(StoreContext storeContext, IAppLogger<StoreContext> logger)
        {
            var instance = new StoreContextSeed();
            instance._logger = logger;
            if (!storeContext.MeasurementUnits.Any())
            {
                instance.measurementUnits = instance.GetPreconfiguredMeasurementUnits();
                storeContext.MeasurementUnits.AddRange(instance.measurementUnits);
                await storeContext.SaveChangesAsync();
            }
            if (!storeContext.Brands.Any())
            {
                instance.brands = instance.GetPreconfiguredBrands();
                storeContext.Brands.AddRange(instance.brands);
                await storeContext.SaveChangesAsync();
            }
            if (!storeContext.Categories.Any())
            {
                instance.categories = instance.GetPreconfiguredCategories();
                storeContext.Categories.AddRange(instance.categories);
                await storeContext.SaveChangesAsync();
            }
            if (!storeContext.Stores.Any())
            {
                instance.stores = instance.GetPreconfiguredStores();
                storeContext.Stores.AddRange(instance.stores);
                await storeContext.SaveChangesAsync();
            }
            if (!storeContext.Items.Any())
            {
                instance.items = instance.GetPreconfiguredItems();
                storeContext.Items.AddRange(instance.items);
                await storeContext.SaveChangesAsync();
            }
            if (!storeContext.ItemImages.Any())
            {
                instance.itemsImages = instance.GetPreconfiguredItemImageDetails();
                storeContext.ItemImages.AddRange(instance.itemsImages);
                await storeContext.SaveChangesAsync();
            }
            if (!storeContext.Characteristics.Any())
            {
                instance.characteristics = instance.GetPreconfiguredCharacteristics();
                storeContext.Characteristics.AddRange(instance.characteristics);
                await storeContext.SaveChangesAsync();
            }
            if (!storeContext.CharacteristicValues.Any())
            {
                instance.characteristicValues = instance.GetPreconfiguredCharacteristicValues();
                storeContext.CharacteristicValues.AddRange(instance.characteristicValues);
                await storeContext.SaveChangesAsync();
            }
            if (!storeContext.ItemVariants.Any())
            {
                instance.itemVariants = instance.GetPreconfiguredItemVariants();
                storeContext.ItemVariants.AddRange(instance.itemVariants);
                await storeContext.SaveChangesAsync();
            }
            if (!storeContext.ItemVariantCharacteristicValues.Any())
            {
                instance.itemVariantCharacteristicValues = instance.GetPreconfiguredItemCharacteristicValues();
                storeContext.ItemVariantCharacteristicValues.AddRange(instance.itemVariantCharacteristicValues);
                await storeContext.SaveChangesAsync();
            }
        }

        private List<MeasurementUnit> measurementUnits;
        private List<Brand> brands;
        private List<Category> categories;
        private List<Characteristic> characteristics;
        private List<CharacteristicValue> characteristicValues;
        private List<Store> stores;
        private List<Item> items;
        private List<ItemImage> itemsImages;
        private List<ItemVariant> itemVariants;
        private List<ItemVariantCharacteristicValue> itemVariantCharacteristicValues;
        private IAppLogger<StoreContext> _logger;

        string johnId = "john@mail.com";
        string jenniferId = "jennifer@mail.com";

        private List<MeasurementUnit> GetPreconfiguredMeasurementUnits()
        {
            return new List<MeasurementUnit>()
            {
                new MeasurementUnit { Title = "m" },
                new MeasurementUnit { Title = "kg" },
                new MeasurementUnit { Title = "pcs" }
            };
        }
        private List<Brand> GetPreconfiguredBrands()
        {
            return new List<Brand>
            {
                new Brand { Title = "Apple" },
                new Brand { Title = "Samsung" },
                new Brand { Title = "Pebble"},

                new Brand { Title = "CK" },
                new Brand { Title = "Armani" }
            };
        }
        private List<Category> GetPreconfiguredCategories()
        {
            return new List<Category>
            {
                new Category { Title = "Categories", CanHaveItems = true },

                new Category { Title = "Electronics",  ParentCategoryId = 1, CanHaveItems = false }, //1
                new Category { Title = "Smartphones",  ParentCategoryId = 2, CanHaveItems = true}, //2
                new Category { Title = "Smartwatches", ParentCategoryId = 2, CanHaveItems = true}, //3

                new Category { Title = "Clothes",  ParentCategoryId = 1, CanHaveItems = false}, //4
                new Category { Title = "Shoes", ParentCategoryId = 5, CanHaveItems = true}, //5
                new Category { Title = "Jackets", ParentCategoryId = 5, CanHaveItems = true}, //6
            };
        }
        private List<Characteristic> GetPreconfiguredCharacteristics()
        {
            var characteristics = new List<Characteristic>
            {
                new Characteristic { Title = "Storage", CategoryId = categories[1].Id }, //0
                new Characteristic { Title = "Resolution", CategoryId = categories[1].Id  }, //1
                new Characteristic { Title = "Battery", CategoryId = categories[3].Id  },//2

                new Characteristic { Title = "Size", CategoryId = categories[4].Id }, //3
                new Characteristic { Title = "Fashion", CategoryId = categories[4].Id }, //4
                new Characteristic { Title = "Colour", CategoryId = categories[6].Id }, //5
            };
            return characteristics;
        }
        private List<CharacteristicValue> GetPreconfiguredCharacteristicValues()
        {
            var categoryPropertyValues = new List<CharacteristicValue>
            {
                new CharacteristicValue { Title = "16GB",  CharacteristicId = characteristics[0].Id }, //0
                new CharacteristicValue { Title = "32GB",  CharacteristicId = characteristics[0].Id }, //1
                new CharacteristicValue { Title = "64GB",  CharacteristicId = characteristics[0].Id }, //2
                new CharacteristicValue { Title = "HD",  CharacteristicId = characteristics[1].Id }, //3
                new CharacteristicValue { Title = "Full HD",  CharacteristicId = characteristics[1].Id }, //4
                new CharacteristicValue { Title = "1000 mAh",  CharacteristicId = characteristics[2].Id }, //5
                new CharacteristicValue { Title = "2000 mAh",  CharacteristicId = characteristics[2].Id }, //6

                new CharacteristicValue { Title = "X",  CharacteristicId = characteristics[3].Id }, //7
                new CharacteristicValue { Title = "XL",  CharacteristicId = characteristics[3].Id }, //8
                new CharacteristicValue { Title = "XXL",  CharacteristicId = characteristics[3].Id }, //9
                new CharacteristicValue { Title = "Much fashion",  CharacteristicId = characteristics[4].Id }, //10
                new CharacteristicValue { Title = "Not so fashion",  CharacteristicId = characteristics[4].Id }, //11
                new CharacteristicValue { Title = "Black",  CharacteristicId = characteristics[5].Id }, //12
                new CharacteristicValue { Title = "White",  CharacteristicId = characteristics[5].Id }, //13
            };
            return categoryPropertyValues;
        }
        private List<Store> GetPreconfiguredStores()
        {
            return new List<Store>
            {
                new Store { Title = "John's store", Address = "NY", Description = "Electronics best deals", OwnerId = johnId },
                new Store { Title = "Jennifer's store", Address = "Sydney", Description = "Fashion", OwnerId = jenniferId }
            };
        }
        private List<Item> GetPreconfiguredItems()
        {
            return new List<Item>()
            {
                new Item { Title = "iPhone 6", BrandId = brands[0].Id, CategoryId = categories[2].Id,
                    MeasurementUnitId = measurementUnits[2].Id, StoreId = stores[0].Id, OwnerId = stores[0].OwnerId },
                new Item { Title = "Samsung 7", BrandId = brands[1].Id, CategoryId = categories[2].Id,
                    MeasurementUnitId = measurementUnits[2].Id,StoreId = stores[0].Id, OwnerId = stores[0].OwnerId },
                new Item { Title = "Samsung 8", BrandId = brands[1].Id, CategoryId = categories[2].Id,
                    MeasurementUnitId = measurementUnits[2].Id, StoreId = stores[0].Id, OwnerId = stores[0].OwnerId },
                new Item { Title = "Pebble Watch", BrandId = brands[2].Id, CategoryId = categories[3].Id,
                    MeasurementUnitId = measurementUnits[2].Id, StoreId = stores[0].Id, OwnerId = stores[0].OwnerId },

                new Item { Title = "Shoes", BrandId = brands[3].Id, CategoryId = categories[5].Id,
                    MeasurementUnitId = measurementUnits[2].Id, StoreId = stores[1].Id, OwnerId = stores[1].OwnerId },
                new Item { Title = "Jacket", BrandId = brands[4].Id, CategoryId = categories[6].Id,
                    MeasurementUnitId = measurementUnits[2].Id, StoreId = stores[1].Id, OwnerId = stores[1].OwnerId },
            };
        }
        private List<ItemImage> GetPreconfiguredItemImageDetails()
        {
            string contentType = @"image/jpeg";
            var imagesDetails = new List<ItemImage>
            {
                new ItemImage ("iPhone 6 - 1", johnId, contentType, items[0].Id, null) { FullPath= Path.Combine(@"C:\files\", johnId, "iPhone 6 - 1.jpg") },
                new ItemImage ("iPhone 6 - 2.jpg", johnId, contentType, items[0].Id, null) { FullPath= Path.Combine(@"C:\files\", johnId, "iPhone 6 - 2.jpg") },
                new ItemImage ("iPhone 6 - 3.jpg", johnId, contentType, items[0].Id, null) { FullPath= Path.Combine(@"C:\files\", johnId, "iPhone 6 - 3.jpg") },
                new ItemImage ("Samsung 7.jpg", johnId, contentType, items[1].Id, null) { FullPath= Path.Combine(@"C:\files\", johnId, "Samsung 7.jpg") },
                new ItemImage ("Samsung 8.jpg", johnId, contentType, items[2].Id, null) {  FullPath= Path.Combine(@"C:\files\", johnId, "Samsung 8.jpg") },
                new ItemImage ("Pebble.jpg", johnId, contentType, items[3].Id, null) {  FullPath= Path.Combine(@"C:\files\", johnId, "Pebble.jpg") },

                new ItemImage ("Shoes.jpg", jenniferId, contentType, items[4].Id, null) { FullPath= Path.Combine(@"C:\files\", jenniferId, "Shoes.jpg") },
                new ItemImage ("Jacket.jpg", jenniferId,  contentType, items[5].Id, null) { FullPath= Path.Combine(@"C:\files\", jenniferId, "Jacket.jpg") }
            };
            return imagesDetails;
        }
        private List<ItemVariant> GetPreconfiguredItemVariants()
        {
            var itemVariants = new List<ItemVariant>
            {
                new ItemVariant { Title = "iPhone 6 32GB", Price = 700, ItemId = items[0].Id, OwnerId = johnId }, //0
                new ItemVariant { Title = "iPhone 6 64GB", Price = 800, ItemId = items[0].Id, OwnerId = johnId }, //1
                new ItemVariant { Title = "Samsung 7 32GB HD", Price = 500, ItemId = items[1].Id, OwnerId = johnId }, //2
                new ItemVariant { Title = "Samsung 7 32GB Full HD", Price = 550, ItemId = items[1].Id, OwnerId = johnId }, //3
                new ItemVariant { Title = "Samsung 8 32GB HD", Price = 700, ItemId = items[2].Id, OwnerId = johnId },  //4
                new ItemVariant { Title = "Samsung 8 32GB Full HD", Price = 750, ItemId = items[2].Id, OwnerId = johnId }, //5
                new ItemVariant { Title = "Pebble 1000mAh", Price = 400, ItemId = items[3].Id, OwnerId = johnId  },

                new ItemVariant { Title = "Shoes X Much fashion", Price = 700, ItemId = items[4].Id, OwnerId = jenniferId  }, //6
                new ItemVariant { Title = "Shoes XXL Much fashion", Price = 700, ItemId = items[4].Id, OwnerId = jenniferId  }, //7
                new ItemVariant { Title = "Jacket black", Price = 450, ItemId = items[5].Id, OwnerId = jenniferId  }, //8
                new ItemVariant { Title = "Jacket white", Price = 500, ItemId = items[5].Id, OwnerId = jenniferId  },  //9

            };
            return itemVariants;
        }
        private List<ItemVariantCharacteristicValue> GetPreconfiguredItemCharacteristicValues()
        {
            var itemPropertyValues = new List<ItemVariantCharacteristicValue>
            {
                // iphone 32
                new ItemVariantCharacteristicValue (
                    itemVariants[0].Id,
                    characteristicValues[1].Id
                ) { OwnerId = johnId },
                new ItemVariantCharacteristicValue (
                    itemVariants[0].Id,
                    characteristicValues[3].Id
                ) { OwnerId = johnId },
                // iphone 64
                new ItemVariantCharacteristicValue (
                    itemVariants[1].Id,
                    characteristicValues[2].Id
                ) { OwnerId = johnId },
                new ItemVariantCharacteristicValue (
                    itemVariants[1].Id,
                    characteristicValues[4].Id
                ) { OwnerId = johnId },
                //samsung 7 32 hd
                new ItemVariantCharacteristicValue (
                    itemVariants[2].Id,
                    characteristicValues[1].Id
                ) { OwnerId = johnId },
                new ItemVariantCharacteristicValue (
                    itemVariants[2].Id,
                    characteristicValues[3].Id
                ) { OwnerId = johnId },
                //samsung 7 32 full hd
                new ItemVariantCharacteristicValue (
                    itemVariants[3].Id,
                    characteristicValues[1].Id
                ) { OwnerId = johnId },
                new ItemVariantCharacteristicValue (
                    itemVariants[3].Id,
                    characteristicValues[4].Id
                ) { OwnerId = johnId },
                //samsung 8 64 hd
                new ItemVariantCharacteristicValue (
                    itemVariants[4].Id,
                    characteristicValues[2].Id
                ) { OwnerId = johnId },
                new ItemVariantCharacteristicValue (
                    itemVariants[4].Id,
                    characteristicValues[3].Id
                ) { OwnerId = johnId },
                //samsung 8 64 full hd
                new ItemVariantCharacteristicValue (
                    itemVariants[5].Id,
                    characteristicValues[2].Id
                ) { OwnerId = johnId },
                new ItemVariantCharacteristicValue (
                    itemVariants[5].Id,
                    characteristicValues[4].Id
                ) { OwnerId = johnId },
                //pebble 100mAh
                new ItemVariantCharacteristicValue (
                    itemVariants[6].Id,
                    characteristicValues[5].Id
                ) { OwnerId = johnId },

                //shoes X much fashion
                new ItemVariantCharacteristicValue (
                    itemVariants[7].Id,
                    characteristicValues[7].Id
                ) { OwnerId = jenniferId },
                new ItemVariantCharacteristicValue (
                    itemVariants[7].Id,
                    characteristicValues[10].Id
                ) { OwnerId = jenniferId },
                //shoes XXL much fashion
                new ItemVariantCharacteristicValue (
                    itemVariants[8].Id,
                    characteristicValues[9].Id
                ) { OwnerId = jenniferId },
                new ItemVariantCharacteristicValue (
                    itemVariants[8].Id,
                    characteristicValues[10].Id
                ) { OwnerId = jenniferId },
                //jacket black
                new ItemVariantCharacteristicValue (
                    itemVariants[9].Id,
                    characteristicValues[12].Id
                ) { OwnerId = jenniferId },
                //jacket white
                new ItemVariantCharacteristicValue (
                    itemVariants[9].Id,
                    characteristicValues[13].Id
                ) { OwnerId = jenniferId },
            };
            return itemPropertyValues;
        }
        private string GetIdByEmail(UserManager<ApplicationUser> userManager, string email)
        {
            return userManager.FindByEmailAsync(email).Result.Id;
        }
    }
}
