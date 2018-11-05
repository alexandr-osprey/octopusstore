using ApplicationCore.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace Infrastructure.Data.SampleData
{
    public class Brands: SampleDataEntities<Brand>
    {
        public Brand Apple { get; protected set; }
        public Brand Samsung { get; protected set; }
        public Brand Pebble { get; protected set; }
        public Brand CK { get; protected set; }
        public Brand Armani { get; protected set; }

        public Brands(StoreContext storeContext): base(storeContext)
        {
            Seed();
            Init();
            //entities = GetQueryable().ToList();
            
        }

        protected override IEnumerable<Brand> GetSourceEntities()
        {
            return new List<Brand>
            {
                new Brand { Title = "Apple", OwnerId = Users.AdminId },
                new Brand { Title = "Samsung", OwnerId = Users.AdminId },
                new Brand { Title = "Pebble", OwnerId = Users.AdminId},
                new Brand { Title = "CK", OwnerId = Users.AdminId },
                new Brand { Title = "Armani", OwnerId = Users.AdminId }
            };
        }

        protected override IQueryable<Brand> GetQueryable()
        {
            return base.GetQueryable().Include(b => b.Items);
        }

        public override void Init()
        {
            base.Init();
            Apple = Entities[0];
            Samsung = Entities[1];
            Pebble = Entities[2];
            CK = Entities[3];
            Armani = Entities[4];
        }
    }
}
