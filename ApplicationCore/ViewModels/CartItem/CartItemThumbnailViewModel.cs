using ApplicationCore.Entities;

namespace ApplicationCore.ViewModels
{
    public class CartItemThumbnailViewModel : CartItemViewModel
    {
        public ItemVariantViewModel ItemVariant { get; set; }
        public ItemViewModel Item { get; set; }
        public MeasurementUnitViewModel MeasurementUnit { get; set; }
        public StoreViewModel Store { get; set; }

        public CartItemThumbnailViewModel(): base()
        {
        }

        public CartItemThumbnailViewModel(CartItem cartItem): base(cartItem)
        {
            ItemVariant = new ItemVariantViewModel(cartItem.ItemVariant);
            Item = new ItemViewModel(cartItem.ItemVariant.Item);
            MeasurementUnit = new MeasurementUnitViewModel(cartItem.ItemVariant.Item.MeasurementUnit);
            Number = cartItem.Number;
            Store = new StoreViewModel(cartItem.ItemVariant.Item.Store);
        }
    }
}
