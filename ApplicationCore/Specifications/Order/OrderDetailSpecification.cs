using ApplicationCore.Entities;

namespace ApplicationCore.Specifications
{
    public class OrderDetailSpecification: EntitySpecification<Order>
    {
        public OrderDetailSpecification(int id) : base(id)
        {
            SetIncludes();
        }

        protected void SetIncludes()
        {
            AddInclude(o => o.ItemVariant.Item.Store);
        }
    }
}
