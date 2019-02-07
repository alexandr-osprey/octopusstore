using ApplicationCore.Entities;

namespace ApplicationCore.ViewModels
{
    public class OrderThumbnailViewModel : OrderViewModel
    {
        public ItemVariantViewModel ItemVariant { get; set; }
        public StoreViewModel Store { get; set; }

        public OrderThumbnailViewModel() : base()
        {
        }

        public OrderThumbnailViewModel(Order order) : base(order)
        {
            ItemVariant = new ItemVariantViewModel(order.ItemVariant);
            Store = new StoreViewModel(order.ItemVariant.Item.Store);
        }
    }
}
