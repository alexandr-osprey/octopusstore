using ApplicationCore.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace Infrastructure.Data.SampleData
{
    public class Stores: SampleDataEntities<Store>
    {
        public Store Johns  { get; protected set; }
        public Store Jennifers  { get; protected set; }

        public Stores(StoreContext storeContext): base(storeContext)
        {
            Seed();
            Init();
        }

        protected override IEnumerable<Store> GetSourceEntities()
        {
            return new List<Store>
            {
                new Store { Title = "John's store", Address = "NY", Description = "Electronics best deals", OwnerId = Users.JohnId },
                new Store { Title = "Jennifer's store", Address = "Sydney", Description = "Fashion", OwnerId = Users.JenniferId }
            };
        }

        protected override IQueryable<Store> GetQueryable()
        {
            return base.GetQueryable().Include(s => s.Items);
        }

        public override void Init()
        {
            base.Init();
            Johns = Entities[0];
            Jennifers = Entities[1];
        }
    }
}
