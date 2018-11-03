using ApplicationCore.Interfaces;

namespace ApplicationCore.Entities
{
    public class CartItem: Entity, IGenericMemberwiseClonable<CartItem>
    {
        public int ItemVariantId { get; set; }
        public int Number { get; set; }
        public ItemVariant ItemVariant { get; set; }

        public CartItem(): base()
        {
        }
        protected CartItem(CartItem cartItem): base(cartItem)
        {
            ItemVariantId = cartItem.ItemVariantId;
            Number = cartItem.Number;
        }

        public CartItem ShallowClone()
        {
            return (CartItem)MemberwiseClone();
        }
    }
}
