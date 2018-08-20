using ApplicationCore.Entities;
using ApplicationCore.Specifications;

namespace OctopusStore.Specifications
{
    public class ItemVariantCharacteristicValueSpecification : Specification<ItemVariantCharacteristicValue>
    {
        public ItemVariantCharacteristicValueSpecification(int itemVariantId)
            : base((i => i.ItemVariantId == itemVariantId))
        {
            Description = $"ItemVariantCharacteristicValue with ItemVariantId={itemVariantId}";
        }
    }
}
