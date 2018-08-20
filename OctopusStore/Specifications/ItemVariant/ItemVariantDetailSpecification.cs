using ApplicationCore.Entities;

namespace OctopusStore.Specifications
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
