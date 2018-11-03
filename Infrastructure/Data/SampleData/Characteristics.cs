using ApplicationCore.Entities;
using System.Collections.Generic;

namespace Infrastructure.Data.SampleData
{
    public class Characteristics: SampleDataEntities<Characteristic>
    {
        public Categories Categories { get; }

        public Characteristic Storage { get; }
        public Characteristic Resolution { get; }
        public Characteristic Battery { get; }
        public Characteristic Size { get; }
        public Characteristic Fashion { get; }
        public Characteristic Colour { get; }

        public Characteristics(StoreContext storeContext, Categories categories): base(storeContext)
        {
            Categories = categories;
            Seed();
            Storage = Entities[0];
            Resolution = Entities[1];
            Battery = Entities[2];
            Size = Entities[3];
            Fashion = Entities[4];
            Colour = Entities[5];
        }

        protected override IEnumerable<Characteristic> GetSourceEntities()
        {
            return new List<Characteristic>
            {
                new Characteristic { Title = "Storage", CategoryId = Categories.Electronics.Id, OwnerId = Users.AdminId }, //0
                new Characteristic { Title = "Resolution", CategoryId = Categories.Electronics.Id, OwnerId = Users.AdminId  }, //1
                new Characteristic { Title = "Battery", CategoryId = Categories.Smartwatches.Id, OwnerId = Users.AdminId  },//2

                new Characteristic { Title = "Size", CategoryId = Categories.Clothes.Id, OwnerId = Users.AdminId }, //3
                new Characteristic { Title = "Fashion", CategoryId = Categories.Clothes.Id, OwnerId = Users.AdminId }, //4
                new Characteristic { Title = "Colour", CategoryId = Categories.Jackets.Id, OwnerId = Users.AdminId }, //5
            };
        }
    }
}
