using ApplicationCore.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace Infrastructure.Data.SampleData
{
    public class CartItems: SampleDataEntities<CartItem>
    {
        protected ItemVariants ItemVariants { get; }

        public CartItem JohnIPhoneXR64FromJohns { get; protected set; }
        public CartItem JenniferIPhoneXR64FromJohns { get; protected set; }
        public CartItem JohnDress1FromJennifers { get; protected set; }
        public CartItem JohnShoesDMXFromJennifers { get; protected set; }
        public CartItem User1IPhoneXR64FromJohns { get; protected set; }
        public CartItem User1Dress1FromJennifers { get; protected set; }

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
                new CartItem() { OwnerId = Users.JohnId, ItemVariantId = ItemVariants.IPhoneXR64GBWhite.Id, Number = 1 },
                new CartItem() { OwnerId = Users.JenniferId, ItemVariantId = ItemVariants.IPhoneXR64GBWhite.Id, Number = 1 },
                new CartItem() { OwnerId = Users.JohnId, ItemVariantId = ItemVariants.UCBDress1WhiteS.Id, Number = 1 },
                new CartItem() { OwnerId = Users.JohnId, ItemVariantId = ItemVariants.ReebokDMXRun10White35.Id, Number = 1 },
                new CartItem() { OwnerId = Users.User1Id, ItemVariantId = ItemVariants.IPhoneXR64GBWhite.Id, Number = 1 },
                new CartItem() { OwnerId = Users.User1Id, ItemVariantId = ItemVariants.UCBDress1WhiteS.Id, Number = 1 },
            };
        }

        protected override IQueryable<CartItem> GetQueryable()
        {
            return base.GetQueryable().Include(i => i.ItemVariant);
        }

        public override void Init()
        {
            base.Init();
            JohnIPhoneXR64FromJohns = Entities[0];
            JenniferIPhoneXR64FromJohns = Entities[1];
            JohnDress1FromJennifers = Entities[2];
            JohnShoesDMXFromJennifers = Entities[3];
            User1IPhoneXR64FromJohns = Entities[4];
            User1Dress1FromJennifers = Entities[5];
        }
    }
}
