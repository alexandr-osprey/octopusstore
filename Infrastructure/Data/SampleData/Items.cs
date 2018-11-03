using ApplicationCore.Entities;
using System.Collections.Generic;

namespace Infrastructure.Data.SampleData
{
    public class Items: SampleDataEntities<Item>
    {
        public Brands Brands { get; }
        public Stores Stores { get; }
        public Categories Categories { get; }
        public MeasurementUnits MeasurementUnits { get; }

        public Item IPhone6 { get; }
        public Item Samsung7 { get; }
        public Item Samsung8 { get; }
        public Item PebbleWatch { get; }
        public Item Shoes { get; }
        public Item Jacket { get; }

        public Items(StoreContext storeContext, Brands brands, Stores stores, Categories categories, MeasurementUnits measurementUnits): base(storeContext)
        {
            Brands = brands;
            Stores = stores;
            Categories = categories;
            MeasurementUnits = measurementUnits;
            Seed();

            IPhone6 = Entities[0];
            Samsung7 = Entities[1];
            Samsung8 = Entities[2];
            PebbleWatch = Entities[3];
            Shoes = Entities[4];
            Jacket = Entities[5];
        }

        protected override IEnumerable<Item> GetSourceEntities()
        {
            return new List<Item>()
            {
                new Item { Title = "iPhone 6", BrandId = Brands.Apple.Id, CategoryId = Categories.Smartphones.Id,
                    MeasurementUnitId = MeasurementUnits.Pcs.Id, StoreId = Stores.Johns.Id, OwnerId = Stores.Johns.OwnerId },
                new Item { Title = "Samsung 7", BrandId = Brands.Samsung.Id, CategoryId = Categories.Smartphones.Id,
                    MeasurementUnitId = MeasurementUnits.Pcs.Id,StoreId = Stores.Johns.Id, OwnerId = Stores.Johns.OwnerId },
                new Item { Title = "Samsung 8", BrandId = Brands.Samsung.Id, CategoryId = Categories.Smartphones.Id,
                    MeasurementUnitId = MeasurementUnits.Pcs.Id, StoreId = Stores.Johns.Id, OwnerId = Stores.Johns.OwnerId },
                new Item { Title = "Pebble Watch", BrandId = Brands.Pebble.Id, CategoryId = Categories.Smartwatches.Id,
                    MeasurementUnitId = MeasurementUnits.Pcs.Id, StoreId = Stores.Johns.Id, OwnerId = Stores.Johns.OwnerId },

                new Item { Title = "Shoes", BrandId = Brands.CK.Id, CategoryId = Categories.Shoes.Id,
                    MeasurementUnitId = MeasurementUnits.Pcs.Id, StoreId = Stores.Jennifers.Id, OwnerId = Stores.Jennifers.OwnerId },
                new Item { Title = "Jacket", BrandId = Brands.Armani.Id, CategoryId = Categories.Jackets.Id,
                    MeasurementUnitId = MeasurementUnits.Pcs.Id, StoreId = Stores.Jennifers.Id, OwnerId = Stores.Jennifers.OwnerId },
            };
        }
    }
}
