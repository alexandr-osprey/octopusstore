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

        public Brand Reebok { get; protected set; }
        public Brand UnitedColorsOfBenetton { get; protected set; }
        public Brand MarcOPolo { get; protected set; }
        public Brand DanielePatrici { get; protected set; }

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
                new Brand { Title = "Reebok", OwnerId = Users.AdminId },
                new Brand { Title = "UnitedColorsOfBenetton", OwnerId = Users.AdminId },
                new Brand { Title = "Daniele Patrici", OwnerId = Users.AdminId },
                new Brand { Title = "Marc O' Polo", OwnerId = Users.AdminId },
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
            Reebok = Entities[2];
            UnitedColorsOfBenetton = Entities[3];
            DanielePatrici = Entities[4];
            MarcOPolo = Entities[5];
        }
    }
}
