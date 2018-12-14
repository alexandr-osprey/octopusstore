using ApplicationCore.Interfaces;

namespace ApplicationCore.Entities
{
    public class OrderItem : Entity, IGenericMemberwiseClonable<OrderItem>
    {
        public int ItemVariantId { get; set; }
        public int Number { get; set; }
        public int OrderId { get; set; }

        public virtual ItemVariant ItemVariant { get; set; }
        public virtual Order Order { get; set; }

        public OrderItem() : base()
        {
        }

        public OrderItem(CartItem cartItem, int orderId) : base()
        {
            ItemVariantId = cartItem.ItemVariantId;
            Number = cartItem.Number;
            OrderId = orderId;
        }

        public bool Equals(OrderItem orderItem) => base.Equals(orderItem)
            && OrderId == orderItem.OrderId
            && ItemVariantId == orderItem.ItemVariantId
            && Number == orderItem.Number;

        public override bool Equals(object obj) => Equals(obj as OrderItem);
        public override int GetHashCode() => base.GetHashCode();

        protected OrderItem(OrderItem orderItem) : base(orderItem)
        {
            OrderId = orderItem.OrderId;
            ItemVariantId = orderItem.ItemVariantId;
            Number = orderItem.Number;
        }

        public OrderItem ShallowClone()
        {
            return (OrderItem)MemberwiseClone();
        }
    }
}
