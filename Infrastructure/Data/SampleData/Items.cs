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

        public Item IPhone6  { get; protected set; }
        public Item Samsung7  { get; protected set; }
        public Item Samsung8  { get; protected set; }
        public Item PebbleWatch  { get; protected set; }
        public Item Shoes  { get; protected set; }
        public Item Jacket  { get; protected set; }

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
                new Item { Title = "iPhone 6", BrandId = Brands.Apple.Id, CategoryId = Categories.Smartphones.Id,
                    MeasurementUnitId = MeasurementUnits.Pcs.Id, StoreId = Stores.Johns.Id, OwnerId = Stores.Johns.OwnerId, Description = "desc" },
                new Item { Title = "Samsung 7", BrandId = Brands.Samsung.Id, CategoryId = Categories.Smartphones.Id,
                    MeasurementUnitId = MeasurementUnits.Pcs.Id,StoreId = Stores.Johns.Id, OwnerId = Stores.Johns.OwnerId, Description = "desc" },
                new Item { Title = "Samsung 8", BrandId = Brands.Samsung.Id, CategoryId = Categories.Smartphones.Id,
                    MeasurementUnitId = MeasurementUnits.Pcs.Id, StoreId = Stores.Johns.Id, OwnerId = Stores.Johns.OwnerId, Description = "desc" },
                new Item { Title = "Pebble Watch", BrandId = Brands.Pebble.Id, CategoryId = Categories.Smartwatches.Id,
                    MeasurementUnitId = MeasurementUnits.Pcs.Id, StoreId = Stores.Johns.Id, OwnerId = Stores.Johns.OwnerId, Description = "desc" },

                new Item { Title = "Shoes", BrandId = Brands.CK.Id, CategoryId = Categories.Shoes.Id,
                    MeasurementUnitId = MeasurementUnits.Pcs.Id, StoreId = Stores.Jennifers.Id, OwnerId = Stores.Jennifers.OwnerId, Description = "desc" },
                new Item { Title = "Jacket", BrandId = Brands.Armani.Id, CategoryId = Categories.Jackets.Id,
                    MeasurementUnitId = MeasurementUnits.Pcs.Id, StoreId = Stores.Jennifers.Id, OwnerId = Stores.Jennifers.OwnerId, Description = "desc" },
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
                .Include(i => i.Images)
                .Include(i => i.Store)
                .Include(i => i.MeasurementUnit);
        }

        public override void Init()
        {
            base.Init();
            IPhone6 = Entities[0];
            Samsung7 = Entities[1];
            Samsung8 = Entities[2];
            PebbleWatch = Entities[3];
            Shoes = Entities[4];
            Jacket = Entities[5];
        }
    }
}
