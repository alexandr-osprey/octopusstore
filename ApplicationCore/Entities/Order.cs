using ApplicationCore.Interfaces;
using System;

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
        public int ItemVariantId { get; set; }
        public int Number { get; set; }
        public DateTime DateTimeCreated { get; set; }
        public DateTime DateTimeFinished { get; set; }
        public DateTime DateTimeCancelled { get; set; }
        public OrderStatus Status { get; set; }
        public decimal Sum { get; set; }

        public virtual ItemVariant ItemVariant { get; set; }

        public Order() : base()
        {
        }

        public bool Equals(Order other) => base.Equals(other)
            && ItemVariantId == other.ItemVariantId
            && Number == other.Number
            && DateTimeCreated == other.DateTimeCreated
            && DateTimeFinished == other.DateTimeFinished
            && DateTimeCancelled == other.DateTimeCancelled
            && Status == other.Status
            && Sum == other.Sum;
        public override bool Equals(object obj) => Equals(obj as CartItem);
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
