using ApplicationCore.Entities;

namespace ApplicationCore.Specifications
{
    public class CartItemSpecification: EntitySpecification<CartItem>
    {
        public CartItemSpecification(string ownerId): base(i => i.OwnerId == ownerId)
        {
            Description += $"{nameof(ownerId)}: {ownerId}";
        }
        public CartItemSpecification(string ownerId, int itemVariantId)
           : base(i => i.OwnerId == ownerId && i.ItemVariantId == itemVariantId)
        {
            Description += $"{nameof(ownerId)}: {ownerId}, {nameof(itemVariantId)}: {itemVariantId}";
        }
        public CartItemSpecification(int id) : base(id)
        {
        }
    }
}
