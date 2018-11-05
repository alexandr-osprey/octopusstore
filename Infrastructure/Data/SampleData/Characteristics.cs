using ApplicationCore.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace Infrastructure.Data.SampleData
{
    public class Characteristics: SampleDataEntities<Characteristic>
    {
        protected Categories Categories { get; }

        public Characteristic Storage { get; protected set; }
        public Characteristic Resolution { get; protected set; }
        public Characteristic Battery { get; protected set; }
        public Characteristic Size { get; protected set; }
        public Characteristic Fashion { get; protected set; }
        public Characteristic Colour { get; protected set; }
        public Characteristic Univercal { get; protected set; }

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
                new Characteristic { Title = "Storage", CategoryId = Categories.Electronics.Id, OwnerId = Users.AdminId }, //0
                new Characteristic { Title = "Resolution", CategoryId = Categories.Electronics.Id, OwnerId = Users.AdminId  }, //1
                new Characteristic { Title = "Battery", CategoryId = Categories.Smartwatches.Id, OwnerId = Users.AdminId  },//2

                new Characteristic { Title = "Size", CategoryId = Categories.Clothes.Id, OwnerId = Users.AdminId }, //3
                new Characteristic { Title = "Fashion", CategoryId = Categories.Clothes.Id, OwnerId = Users.AdminId }, //4
                new Characteristic { Title = "Colour", CategoryId = Categories.Jackets.Id, OwnerId = Users.AdminId }, //5
                new Characteristic { Title = "Universal", CategoryId = Categories.Root.Id, OwnerId = Users.AdminId }, //5
            };
        }

        protected override IQueryable<Characteristic> GetQueryable()
        {
            return base.GetQueryable().Include(c => c.Category);
        }

        public override void Init()
        {
            base.Init();
            Storage = Entities[0];
            Resolution = Entities[1];
            Battery = Entities[2];
            Size = Entities[3];
            Fashion = Entities[4];
            Colour = Entities[5];
            Univercal = Entities[6];
        }
    }
}
