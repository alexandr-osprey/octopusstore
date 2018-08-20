using ApplicationCore.Entities;
using ApplicationCore.Specifications;

namespace OctopusStore.Specifications
{
    public class ItemThumbnailSpecification : Specification<Item>
    {
        public ItemThumbnailSpecification(int id)
            : base(id)
        {
            AddInclude(i => i.Images);
            AddInclude(i => i.Brand);
            AddInclude(i => i.Category);
            AddInclude(i => i.MeasurementUnit);
            AddInclude(i => i.Store);
            AddInclude(i => i.ItemVariants);
        }
    }
}
