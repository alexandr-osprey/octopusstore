using ApplicationCore.Entities;

namespace ApplicationCore.Specifications
{
    public class ItemVariantCharacteristicValueSpecification : Specification<ItemVariantCharacteristicValue>
    {
        public ItemVariantCharacteristicValueSpecification(int itemVariantId)
            : base((i => i.ItemVariantId == itemVariantId))
        {
            Description = $"ItemVariantCharacteristics with ItemVariantId={itemVariantId}";
        }
    }
}
