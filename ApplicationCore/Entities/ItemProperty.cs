using ApplicationCore.Interfaces;

namespace ApplicationCore.Entities
{
    /// <summary>
    /// Property Value of and Item Variant. For example 32GB of Storage in iPhone variant.
    /// </summary>
    public class ItemProperty: Entity, IGenericMemberwiseClonable<ItemProperty>
    {
        public int ItemVariantId { get; set; }
        public int CharacteristicValueId { get; set; }

        public CharacteristicValue CharacteristicValue { get; set; }
        public ItemVariant ItemVariant { get; set; }

        public ItemProperty()
        {
        }
        public ItemProperty(int itemVariantId, int characteristicValueId)
        {
            ItemVariantId = itemVariantId;
            CharacteristicValueId = characteristicValueId;
        }
        protected ItemProperty(ItemProperty itemProperty): base(itemProperty)
        {
            ItemVariantId = itemProperty.ItemVariantId;
            CharacteristicValueId = itemProperty.CharacteristicValueId;
        }

        public ItemProperty ShallowClone()
        {
            return (ItemProperty)MemberwiseClone();
        }
    }
}
