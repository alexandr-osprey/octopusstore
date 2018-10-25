namespace ApplicationCore.Entities
{
    public class CartItem: Entity
    {
        public int ItemVariantId { get; set; }
        public int Number { get; set; }
        public ItemVariant ItemVariant { get; set; }
    }
}
