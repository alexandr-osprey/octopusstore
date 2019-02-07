using ApplicationCore.Entities;
using System;

namespace ApplicationCore.ViewModels
{
    public class OrderThumbnailViewModel : EntityViewModel<Order>
    {
        public DateTime DateTimeCreated { get; set; }
        public DateTime DateTimeFinished { get; set; }
        public DateTime DateTimeCancelled { get; set; }
        public OrderStatus Status { get; set; }
        public decimal Sum { get; set; }
        public ItemVariantViewModel ItemVariant { get; set; }
        public StoreViewModel StoreViewModel { get; set; }
        public int Number { get; set; }


        public OrderThumbnailViewModel() : base()
        {
        }

        public OrderThumbnailViewModel(Order order) : base(order)
        {
            ItemVariant = new ItemVariantViewModel(order.ItemVariant);
            StoreViewModel = new StoreViewModel(order.ItemVariant.Item.Store);
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
                ItemVariantId = ItemVariant.Id,
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
