using ApplicationCore.Entities;
using System;

namespace ApplicationCore.ViewModels
{
    public class OrderViewModel : EntityViewModel<Order>
    {
        public DateTime DateTimeCreated { get; set; }
        public DateTime DateTimeFinished { get; set; }
        public DateTime DateTimeCancelled { get; set; }
        public OrderStatus Status { get; set; }
        public decimal Sum { get; set; }
        public int ItemVariantId { get; set; }
        public int Number { get; set; }
        public string CustomerEmail { get; set; }

        public OrderViewModel() : base()
        {
        }

        public OrderViewModel(Order order) : base(order)
        {
            ItemVariantId = order.ItemVariantId;
            Number = order.Number;
            DateTimeCreated = order.DateTimeCreated;
            DateTimeFinished = order.DateTimeFinished;
            DateTimeCancelled = order.DateTimeCancelled;
            Status = order.Status;
            Sum = order.Sum;
        }

        public override Order ToModel()
        {
            return new Order()
            {
                Id = Id,
                ItemVariantId = ItemVariantId,
                Number = Number,
                DateTimeCreated = DateTimeCreated,
                DateTimeFinished = DateTimeFinished,
                DateTimeCancelled = DateTimeCancelled,
                Status = Status,
                Sum = Sum
            };
        }

        public override Order UpdateModel(Order modelToUpdate)
        {
            modelToUpdate.Sum = Sum;
            modelToUpdate.Number = Number;
            return modelToUpdate;
        }
    }
}
