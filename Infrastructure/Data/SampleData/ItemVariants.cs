using ApplicationCore.Entities;
using System.Collections.Generic;

namespace Infrastructure.Data.SampleData
{
    public class ItemVariants: SampleDataEntities<ItemVariant>
    {
        protected Items Items { get; }

        public ItemVariant IPhone632GB { get; }
        public ItemVariant IPhone664GB { get; }
        public ItemVariant Samsung732GBHD { get; }
        public ItemVariant Samsung732GBFullHD { get; }
        public ItemVariant Samsung832GBHD { get; }
        public ItemVariant Samsung832GBFullHD { get; }
        public ItemVariant Pebble1000mAh { get; }
        public ItemVariant ShoesXMuchFashion { get; }
        public ItemVariant ShoesXXLMuchFashion { get; }
        public ItemVariant JacketBlack { get; }
        public ItemVariant JacketWhite { get; }

        public ItemVariants(StoreContext storeContext, Items items): base(storeContext)
        {
            Items = items;
            Seed();

            IPhone632GB = Entities[0];
            IPhone664GB = Entities[1];
            Samsung732GBHD = Entities[2];
            Samsung732GBFullHD = Entities[3];
            Samsung832GBHD = Entities[4];
            Samsung832GBFullHD = Entities[5];
            Pebble1000mAh = Entities[6];
            ShoesXMuchFashion = Entities[7];
            ShoesXXLMuchFashion = Entities[8];
            JacketBlack = Entities[9];
            JacketWhite = Entities[10];
        }

        protected override IEnumerable<ItemVariant> GetSourceEntities()
        {
            return new List<ItemVariant>
            {
                new ItemVariant { Title = "iPhone 6 32GB", Price = 700, ItemId = Items.IPhone6.Id, OwnerId = Users.JohnId }, //0
                new ItemVariant { Title = "iPhone 6 64GB", Price = 800, ItemId = Items.IPhone6.Id, OwnerId = Users.JohnId }, //1
                new ItemVariant { Title = "Samsung 7 32GB HD", Price = 500, ItemId = Items.Samsung7.Id, OwnerId = Users.JohnId }, //2
                new ItemVariant { Title = "Samsung 7 32GB Full HD", Price = 550, ItemId = Items.Samsung7.Id, OwnerId = Users.JohnId }, //3
                new ItemVariant { Title = "Samsung 8 32GB HD", Price = 700, ItemId = Items.Samsung8.Id, OwnerId = Users.JohnId },  //4
                new ItemVariant { Title = "Samsung 8 32GB Full HD", Price = 750, ItemId = Items.Samsung8.Id, OwnerId = Users.JohnId }, //5
                new ItemVariant { Title = "Pebble 1000mAh", Price = 400, ItemId = Items.PebbleWatch.Id, OwnerId = Users.JohnId  }, //6

                new ItemVariant { Title = "Shoes X Much fashion", Price = 700, ItemId = Items.Shoes.Id, OwnerId = Users.JenniferId  }, //7
                new ItemVariant { Title = "Shoes XXL Much fashion", Price = 700, ItemId = Items.Shoes.Id, OwnerId = Users.JenniferId  }, //8
                new ItemVariant { Title = "Jacket black", Price = 450, ItemId = Items.Jacket.Id, OwnerId = Users.JenniferId  }, //9
                new ItemVariant { Title = "Jacket white", Price = 500, ItemId = Items.Jacket.Id, OwnerId = Users.JenniferId  },  //10
            };
        }
    }
}
