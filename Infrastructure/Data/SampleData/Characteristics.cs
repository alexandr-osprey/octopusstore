using ApplicationCore.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace Infrastructure.Data.SampleData
{
    public class Characteristics: SampleDataEntities<Characteristic>
    {
        protected Categories Categories { get; }

        public Characteristic SmartphoneStorage { get; protected set; }
        public Characteristic SmartphoneResolution { get; protected set; }
        public Characteristic SmartphoneRAM { get; protected set; }
        public Characteristic SmartphoneBattery { get; protected set; }
        public Characteristic SmartphoneColor { get; protected set; }

        public Characteristic SmartwatchColor { get; protected set; }

        public Characteristic WomenFootwearSize { get; protected set; }
        public Characteristic WomenFootwearType { get; protected set; }
        public Characteristic WomenFootwearColor { get; protected set; }

        public Characteristic WomensDressColor { get; protected set; }
        public Characteristic WomensDressSize { get; protected set; }

        public Characteristic WomensAccessoryType { get; protected set; }
        public Characteristic WomensAccessoryColor { get; protected set; }

        public Characteristics(StoreContext storeContext, Categories categories): base(storeContext)
        {
            Categories = categories;
            Seed();
            Init();
        }

        protected override IEnumerable<Characteristic> GetSourceEntities()
        {
            return new List<Characteristic>
            {
                // smartphones
                new Characteristic { Title = "Storage", CategoryId = Categories.Smartphones.Id, OwnerId = Users.AdminId }, //0
                new Characteristic { Title = "Resolution", CategoryId = Categories.Smartphones.Id, OwnerId = Users.AdminId  }, //1
                new Characteristic { Title = "RAM", CategoryId = Categories.Smartphones.Id, OwnerId = Users.AdminId  }, //2
                new Characteristic { Title = "Battery", CategoryId = Categories.Smartphones.Id, OwnerId = Users.AdminId  },//3
                new Characteristic { Title = "Color", CategoryId = Categories.Smartphones.Id, OwnerId = Users.AdminId  },//4

                // smartwatches
                new Characteristic { Title = "Color", CategoryId = Categories.Smartwatches.Id, OwnerId = Users.AdminId  },//5

                // shoes
                new Characteristic { Title = "Size", CategoryId = Categories.WomensFootwear.Id, OwnerId = Users.AdminId }, //6
                new Characteristic { Title = "Type", CategoryId = Categories.WomensFootwear.Id, OwnerId = Users.AdminId }, //7
                new Characteristic { Title = "Colour", CategoryId = Categories.WomensFootwear.Id, OwnerId = Users.AdminId }, //8

                // dress
                new Characteristic { Title = "Color", CategoryId = Categories.WomensDresses.Id, OwnerId = Users.AdminId }, //9
                new Characteristic { Title = "Size", CategoryId = Categories.WomensDresses.Id, OwnerId = Users.AdminId }, //10

                // accessory
                new Characteristic { Title = "Type", CategoryId = Categories.WomensAccesories.Id, OwnerId = Users.AdminId }, //11
                new Characteristic { Title = "Color", CategoryId = Categories.WomensAccesories.Id, OwnerId = Users.AdminId }, //12
            };
        }

        protected override IQueryable<Characteristic> GetQueryable()
        {
            return base.GetQueryable().Include(c => c.Category);
        }

        public override void Init()
        {
            base.Init();
            SmartphoneStorage = Entities[0];
            SmartphoneResolution = Entities[1];
            SmartphoneRAM = Entities[2];
            SmartphoneBattery = Entities[3];
            SmartphoneColor = Entities[4];

            SmartwatchColor = Entities[5];

            WomenFootwearSize = Entities[6];
            WomenFootwearType = Entities[7];
            WomenFootwearColor = Entities[8];

            WomensDressColor = Entities[9];
            WomensDressSize = Entities[10];

            WomensAccessoryType = Entities[11];
            WomensAccessoryColor = Entities[12];
        }
    }
}
