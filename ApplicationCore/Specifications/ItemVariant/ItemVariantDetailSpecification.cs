using ApplicationCore.Entities;

namespace ApplicationCore.Specifications
{
    public class ItemVariantDetailSpecification : DetailSpecification<ItemVariant>
    {
        public ItemVariantDetailSpecification(int id)
            : base(id)
        {
            AddInclude("ItemVariantCharacteristicValues.CharacteristicValue.Characteristic");
        }
    }
}
