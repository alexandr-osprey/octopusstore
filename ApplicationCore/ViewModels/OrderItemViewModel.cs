using ApplicationCore.Entities;

namespace ApplicationCore.ViewModels
{
    public class OrderItemViewModel : EntityViewModel<OrderItem>
    {
        public int ItemVariantId { get; set; }
        public int Number { get; set; }
        public int OrderId { get; set; }

        public OrderItemViewModel() : base()
        {
        }

        public OrderItemViewModel(OrderItem orderItem) : base(orderItem)
        {
            ItemVariantId = orderItem.ItemVariantId;
            Number = orderItem.Number;
            OrderId = orderItem.OrderId;
        }

        public override OrderItem ToModel()
        {
            return new OrderItem()
            {
                Id = Id,
                ItemVariantId = ItemVariantId,
                Number = Number,
                OrderId = OrderId,
            };
        }

        public override OrderItem UpdateModel(OrderItem modelToUpdate)
        {
            modelToUpdate.Number = modelToUpdate.Number;
            return modelToUpdate;
        }
    }
}
