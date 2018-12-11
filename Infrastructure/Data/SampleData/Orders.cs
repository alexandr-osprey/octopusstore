using ApplicationCore.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Infrastructure.Data.SampleData
{
    public class Orders: SampleDataEntities<Order>
    {
        protected Stores Stores { get; }

        public Order JenInJohnsStore { get; set; }
        public Order JohnInJensStore { get; set; }
        public Order JenInJensStore { get; set; }
        public Order JohnInJohnsStore { get; set; }
        public Order John1000 { get; set; }
        public Order Jen2000 { get; set; }
        public Order John1000Cancelled { get; set; }
        public Order John1000Finished { get; set; }

        public Orders(StoreContext storeContext, Stores stores): base(storeContext)
        {
            Stores = stores;
            Seed();
            Init();
        }

        protected override IEnumerable<Order> GetSourceEntities()
        {
            return new List<Order>
            {
                new Order() { StoreId = Stores.Johns.Id, OwnerId = Users.JenniferId, Sum = 500 },
                new Order() { StoreId = Stores.Jennifers.Id, OwnerId = Users.JohnId, Sum = 500 },
                new Order() { StoreId = Stores.Jennifers.Id, OwnerId = Users.JenniferId, Sum = 500 },
                new Order() { StoreId = Stores.Johns.Id, OwnerId = Users.JohnId, Sum = 500 },
                new Order() { StoreId = Stores.Johns.Id, OwnerId = Users.JenniferId, Sum = 1000 },
                new Order() { StoreId = Stores.Jennifers.Id, OwnerId = Users.JohnId, Sum = 2000 },
                new Order() { StoreId = Stores.Johns.Id, OwnerId = Users.JenniferId, Sum = 1000, Status = OrderStatus.Cancelled, DateTimeCancelled = DateTime.UtcNow },
                new Order() { StoreId = Stores.Johns.Id, OwnerId = Users.JenniferId, Sum = 1000, Status = OrderStatus.Finished, DateTimeCancelled = DateTime.UtcNow },
            };
        }

        protected override IQueryable<Order> GetQueryable()
        {
            return base.GetQueryable()
                .Include(o => o.Store)
                .Include(o => o.OrderItems);
        }

        public override void Init()
        {
            base.Init();
            JenInJohnsStore = Entities[0];
            JohnInJensStore = Entities[1];
            JenInJensStore = Entities[2];
            JohnInJohnsStore = Entities[3];
            John1000 = Entities[4];
            Jen2000 = Entities[5];
            John1000Cancelled = Entities[6];
            John1000Finished = Entities[7];
        }
    }
}
