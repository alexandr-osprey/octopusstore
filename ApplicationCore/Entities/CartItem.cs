using ApplicationCore.Interfaces;

namespace ApplicationCore.Entities
{
    public class CartItem: Entity, IGenericMemberwiseClonable<CartItem>
    {
        public int ItemVariantId { get; set; }
        public int Number { get; set; }

        public virtual ItemVariant ItemVariant { get; set; }

        public CartItem(): base()
        {
        }

        public bool Equals(CartItem other) => base.Equals(other) 
            && ItemVariantId == other.ItemVariantId 
            && Number == other.Number;
        public override bool Equals(object obj) => Equals(obj as CartItem);
        public override int GetHashCode() => base.GetHashCode();

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
