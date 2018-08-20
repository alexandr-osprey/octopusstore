using ApplicationCore.Entities;

namespace OctopusStore.Specifications
{
    public class ItemDetailSpecification : DetailSpecification<Item>
    {
        public ItemDetailSpecification(int id)
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
