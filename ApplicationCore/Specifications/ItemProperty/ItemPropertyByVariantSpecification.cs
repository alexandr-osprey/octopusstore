using ApplicationCore.Entities;

namespace ApplicationCore.Specifications
{
    public class ItemPropertyByVariantSpecification: Specification<ItemProperty>
    {
        public ItemPropertyByVariantSpecification(int itemVariantId): base((i => i.ItemVariantId == itemVariantId))
        {
            Description = $"ItemVariantCharacteristics with ItemVariantId={itemVariantId}";
        }
    }
}
