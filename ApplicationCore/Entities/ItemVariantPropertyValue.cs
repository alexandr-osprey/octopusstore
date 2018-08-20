namespace ApplicationCore.Entities
{
    public class ItemVariantCharacteristicValue : Entity
    {
        public int ItemVariantId { get; set; }
        public int CharacteristicValueId { get; set; }

        public CharacteristicValue CharacteristicValue { get; set; }
        public ItemVariant ItemVariant { get; set; }

        public ItemVariantCharacteristicValue()
        { }
        public ItemVariantCharacteristicValue(int itemVariantId, int categoryPropertyValueId)
        {
            this.ItemVariantId = itemVariantId;
            this.CharacteristicValueId = categoryPropertyValueId;
        }
    }
}
