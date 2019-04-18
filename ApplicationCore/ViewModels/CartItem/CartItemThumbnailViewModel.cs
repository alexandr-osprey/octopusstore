using ApplicationCore.Entities;
using System.Linq;

namespace ApplicationCore.ViewModels
{
    public class CartItemThumbnailViewModel : CartItemViewModel
    {
        public ItemVariantViewModel ItemVariant { get; set; }
        public ItemViewModel Item { get; set; }
        public StoreViewModel Store { get; set; }
        public ItemVariantImageViewModel ItemVariantImage { get; set; }

        public CartItemThumbnailViewModel(): base()
        {
        }

        public CartItemThumbnailViewModel(CartItem cartItem): base(cartItem)
        {
            ItemVariant = new ItemVariantViewModel(cartItem.ItemVariant);
            Item = new ItemViewModel(cartItem.ItemVariant.Item);
            Number = cartItem.Number;
            Store = new StoreViewModel(cartItem.ItemVariant.Item.Store);
            ItemVariantImage = new ItemVariantImageViewModel(cartItem.ItemVariant.Images.FirstOrDefault());
        }
    }
}
