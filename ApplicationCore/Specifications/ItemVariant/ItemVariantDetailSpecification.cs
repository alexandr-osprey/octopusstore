using ApplicationCore.Entities;

namespace ApplicationCore.Specifications
{
    public class ItemVariantDetailSpecification: ItemVariantThumbnailSpecification
    {
        public ItemVariantDetailSpecification(int id): base(id)
        {
            AddInclude("ItemProperties.CharacteristicValue.Characteristic");
        }
    }
}
