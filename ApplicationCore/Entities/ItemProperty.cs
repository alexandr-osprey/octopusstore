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

        public virtual CharacteristicValue CharacteristicValue { get; set; }
        public virtual ItemVariant ItemVariant { get; set; }

        public ItemProperty()
        {
        }

        public bool Equals(ItemProperty other) => base.Equals(other)
           && ItemVariantId == other.ItemVariantId
           && CharacteristicValueId == other.CharacteristicValueId;
        public override bool Equals(object obj) => Equals(obj as ItemProperty);
        public override int GetHashCode() => base.GetHashCode();

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
