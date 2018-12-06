using ApplicationCore.Entities;
using System;

namespace ApplicationCore.ViewModels
{
    public class OrderViewModel : EntityViewModel<Order>
    {
        public int StoreId { get; set; }
        public DateTime DateTimeCreated { get; set; }
        public DateTime DateTimeFinished { get; set; }
        public DateTime DateTimeCancelled { get; set; }
        public OrderStatus Status { get; set; }
        public decimal Sum { get; set; }

        public OrderViewModel() : base()
        {
        }

        public OrderViewModel(Order order) : base(order)
        {
            StoreId = order.StoreId;
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
                StoreId = StoreId,
                DateTimeCreated = DateTimeCreated,
                DateTimeFinished = DateTimeFinished,
                DateTimeCancelled = DateTimeCancelled,
                Status = Status,
                Sum = Sum
            };
        }

        public override Order UpdateModel(Order modelToUpdate)
        {
            modelToUpdate.Sum = modelToUpdate.Sum;
            return modelToUpdate;
        }
    }
}
