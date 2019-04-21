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
        protected ItemVariants ItemVariants { get; }

        public Order JenInJohnsStore { get; set; }
        public Order JohnInJensStore { get; set; }
        public Order JenInJensStore { get; set; }
        public Order JohnInJohnsStore { get; set; }
        public Order JenInJohnsStoreCancelled { get; set; }
        public Order JenInJohnsStoreFinished { get; set; }

        public Orders(StoreContext storeContext, ItemVariants itemVariants): base(storeContext)
        {
            ItemVariants = itemVariants;
            Seed();
            Init();
        }

        protected override IEnumerable<Order> GetSourceEntities()
        {
            return new List<Order>
            {
                new Order() { ItemVariantId = ItemVariants.IPhoneXR64GBRed.Id, OwnerId = Users.JenniferId, Number = 1, Sum = ItemVariants.IPhoneXR64GBRed.Price, Status = OrderStatus.Created },
                new Order() { ItemVariantId = ItemVariants.ReebokFastTempoWhite42.Id, OwnerId = Users.JohnId, Number = 2, Sum = ItemVariants.ReebokFastTempoWhite42.Price * 2, Status = OrderStatus.Created },
                new Order() { ItemVariantId = ItemVariants.MarcOPoloShoes1Black35.Id, OwnerId = Users.JenniferId, Number = 3, Sum = ItemVariants.MarcOPoloShoes1Black35.Price * 3, Status = OrderStatus.Created },
                new Order() { ItemVariantId = ItemVariants.SamsungGalaxyS10128GBBlack.Id, OwnerId = Users.JohnId, Number = 4, Sum = ItemVariants.SamsungGalaxyS10128GBBlack.Price * 4, Status = OrderStatus.Created },
                new Order() { ItemVariantId = ItemVariants.IPhone8Plus64GBWhite.Id, OwnerId = Users.JenniferId, Number = 5, Sum = ItemVariants.IPhone8Plus64GBWhite.Price * 5, Status = OrderStatus.Cancelled, DateTimeCancelled = DateTime.UtcNow },
                new Order() { ItemVariantId = ItemVariants.IPhone8Plus128GBBlack.Id, OwnerId = Users.JenniferId, Number = 6, Sum = ItemVariants.IPhone8Plus128GBBlack.Price * 6, Status = OrderStatus.Finished, DateTimeCancelled = DateTime.UtcNow },
            };
        }

        protected override IQueryable<Order> GetQueryable()
        {
            return base.GetQueryable()
                .Include(o => o.ItemVariant);
        }


        public override void Init()
        {
            base.Init();
            JenInJohnsStore = Entities[0];
            JohnInJensStore = Entities[1];
            JenInJensStore = Entities[2];
            JohnInJohnsStore = Entities[3];
            JenInJohnsStoreCancelled = Entities[4];
            JenInJohnsStoreFinished = Entities[5];
        }
    }
}
