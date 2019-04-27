using ApplicationCore.Entities;

namespace ApplicationCore.Specifications
{
    public class ItemVariantThumbnailSpecification : EntitySpecification<ItemVariant>
    {
        public ItemVariantThumbnailSpecification(int id): base(id)
        {
            AddInclude(v => v.Images);
        }
    }
}
