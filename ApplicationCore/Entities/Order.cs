using ApplicationCore.Interfaces;
using System;
using System.Collections.Generic;

namespace ApplicationCore.Entities
{
    public enum OrderStatus
    {
        Created,
        Finished,
        Cancelled
    }

    public class Order : Entity, ShallowClonable<Order>
    {
        public DateTime DateTimeCreated { get; set; }
        public DateTime DateTimeFinished { get; set; }
        public DateTime DateTimeCancelled { get; set; }
        public OrderStatus Status { get; set; }
        public decimal Sum { get; set; }
        public int ItemVariantId { get; set; }
        public int Number { get; set; }

        public ItemVariant ItemVariant { get; set; }

        public Order() : base()
        {
            Status = OrderStatus.Created;
            DateTimeCreated = DateTime.UtcNow;
        }

        public Order(CartItem cartItem) : this()
        {
            ItemVariantId = cartItem.ItemVariantId;
            Number = cartItem.Number;
        }

        public bool Equals(Order order) => base.Equals(order)
            && ItemVariantId == order.ItemVariantId
            && Number == order.Number
            && DateTimeCreated == order.DateTimeCreated
            && DateTimeFinished == order.DateTimeFinished
            && DateTimeCancelled == order.DateTimeCancelled
            && Status == order.Status
            && Sum == order.Sum;
        public override bool Equals(object obj) => Equals(obj as Order);
        public override int GetHashCode() => base.GetHashCode();

        protected Order(Order order) : base(order)
        {
            ItemVariantId = order.ItemVariantId;
            Number = order.Number;
            DateTimeCreated = order.DateTimeCreated;
            DateTimeFinished = order.DateTimeFinished;
            DateTimeCancelled = order.DateTimeCancelled;
            Sum = order.Sum;
        }

        public Order ShallowClone()
        {
            return (Order)MemberwiseClone();
        }
    }
}
