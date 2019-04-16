using ApplicationCore.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace Infrastructure.Data.SampleData
{
    public class Items: SampleDataEntities<Item>
    {
        protected Brands Brands { get; }
        protected Stores Stores { get; }
        protected Categories Categories { get; }
        protected MeasurementUnits MeasurementUnits { get; }

        public Item IPhoneXR  { get; protected set; }
        public Item IPhone8Plus { get; protected set; }
        public Item SamsungS9  { get; protected set; }
        public Item SamsungS10  { get; protected set; }
        public Item AppleWatchSeries4  { get; protected set; }
        public Item SamsungGalaxyWatch { get; protected set; }

        public Item ReebokFastTempo { get; protected set; }
        public Item ReebokDMXRun10 { get; protected set; }

        public Item MarcOPoloShoes1 { get; protected set; }
        public Item MarcOPoloShoes2 { get; protected set; }

        public Item UCBDress1 { get; protected set; }
        public Item UCBDress2 { get; protected set; }

        public Item DanielePatriciBag1 { get; protected set; }
        public Item DanielePatriciBag2 { get; protected set; }

        public Item DanielePatriciClutch1 { get; protected set; }

        public Items(StoreContext storeContext, Brands brands, Stores stores, Categories categories, MeasurementUnits measurementUnits): base(storeContext)
        {
            Brands = brands;
            Stores = stores;
            Categories = categories;
            MeasurementUnits = measurementUnits;
            Seed();
            Init();
        }

        protected override IEnumerable<Item> GetSourceEntities()
        {
            var list = new List<Item>()
            {
                new Item { Title = "Apple iPhone XR", BrandId = Brands.Apple.Id, CategoryId = Categories.Smartphones.Id,
                    MeasurementUnitId = MeasurementUnits.Pcs.Id, StoreId = Stores.Johns.Id, OwnerId = Stores.Johns.OwnerId, Description = "This Apple iPhone XR GSM Unlocked Smartphone 4G LTE iOS Smartphone " +
                        "is fully functional and will work with T-Mobile, AT&T and all other GSM networks" },
                new Item { Title = "Apple iPhone XR", BrandId = Brands.Apple.Id, CategoryId = Categories.Smartphones.Id,
                    MeasurementUnitId = MeasurementUnits.Pcs.Id, StoreId = Stores.Johns.Id, OwnerId = Stores.Johns.OwnerId, Description = "This Apple iPhone 8 Plus GSM Unlocked Smartphone 4G LTE iOS Smartphone " +
                        "is fully functional and will work with T-Mobile, AT&T and all other GSM networks" },
                new Item { Title = "Samsung Galaxy S9", BrandId = Brands.Samsung.Id, CategoryId = Categories.Smartphones.Id,
                    MeasurementUnitId = MeasurementUnits.Pcs.Id,StoreId = Stores.Johns.Id, OwnerId = Stores.Johns.OwnerId, Description = "Factory Unlocked will work with any network" },
                new Item { Title = "Samsung Galaxy S10", BrandId = Brands.Samsung.Id, CategoryId = Categories.Smartphones.Id,
                    MeasurementUnitId = MeasurementUnits.Pcs.Id, StoreId = Stores.Johns.Id, OwnerId = Stores.Johns.OwnerId, Description = "Factory Unlocked will work with any network" },

                new Item { Title = "Apple Watch", BrandId = Brands.Apple.Id, CategoryId = Categories.Smartwatches.Id,
                    MeasurementUnitId = MeasurementUnits.Pcs.Id, StoreId = Stores.Johns.Id, OwnerId = Stores.Johns.OwnerId, Description = "GLONASS, Galileo, and QZSS, Barometric altimeter, Optical heart sensor, Electrical heart sensor, " +
                    "Improved accelerometer up to 32 g‑forces, Improved gyroscope, Ambient light sensor, LTPO OLED Retina display with Force Touch (1000 nits), " +
                    "Digital Crown with haptic feedback, Louder speaker, Ion-X strengthened glass, Sapphire crystal and ceramic back" },
                new Item { Title = "Samsung Galaxy Watch", BrandId = Brands.Samsung.Id, CategoryId = Categories.Smartwatches.Id,
                    MeasurementUnitId = MeasurementUnits.Pcs.Id, StoreId = Stores.Johns.Id, OwnerId = Stores.Johns.OwnerId, Description = "Go nonstop for days on a single charge. The wireless charger " +
                    "lets you power up without slowing down. (Average expected performance based on typical use. Results may vary.) Available in two sizes and three colors, the Galaxy Watch offers stylish watch faces so realistic they hardly look digital. " +
                    "Plus, choose from a collection of interchangeable bands Pairs with both Android and iOS smartphones via Bluetooth connection" },

                new Item { Title = "Reebok Fast Tempo", BrandId = Brands.Reebok.Id, CategoryId = Categories.WomensDresses.Id,
                    MeasurementUnitId = MeasurementUnits.Pcs.Id, StoreId = Stores.Jennifers.Id, OwnerId = Stores.Jennifers.OwnerId, Description = "Whether you're pushing through an interval class or headed out on a short run, these men's shoes provide the ultimate in flexibility while keeping your foot stable. " +
                    "The breathable Flexweave upper has targeted areas of support and stretch for multidirectional movement." +
                    " An internal midfoot strap secures the fit, and lightweight cushioning keeps you comfortable." },
                new Item { Title = "Reebok DMX Run 10", BrandId = Brands.Reebok.Id, CategoryId = Categories.WomensDresses.Id,
                    MeasurementUnitId = MeasurementUnits.Pcs.Id, StoreId = Stores.Jennifers.Id, OwnerId = Stores.Jennifers.OwnerId, Description = "The innovators from the Reebok continue to release contemporary must-haves and classic reissues. " +
                    "The Reebok DMX Run 10 has been revived from the vault, this time as a lifestyle sneaker. This classic shoe displays a futuristic design with durable details." },

                new Item { Title = "Marc O' Polo Shoes", BrandId = Brands.MarcOPolo.Id, CategoryId = Categories.WomensFootwear.Id,
                    MeasurementUnitId = MeasurementUnits.Pcs.Id, StoreId = Stores.Jennifers.Id, OwnerId = Stores.Jennifers.OwnerId, Description = "Brand new leather shoes" },
                new Item { Title = "Marc O' Polo Shoes", BrandId = Brands.MarcOPolo.Id, CategoryId = Categories.WomensFootwear.Id,
                    MeasurementUnitId = MeasurementUnits.Pcs.Id, StoreId = Stores.Jennifers.Id, OwnerId = Stores.Jennifers.OwnerId, Description = "Brand new leather shoes" },

                new Item { Title = "United Colors Of Benetton Dress", BrandId = Brands.UnitedColorsOfBenetton.Id, CategoryId = Categories.WomensDresses.Id,
                    MeasurementUnitId = MeasurementUnits.Pcs.Id, StoreId = Stores.Jennifers.Id, OwnerId = Stores.Jennifers.OwnerId, Description = "Light summer dress" },
                new Item { Title = "United Colors Of Benetton Dress", BrandId = Brands.UnitedColorsOfBenetton.Id, CategoryId = Categories.WomensDresses.Id,
                    MeasurementUnitId = MeasurementUnits.Pcs.Id, StoreId = Stores.Jennifers.Id, OwnerId = Stores.Jennifers.OwnerId, Description = "Light summer dress" },

                new Item { Title = "Danielle Patrici Bag", BrandId = Brands.DanielePatrici.Id, CategoryId = Categories.WomensAccesories.Id,
                    MeasurementUnitId = MeasurementUnits.Pcs.Id, StoreId = Stores.Jennifers.Id, OwnerId = Stores.Jennifers.OwnerId, Description = "Comfortable everyday bag" },
                new Item { Title = "Danielle Patrici Bag", BrandId = Brands.DanielePatrici.Id, CategoryId = Categories.WomensAccesories.Id,
                    MeasurementUnitId = MeasurementUnits.Pcs.Id, StoreId = Stores.Jennifers.Id, OwnerId = Stores.Jennifers.OwnerId, Description = "Comfortable everyday bag" },

                new Item { Title = "Danielle Patrici Clutch", BrandId = Brands.DanielePatrici.Id, CategoryId = Categories.WomensAccesories.Id,
                    MeasurementUnitId = MeasurementUnits.Pcs.Id, StoreId = Stores.Jennifers.Id, OwnerId = Stores.Jennifers.OwnerId, Description = "Comfortable everyday bag" },
            };
            var result = list;
            //    .Concat(list.Select(i => i.ShallowClone()))
            //    .Concat(list.Select(i => i.ShallowClone()))
            //    .Concat(list.Select(i => i.ShallowClone()))
            //    .Concat(list.Select(i => i.ShallowClone()))
            //    .Concat(list.Select(i => i.ShallowClone()))
            //    .Concat(list.Select(i => i.ShallowClone()))
            //    .Concat(list.Select(i => i.ShallowClone()))
            //    .Concat(list.Select(i => i.ShallowClone()))
            //    .Concat(list.Select(i => i.ShallowClone()));
            //result = result.Concat(result.Select(i => i.ShallowClone()));
            //result = result.Concat(result.Select(i => i.ShallowClone()));
            //result = result.Concat(result.Select(i => i.ShallowClone()));
            //result = result.Concat(result.Select(i => i.ShallowClone()));
            //result = result.Concat(result.Select(i => i.ShallowClone()));
            //result = result.Concat(result.Select(i => i.ShallowClone()));
            return result;
        }

        protected override IQueryable<Item> GetQueryable()
        {
            return base.GetQueryable()
                .Include(i => i.Brand)
                .Include(i => i.Category)
                .Include(i => i.ItemVariants)
                //.Include(i => i.Images)
                .Include(i => i.Store)
                .Include(i => i.MeasurementUnit);
        }

        public override void Init()
        {
            base.Init();
            IPhoneXR = Entities[0];
            IPhone8Plus = Entities[1];
            SamsungS9 = Entities[2];
            SamsungS10 = Entities[3];
            AppleWatchSeries4 = Entities[4];
            SamsungGalaxyWatch = Entities[5];
            ReebokFastTempo = Entities[6];
            ReebokDMXRun10 = Entities[7];
            MarcOPoloShoes1 = Entities[8];
            MarcOPoloShoes2 = Entities[9];
            UCBDress1 = Entities[10];
            UCBDress2 = Entities[11];
            DanielePatriciBag1 = Entities[12];
            DanielePatriciBag2 = Entities[13];
            DanielePatriciClutch1 = Entities[14];
        }
    }
}
