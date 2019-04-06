namespace ApplicationCore.Specifications
{
    public class CartItemThumbnailSpecification: CartItemSpecification
    {
        public CartItemThumbnailSpecification(string ownerId): base(ownerId)
        {
            AddInclude();
        }

        public CartItemThumbnailSpecification(string ownerId, int itemVariantId)
           : base(ownerId, itemVariantId)
        {
            AddInclude();
        }

        public CartItemThumbnailSpecification(int id): base(id)
        {
            AddInclude();
        }

        protected void AddInclude()
        {
            AddInclude(c => c.ItemVariant.Item.Images);
            AddInclude(c => c.ItemVariant.Item.Store);
            AddInclude(c => c.ItemVariant.Item.MeasurementUnit);
        }
    }
}
