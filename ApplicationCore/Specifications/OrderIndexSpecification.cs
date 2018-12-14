using ApplicationCore.Entities;

namespace ApplicationCore.Specifications
{
    public class OrderIndexSpecification: EntitySpecification<Order>
    {
        public OrderIndexSpecification(int pageIndex, int pageSize, int? storeId, OrderStatus? orderStatus, string ownerId)
           : base(i => (!HasValue(ownerId) || i.OwnerId.Equals(ownerId)) &&
                        (!storeId.HasValue || i.StoreId == storeId) &&
                        (!orderStatus.HasValue || i.Status == orderStatus)) 
        {
            SetPaging(pageIndex, pageSize);

            Description = $"{nameof(pageIndex)}: {pageIndex}, {nameof(pageSize)}: {pageSize}" +
                $", {nameof(ownerId)}: { ownerId ?? "null" }" +
                $", {nameof(storeId)} (0 - not provided): {storeId ?? 0}" +
                $", {nameof(orderStatus)} (0 - not provided): {orderStatus ?? 0}";
        }
    }
}
