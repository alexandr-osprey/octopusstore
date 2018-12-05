using ApplicationCore.Interfaces;
using System;
using System.Collections.Generic;

namespace ApplicationCore.Entities
{
    public enum OrderStatus
    {
        Created,
        Finished,
        Candelled
    }

    public class Order : Entity, IGenericMemberwiseClonable<Order>
    {
        public int StoreId { get; set; }
        public DateTime DateTimeCreated { get; set; }
        public DateTime DateTimeFinished { get; set; }
        public DateTime DateTimeCancelled { get; set; }
        public OrderStatus Status { get; set; }
        public decimal Sum { get; set; }

        public Store Store { get; set; }
        public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

        public Order() : base()
        {
            Status = OrderStatus.Created;
            DateTimeCreated = DateTime.UtcNow;
        }

        public bool Equals(Order order) => base.Equals(order)
            && StoreId == order.StoreId
            && DateTimeCreated == order.DateTimeCreated
            && DateTimeFinished == order.DateTimeFinished
            && DateTimeCancelled == order.DateTimeCancelled
            && Status == order.Status
            && Sum == order.Sum;
        public override bool Equals(object obj) => Equals(obj as Order);
        public override int GetHashCode() => base.GetHashCode();

        protected Order(Order order) : base(order)
        {
            StoreId = order.StoreId;
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
