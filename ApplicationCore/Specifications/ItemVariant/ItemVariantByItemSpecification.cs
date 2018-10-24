using ApplicationCore.Entities;
using ApplicationCore.Specifications;

namespace ApplicationCore.Specifications
{
    public class ItemVariantByItemSpecification : Specification<ItemVariant>
    {
        public ItemVariantByItemSpecification(int itemId)
            : base((v => v.ItemId == itemId))
        {
            Description = $"ItemVariant with ItemId={itemId}";
        }
    }
}
