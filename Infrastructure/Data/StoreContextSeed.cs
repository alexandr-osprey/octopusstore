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
            //var instance = new StoreContextSeed();
            _logger = logger;
            if (!storeContext.MeasurementUnits.Any())
            {
                storeContext.MeasurementUnits.AddRange(PreconfiguredMeasurementUnits);
                await storeContext.SaveChangesAsync();
            }
            if (!storeContext.Brands.Any())
            {
                storeContext.Brands.AddRange(PreconfiguredBrands);
                await storeContext.SaveChangesAsync();
            }
            if (!storeContext.Categories.Any())
            {
                storeContext.Categories.AddRange(PreconfiguredCategories);
                await storeContext.SaveChangesAsync();
            }
            if (!storeContext.Stores.Any())
            {
                storeContext.Stores.AddRange(PreconfiguredStores);
                await storeContext.SaveChangesAsync();
            }
            if (!storeContext.Items.Any())
            {
                storeContext.Items.AddRange(PreconfiguredItems);
                await storeContext.SaveChangesAsync();
            }
            if (!storeContext.ItemImages.Any())
            {
                storeContext.ItemImages.AddRange(PreconfiguredItemImageDetail);
                await storeContext.SaveChangesAsync();
            }
            if (!storeContext.Characteristics.Any())
            {
                storeContext.Characteristics.AddRange(PreconfiguredCharacteristics);
                await storeContext.SaveChangesAsync();
            }
            if (!storeContext.CharacteristicValues.Any())
            {

                storeContext.CharacteristicValues.AddRange(PreconfiguredCharacteristicValues);
                await storeContext.SaveChangesAsync();
            }
            if (!storeContext.ItemVariants.Any())
            {
                storeContext.ItemVariants.AddRange(PreconfiguredItemVariants);
                await storeContext.SaveChangesAsync();
            }
            if (!storeContext.ItemProperties.Any())
            {
                storeContext.ItemProperties.AddRange(PreconfiguredItemProperties);
                await storeContext.SaveChangesAsync();
            }
            if (!storeContext.CartItems.Any())
            {
                storeContext.CartItems.AddRange(PreconfiguredCartItems);
                await storeContext.SaveChangesAsync();
            }
            //throw new Exception($"itemsImages == null { itemsImages == null}");
            EnsureFilesAreInPlace();
        }

        private static IAppLogger<StoreContext> _logger;
        private static string adminId = "admin@mail.com";
        private static string johnId = "john@mail.com";
        private static string jenniferId = "jennifer@mail.com";
        private static readonly string pathToFiles = @"C:\files\";
        private static readonly string pathToBackup = @"C:\files\backup";
        private static readonly string itemImageContentType = @"image/jpeg";

        private static List<MeasurementUnit> _PreconfiguredMeasurementUnits;
        private static List<MeasurementUnit> PreconfiguredMeasurementUnits
        {
            get
            {
                if (_PreconfiguredMeasurementUnits == null)
                {
                    _PreconfiguredMeasurementUnits = new List<MeasurementUnit>()
                    {
                        new MeasurementUnit { Title = "m", OwnerId = adminId },
                        new MeasurementUnit { Title = "kg", OwnerId = adminId },
                        new MeasurementUnit { Title = "pcs", OwnerId = adminId }
                    };
                }
                return _PreconfiguredMeasurementUnits;
            }
        }

        private static List<Brand> _PreconfiguredBrands;
        private static List<Brand> PreconfiguredBrands
        {
            get
            {
                if (_PreconfiguredBrands == null)
                {
                    _PreconfiguredBrands = new List<Brand>
                    {
                        new Brand { Title = "Apple", OwnerId = adminId },
                        new Brand { Title = "Samsung", OwnerId = adminId },
                        new Brand { Title = "Pebble", OwnerId = adminId},
                        new Brand { Title = "CK", OwnerId = adminId },
                        new Brand { Title = "Armani", OwnerId = adminId }
                    };
                }
                return _PreconfiguredBrands;
            }
        }
        private static List<Category> _PreconfiguredCategories;
        private static List<Category> PreconfiguredCategories
        {
            get
            {
                if (_PreconfiguredCategories == null)
                {
                    _PreconfiguredCategories = new List<Category>
                    {
                        new Category { Title = "Categories", CanHaveItems = true, OwnerId = adminId },

                        new Category { Title = "Electronics",  ParentCategoryId = 1, CanHaveItems = false, OwnerId = adminId }, //1
                        new Category { Title = "Smartphones",  ParentCategoryId = 2, CanHaveItems = true, OwnerId = adminId}, //2
                        new Category { Title = "Smartwatches", ParentCategoryId = 2, CanHaveItems = true, OwnerId = adminId}, //3

                        new Category { Title = "Clothes",  ParentCategoryId = 1, CanHaveItems = false, OwnerId = adminId}, //4
                        new Category { Title = "Shoes", ParentCategoryId = 5, CanHaveItems = true, OwnerId = adminId}, //5
                        new Category { Title = "Jackets", ParentCategoryId = 5, CanHaveItems = true, OwnerId = adminId}, //6
                    };
                }
                return _PreconfiguredCategories;
            }
        }
        private static List<Characteristic> _PreconfiguredCharacteristics;
        private static List<Characteristic> PreconfiguredCharacteristics
        {
            get
            {
                if (_PreconfiguredCharacteristics == null)
                {
                    _PreconfiguredCharacteristics = new List<Characteristic>
                    {
                        new Characteristic { Title = "Storage", CategoryId = PreconfiguredCategories[1].Id, OwnerId = adminId }, //0
                        new Characteristic { Title = "Resolution", CategoryId = PreconfiguredCategories[1].Id, OwnerId = adminId  }, //1
                        new Characteristic { Title = "Battery", CategoryId = PreconfiguredCategories[3].Id, OwnerId = adminId  },//2

                        new Characteristic { Title = "Size", CategoryId = PreconfiguredCategories[4].Id, OwnerId = adminId }, //3
                        new Characteristic { Title = "Fashion", CategoryId = PreconfiguredCategories[4].Id, OwnerId = adminId }, //4
                        new Characteristic { Title = "Colour", CategoryId = PreconfiguredCategories[6].Id, OwnerId = adminId }, //5
                    };
                }
                return _PreconfiguredCharacteristics;
            }
        }
        private static List<CharacteristicValue> _PreconfiguredCharacteristicValues;
        private static List<CharacteristicValue> PreconfiguredCharacteristicValues
        {
            get
            {
                if (_PreconfiguredCharacteristicValues == null)
                {
                    _PreconfiguredCharacteristicValues = new List<CharacteristicValue>
                    {
                        new CharacteristicValue { Title = "16GB",  CharacteristicId = PreconfiguredCharacteristics[0].Id, OwnerId = adminId }, //0
                        new CharacteristicValue { Title = "32GB",  CharacteristicId = PreconfiguredCharacteristics[0].Id, OwnerId = adminId }, //1
                        new CharacteristicValue { Title = "64GB",  CharacteristicId = PreconfiguredCharacteristics[0].Id, OwnerId = adminId }, //2
                        new CharacteristicValue { Title = "HD",  CharacteristicId = PreconfiguredCharacteristics[1].Id, OwnerId = adminId }, //3
                        new CharacteristicValue { Title = "Full HD",  CharacteristicId = PreconfiguredCharacteristics[1].Id, OwnerId = adminId }, //4
                        new CharacteristicValue { Title = "1000 mAh",  CharacteristicId = PreconfiguredCharacteristics[2].Id, OwnerId = adminId }, //5
                        new CharacteristicValue { Title = "2000 mAh",  CharacteristicId = PreconfiguredCharacteristics[2].Id, OwnerId = adminId }, //6

                        new CharacteristicValue { Title = "X",  CharacteristicId = PreconfiguredCharacteristics[3].Id, OwnerId = adminId }, //7
                        new CharacteristicValue { Title = "XL",  CharacteristicId = PreconfiguredCharacteristics[3].Id, OwnerId = adminId }, //8
                        new CharacteristicValue { Title = "XXL",  CharacteristicId = PreconfiguredCharacteristics[3].Id, OwnerId = adminId }, //9
                        new CharacteristicValue { Title = "Much fashion",  CharacteristicId = PreconfiguredCharacteristics[4].Id, OwnerId = adminId }, //10
                        new CharacteristicValue { Title = "Not so fashion",  CharacteristicId = PreconfiguredCharacteristics[4].Id, OwnerId = adminId }, //11
                        new CharacteristicValue { Title = "Black",  CharacteristicId = PreconfiguredCharacteristics[5].Id, OwnerId = adminId }, //12
                        new CharacteristicValue { Title = "White",  CharacteristicId = PreconfiguredCharacteristics[5].Id, OwnerId = adminId }, //13
                    };
                }
                return _PreconfiguredCharacteristicValues;
            }
        }
        private static List<Store> _PreconfiguredStores;
        private static List<Store> PreconfiguredStores
        {
            get
            {
                if (_PreconfiguredStores == null)
                {
                    _PreconfiguredStores = new List<Store>
                    {
                        new Store { Title = "John's store", Address = "NY", Description = "Electronics best deals", OwnerId = johnId },
                        new Store { Title = "Jennifer's store", Address = "Sydney", Description = "Fashion", OwnerId = jenniferId }
                    };
                }
                return _PreconfiguredStores;
            }
        }
        private static List<Item> _PreconfiguredItems;
        private static List<Item> PreconfiguredItems
        {
            get
            {
                if (_PreconfiguredItems == null)
                {
                    _PreconfiguredItems = new List<Item>()
                    {
                        new Item { Title = "iPhone 6", BrandId = PreconfiguredBrands[0].Id, CategoryId = PreconfiguredCategories[2].Id,
                            MeasurementUnitId = PreconfiguredMeasurementUnits[2].Id, StoreId = PreconfiguredStores[0].Id, OwnerId = PreconfiguredStores[0].OwnerId },
                        new Item { Title = "Samsung 7", BrandId = PreconfiguredBrands[1].Id, CategoryId = PreconfiguredCategories[2].Id,
                            MeasurementUnitId = PreconfiguredMeasurementUnits[2].Id,StoreId = PreconfiguredStores[0].Id, OwnerId = PreconfiguredStores[0].OwnerId },
                        new Item { Title = "Samsung 8", BrandId = PreconfiguredBrands[1].Id, CategoryId = PreconfiguredCategories[2].Id,
                            MeasurementUnitId = PreconfiguredMeasurementUnits[2].Id, StoreId = PreconfiguredStores[0].Id, OwnerId = PreconfiguredStores[0].OwnerId },
                        new Item { Title = "Pebble Watch", BrandId = PreconfiguredBrands[2].Id, CategoryId = PreconfiguredCategories[3].Id,
                            MeasurementUnitId = PreconfiguredMeasurementUnits[2].Id, StoreId = PreconfiguredStores[0].Id, OwnerId = PreconfiguredStores[0].OwnerId },

                        new Item { Title = "Shoes", BrandId = PreconfiguredBrands[3].Id, CategoryId = PreconfiguredCategories[5].Id,
                            MeasurementUnitId = PreconfiguredMeasurementUnits[2].Id, StoreId = PreconfiguredStores[1].Id, OwnerId = PreconfiguredStores[1].OwnerId },
                        new Item { Title = "Jacket", BrandId = PreconfiguredBrands[4].Id, CategoryId = PreconfiguredCategories[6].Id,
                            MeasurementUnitId = PreconfiguredMeasurementUnits[2].Id, StoreId = PreconfiguredStores[1].Id, OwnerId = PreconfiguredStores[1].OwnerId },
                    };
                }
                return _PreconfiguredItems;
            }
        }
        private static List<ItemImage> _PreconfiguredItemImageDetail;
        private static List<ItemImage> PreconfiguredItemImageDetail
        {
            get
            {
                if (_PreconfiguredItemImageDetail == null)
                {
                    _PreconfiguredItemImageDetail = new List<ItemImage>
                    {
                        new ItemImage ("iPhone 6 - 1", itemImageContentType, PreconfiguredItems[0].Id, null) { OwnerId = johnId, FullPath = Path.Combine(pathToFiles, johnId, "iPhone 6 - 1.jpg"),  },
                        new ItemImage ("iPhone 6 - 2.jpg", itemImageContentType, PreconfiguredItems[0].Id, null) { OwnerId = johnId, FullPath = Path.Combine(pathToFiles, johnId, "iPhone 6 - 2.jpg") },
                        new ItemImage ("iPhone 6 - 3.jpg", itemImageContentType, PreconfiguredItems[0].Id, null) { OwnerId = johnId, FullPath = Path.Combine(pathToFiles, johnId, "iPhone 6 - 3.jpg") },
                        new ItemImage ("Samsung 7.jpg", itemImageContentType, PreconfiguredItems[1].Id, null) { OwnerId = johnId, FullPath = Path.Combine(pathToFiles, johnId, "Samsung 7.jpg") },
                        new ItemImage ("Samsung 8.jpg", itemImageContentType, PreconfiguredItems[2].Id, null) { OwnerId = johnId, FullPath = Path.Combine(pathToFiles, johnId, "Samsung 8.jpg") },
                        new ItemImage ("Pebble.jpg", itemImageContentType, PreconfiguredItems[3].Id, null) { OwnerId = johnId, FullPath= Path.Combine(pathToFiles, johnId, "Pebble.jpg") },

                        new ItemImage ("Shoes.jpg", itemImageContentType, PreconfiguredItems[4].Id, null) { OwnerId = johnId, FullPath= Path.Combine(pathToFiles, jenniferId, "Shoes.jpg") },
                        new ItemImage ("Jacket.jpg", itemImageContentType, PreconfiguredItems[5].Id, null) { OwnerId = johnId, FullPath= Path.Combine(pathToFiles, jenniferId, "Jacket.jpg") }
                    };
                }
                return _PreconfiguredItemImageDetail;
            }
        }
        private static List<ItemVariant> _PreconfiguredItemVariants;
        private static List<ItemVariant> PreconfiguredItemVariants
        {
            get
            {
                if (_PreconfiguredItemVariants == null)
                {
                    _PreconfiguredItemVariants = new List<ItemVariant>
                    {
                        new ItemVariant { Title = "iPhone 6 32GB", Price = 700, ItemId = PreconfiguredItems[0].Id, OwnerId = johnId }, //0
                        new ItemVariant { Title = "iPhone 6 64GB", Price = 800, ItemId = PreconfiguredItems[0].Id, OwnerId = johnId }, //1
                        new ItemVariant { Title = "Samsung 7 32GB HD", Price = 500, ItemId = PreconfiguredItems[1].Id, OwnerId = johnId }, //2
                        new ItemVariant { Title = "Samsung 7 32GB Full HD", Price = 550, ItemId = PreconfiguredItems[1].Id, OwnerId = johnId }, //3
                        new ItemVariant { Title = "Samsung 8 32GB HD", Price = 700, ItemId = PreconfiguredItems[2].Id, OwnerId = johnId },  //4
                        new ItemVariant { Title = "Samsung 8 32GB Full HD", Price = 750, ItemId = PreconfiguredItems[2].Id, OwnerId = johnId }, //5
                        new ItemVariant { Title = "Pebble 1000mAh", Price = 400, ItemId = PreconfiguredItems[3].Id, OwnerId = johnId  }, //6

                        new ItemVariant { Title = "Shoes X Much fashion", Price = 700, ItemId = PreconfiguredItems[4].Id, OwnerId = jenniferId  }, //7
                        new ItemVariant { Title = "Shoes XXL Much fashion", Price = 700, ItemId = PreconfiguredItems[4].Id, OwnerId = jenniferId  }, //8
                        new ItemVariant { Title = "Jacket black", Price = 450, ItemId = PreconfiguredItems[5].Id, OwnerId = jenniferId  }, //9
                        new ItemVariant { Title = "Jacket white", Price = 500, ItemId = PreconfiguredItems[5].Id, OwnerId = jenniferId  },  //10
                    };
                }
                return _PreconfiguredItemVariants;
            }
        }
        private static List<ItemProperty> _PreconfiguredItemProperties;
        private static List<ItemProperty> PreconfiguredItemProperties
        {
            get
            {
                if (_PreconfiguredItemProperties == null)
                {
                    _PreconfiguredItemProperties = new List<ItemProperty>
                    {
                        // iphone 32
                        new ItemProperty (
                            PreconfiguredItemVariants[0].Id,
                            PreconfiguredCharacteristicValues[1].Id
                        ) { OwnerId = johnId },
                        new ItemProperty (
                            PreconfiguredItemVariants[0].Id,
                            PreconfiguredCharacteristicValues[3].Id
                        ) { OwnerId = johnId },
                        // iphone 64
                        new ItemProperty (
                            PreconfiguredItemVariants[1].Id,
                            PreconfiguredCharacteristicValues[2].Id
                        ) { OwnerId = johnId },
                        new ItemProperty (
                            PreconfiguredItemVariants[1].Id,
                            PreconfiguredCharacteristicValues[4].Id
                        ) { OwnerId = johnId },
                        //samsung 7 32 hd
                        new ItemProperty (
                            PreconfiguredItemVariants[2].Id,
                            PreconfiguredCharacteristicValues[1].Id
                        ) { OwnerId = johnId },
                        new ItemProperty (
                            PreconfiguredItemVariants[2].Id,
                            PreconfiguredCharacteristicValues[3].Id
                        ) { OwnerId = johnId },
                        //samsung 7 32 full hd
                        new ItemProperty (
                            PreconfiguredItemVariants[3].Id,
                            PreconfiguredCharacteristicValues[1].Id
                        ) { OwnerId = johnId },
                        new ItemProperty (
                            PreconfiguredItemVariants[3].Id,
                            PreconfiguredCharacteristicValues[4].Id
                        ) { OwnerId = johnId },
                        //samsung 8 64 hd
                        new ItemProperty (
                            PreconfiguredItemVariants[4].Id,
                            PreconfiguredCharacteristicValues[2].Id
                        ) { OwnerId = johnId },
                        new ItemProperty (
                            PreconfiguredItemVariants[4].Id,
                            PreconfiguredCharacteristicValues[3].Id
                        ) { OwnerId = johnId },
                        //samsung 8 64 full hd
                        new ItemProperty (
                            PreconfiguredItemVariants[5].Id,
                            PreconfiguredCharacteristicValues[2].Id
                        ) { OwnerId = johnId },
                        new ItemProperty (
                            PreconfiguredItemVariants[5].Id,
                            PreconfiguredCharacteristicValues[4].Id
                        ) { OwnerId = johnId },
                        //pebble 100mAh
                        new ItemProperty (
                            PreconfiguredItemVariants[6].Id,
                            PreconfiguredCharacteristicValues[5].Id
                        ) { OwnerId = johnId },

                        //shoes X much fashion
                        new ItemProperty (
                            PreconfiguredItemVariants[7].Id,
                            PreconfiguredCharacteristicValues[7].Id
                        ) { OwnerId = jenniferId },
                        new ItemProperty (
                            PreconfiguredItemVariants[7].Id,
                            PreconfiguredCharacteristicValues[10].Id
                        ) { OwnerId = jenniferId },
                        //shoes XXL much fashion
                        new ItemProperty (
                            PreconfiguredItemVariants[8].Id,
                            PreconfiguredCharacteristicValues[9].Id
                        ) { OwnerId = jenniferId },
                        new ItemProperty (
                            PreconfiguredItemVariants[8].Id,
                            PreconfiguredCharacteristicValues[10].Id
                        ) { OwnerId = jenniferId },
                        //jacket black
                        new ItemProperty (
                            PreconfiguredItemVariants[9].Id,
                            PreconfiguredCharacteristicValues[12].Id
                        ) { OwnerId = jenniferId },
                        //jacket white
                        new ItemProperty (
                            PreconfiguredItemVariants[9].Id,
                            PreconfiguredCharacteristicValues[13].Id
                        ) { OwnerId = jenniferId },
                    };
                }
                return _PreconfiguredItemProperties;
            }
        }
        private static List<CartItem> _PreconfiguredCartItems;
        private static List<CartItem> PreconfiguredCartItems
        {
            get
            {
                if (_PreconfiguredCartItems == null)
                {
                    _PreconfiguredCartItems = new List<CartItem>
                    {
                        new CartItem() { OwnerId = johnId, ItemVariantId = PreconfiguredItemVariants[0].Id, Number = 1 },
                        new CartItem() { OwnerId = johnId, ItemVariantId = PreconfiguredItemVariants[1].Id, Number = 2 },
                        new CartItem() { OwnerId = jenniferId, ItemVariantId = PreconfiguredItemVariants[0].Id, Number = 3 },
                        new CartItem() { OwnerId = jenniferId, ItemVariantId = PreconfiguredItemVariants[1].Id, Number = 4 },
                    };
                }
                return _PreconfiguredCartItems;
            }
        }
        public static void EnsureFilesAreInPlace()
        {
            foreach (var itemImage in PreconfiguredItemImageDetail)
            {
                if (!File.Exists(itemImage.FullPath))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(itemImage.FullPath));
                    string rel = Path.GetRelativePath(pathToFiles, itemImage.FullPath);
                    string fullBackupPath = Path.Combine(pathToBackup, rel);
                    File.Copy(fullBackupPath, itemImage.FullPath);
                }
            }
        }
    }
}
