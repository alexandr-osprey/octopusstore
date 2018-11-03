using ApplicationCore.Entities;
using System.Collections.Generic;

namespace Infrastructure.Data.SampleData
{
    public class Stores: SampleDataEntities<Store>
    {
        public Store Johns { get; }
        public Store Jennifers { get; }

        public Stores(StoreContext storeContext): base(storeContext)
        {
            Seed();

            Johns = Entities[0];
            Jennifers = Entities[0];
        }

        protected override IEnumerable<Store> GetSourceEntities()
        {
            return new List<Store>
            {
                new Store { Title = "John's store", Address = "NY", Description = "Electronics best deals", OwnerId = Users.JohnId },
                new Store { Title = "Jennifer's store", Address = "Sydney", Description = "Fashion", OwnerId = Users.JenniferId }
            };
        }
    }
}
