using ApplicationCore.Entities;

namespace ApplicationCore.ViewModels
{
    public class OrderDetailViewModel : OrderViewModel
    {
        public ItemVariantViewModel ItemVariant { get; set; }
        public StoreViewModel Store { get; set; }

        public OrderDetailViewModel(Order order) : base(order)
        {
            ItemVariant = new ItemVariantViewModel(order.ItemVariant);
            Store = new StoreViewModel(order.ItemVariant.Item.Store);
        }
    }
}
