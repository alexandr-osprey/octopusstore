using ApplicationCore.Entities;

namespace ApplicationCore.Specifications
{
    public class OrderThumbnailIndexSpecification: OrderIndexSpecification
    {
        public OrderThumbnailIndexSpecification(OrderIndexSpecification orderIndexSpecification) : base(orderIndexSpecification)
        {

        }

        public OrderThumbnailIndexSpecification(int pageIndex, int pageSize, int? storeId, OrderStatus? orderStatus, string ownerId)
           : base(pageIndex, pageSize, storeId, orderStatus, ownerId) 
        {
            SetPaging(pageIndex, pageSize);
        }

        protected void SetIncludes()
        {
            AddInclude(o => o.ItemVariant.Item.Store);
        }
    }
}
