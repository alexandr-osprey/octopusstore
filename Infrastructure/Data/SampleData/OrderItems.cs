using ApplicationCore.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace Infrastructure.Data.SampleData
{
    public class OrderItems: SampleDataEntities<OrderItem>
    {
        protected Orders Orders { get; }
        protected ItemVariants ItemVariants { get; }

        public OrderItem JenInJohnsStore1 { get; set; }
        public OrderItem JenInJohnsStore2 { get; set; }
        public OrderItem JohnInJensStore1 { get; set; }
        public OrderItem JohnInJensStore2 { get; set; }
        public OrderItem JenInJensStore1 { get; set; }
        public OrderItem JenInJensStore2 { get; set; }
        public OrderItem JohnInJohnsStore1 { get; set; }
        public OrderItem JohnInJohnsStore2 { get; set; }
        public OrderItem John10001 { get; set; }
        public OrderItem John10002 { get; set; }
        public OrderItem Jen20001 { get; set; }
        public OrderItem Jen20002 { get; set; }

        public OrderItems(StoreContext storeContext, Orders orders, ItemVariants itemVariants) : base(storeContext)
        {
            Orders = orders;
            ItemVariants = itemVariants;
            Seed();
            Init();
        }

        protected override IEnumerable<OrderItem> GetSourceEntities()
        {
            return new List<OrderItem>
            {
                new OrderItem() { ItemVariantId = ItemVariants.IPhone632GB.Id, Number = 1, OrderId = Orders.JenInJohnsStore.Id, OwnerId = Users.JenniferId },
                new OrderItem() { ItemVariantId = ItemVariants.Samsung716GBHD.Id, Number = 1, OrderId = Orders.JenInJohnsStore.Id, OwnerId = Users.JenniferId },

                new OrderItem() { ItemVariantId = ItemVariants.IPhone632GB.Id, Number = 2, OrderId = Orders.JohnInJensStore.Id, OwnerId = Users.JohnId },
                new OrderItem() { ItemVariantId = ItemVariants.Samsung716GBHD.Id, Number = 2, OrderId = Orders.JohnInJensStore.Id, OwnerId = Users.JohnId },

                new OrderItem() { ItemVariantId = ItemVariants.IPhone632GB.Id, Number = 3, OrderId = Orders.JenInJensStore.Id, OwnerId = Users.JenniferId },
                new OrderItem() { ItemVariantId = ItemVariants.Samsung716GBHD.Id, Number = 3, OrderId = Orders.JenInJensStore.Id, OwnerId = Users.JenniferId },

                new OrderItem() { ItemVariantId = ItemVariants.IPhone632GB.Id, Number = 4, OrderId = Orders.JohnInJohnsStore.Id, OwnerId = Users.JohnId },
                new OrderItem() { ItemVariantId = ItemVariants.Samsung716GBHD.Id, Number = 4, OrderId = Orders.JohnInJohnsStore.Id, OwnerId = Users.JohnId },

                new OrderItem() { ItemVariantId = ItemVariants.IPhone664GB.Id, Number = 5, OrderId = Orders.John1000.Id, OwnerId = Users.JohnId },
                new OrderItem() { ItemVariantId = ItemVariants.Samsung832GBHD.Id, Number = 5, OrderId = Orders.John1000.Id, OwnerId = Users.JohnId },

                new OrderItem() { ItemVariantId = ItemVariants.IPhone664GB.Id, Number = 6, OrderId = Orders.Jen2000.Id, OwnerId = Users.JenniferId },
                new OrderItem() { ItemVariantId = ItemVariants.Samsung832GBHD.Id, Number = 6, OrderId = Orders.Jen2000.Id, OwnerId = Users.JenniferId },
            };
        }

        protected override IQueryable<OrderItem> GetQueryable()
        {
            return base.GetQueryable()
                .Include(o => o.ItemVariant)
                .Include(o => o.Order);
        }

        public override void Init()
        {
            base.Init();
            JenInJohnsStore1 = Entities[0];
            JenInJohnsStore2 = Entities[1];
            JohnInJensStore1 = Entities[2];
            JohnInJensStore2 = Entities[3];
            JenInJensStore1 = Entities[4];
            JenInJensStore2 = Entities[5];
            JohnInJohnsStore1 = Entities[6];
            JohnInJohnsStore2 = Entities[7];
            John10001 = Entities[8];
            John10002 = Entities[9];
            Jen20001 = Entities[10];
            Jen20002 = Entities[11];
        }
    }
}
