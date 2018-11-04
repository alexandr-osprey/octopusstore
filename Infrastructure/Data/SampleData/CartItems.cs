using ApplicationCore.Entities;
using System.Collections.Generic;

namespace Infrastructure.Data.SampleData
{
    public class CartItems: SampleDataEntities<CartItem>
    {
        protected ItemVariants ItemVariants { get; }

        public CartItem JohnIphone32 { get; }
        public CartItem JohnIphone64 { get; }
        public CartItem JenniferIphone32 { get; }
        public CartItem JenniferIphone64 { get; }

        public CartItems(StoreContext storeContext, ItemVariants itemVariants): base(storeContext)
        {
            ItemVariants = itemVariants;
            Seed();
            JohnIphone32 = Entities[0];
            JohnIphone64 = Entities[1];
            JenniferIphone32 = Entities[2];
            JenniferIphone64 = Entities[3];
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
    }
}
