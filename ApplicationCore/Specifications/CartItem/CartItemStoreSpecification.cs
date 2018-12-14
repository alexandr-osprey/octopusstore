using ApplicationCore.Entities;

namespace ApplicationCore.Specifications
{
    public class CartItemStoreSpecification : EntitySpecification<CartItem>
    {
        public CartItemStoreSpecification(string ownerId, int storeId)
           : base(i => i.OwnerId == ownerId && i.ItemVariant.Item.StoreId == storeId)
        {
            Description += $"{nameof(ownerId)}: {ownerId}, {nameof(storeId)}: {storeId}";
            AddInclude(i => i.ItemVariant);
        }
    }
}
