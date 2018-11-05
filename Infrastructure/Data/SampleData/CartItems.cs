using ApplicationCore.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace Infrastructure.Data.SampleData
{
    public class CartItems: SampleDataEntities<CartItem>
    {
        protected ItemVariants ItemVariants { get; }

        public CartItem JohnIphone32 { get; protected set; }
        public CartItem JohnIphone64 { get; protected set; }
        public CartItem JenniferIphone32 { get; protected set; }
        public CartItem JenniferIphone64 { get; protected set; }

        public CartItems(StoreContext storeContext, ItemVariants itemVariants): base(storeContext)
        {
            ItemVariants = itemVariants;
            Seed();
            Init();
        }

        protected override IEnumerable<CartItem> GetSourceEntities()
        {
            return new List<CartItem>
            {
                new CartItem() { OwnerId = Users.JohnId, ItemVariantId = ItemVariants.IPhone632GB.Id, Number = 1 },
                new CartItem() { OwnerId = Users.JohnId, ItemVariantId = ItemVariants.IPhone664GB.Id, Number = 2 },
                new CartItem() { OwnerId = Users.JenniferId, ItemVariantId = ItemVariants.IPhone632GB.Id, Number = 3 },
                new CartItem() { OwnerId = Users.JenniferId, ItemVariantId = ItemVariants.IPhone664GB.Id, Number = 4 },
            };
        }

        protected override IQueryable<CartItem> GetQueryable()
        {
            return base.GetQueryable().Include(i => i.ItemVariant);
        }

        public override void Init()
        {
            base.Init();
            JohnIphone32 = Entities[0];
            JohnIphone64 = Entities[1];
            JenniferIphone32 = Entities[2];
            JenniferIphone64 = Entities[3];
        }
    }
}
