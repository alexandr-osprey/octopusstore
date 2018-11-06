namespace ApplicationCore.Specifications
{
    public class CartItemThumbnailSpecification: CartItemSpecification
    {
        public CartItemThumbnailSpecification(string ownerId): base(ownerId)
        {
            AddInclude("ItemVariant.Item");
        }
        public CartItemThumbnailSpecification(string ownerId, int itemVariantId)
           : base(ownerId, itemVariantId)
        {
        }
    }
}
