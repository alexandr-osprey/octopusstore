namespace ApplicationCore.Entities
{
    /// <summary>
    /// Property Value of and Item Variant. For example 32GB of Storage in iPhone variant.
    /// </summary>
    public class ItemProperty: Entity
    {
        public int ItemVariantId { get; set; }
        public int CharacteristicValueId { get; set; }
        public CharacteristicValue CharacteristicValue { get; set; }
        public ItemVariant ItemVariant { get; set; }

        public ItemProperty()
        {
        }
        public ItemProperty(int itemVariantId, int categoryPropertyValueId)
        {
            ItemVariantId = itemVariantId;
            CharacteristicValueId = categoryPropertyValueId;
        }
    }
}
