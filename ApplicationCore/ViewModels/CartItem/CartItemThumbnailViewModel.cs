using ApplicationCore.Entities;

namespace ApplicationCore.ViewModels
{
    public class CartItemThumbnailViewModel : EntityViewModel<CartItem>
    {
        public ItemVariantViewModel ItemVariant { get; set; }
        public ItemViewModel Item { get; set; }
        public int Number { get; set; }

        public CartItemThumbnailViewModel(): base()
        {
        }

        public CartItemThumbnailViewModel(CartItem cartItem): base(cartItem)
        {
            ItemVariant = new ItemVariantViewModel(cartItem.ItemVariant);
            Item = new ItemViewModel(cartItem.ItemVariant.Item);
            Number = cartItem.Number;
        }

        public override CartItem ToModel()
        {
            return new CartItem()
            {
                Id = Id,
                ItemVariantId = ItemVariant.Id,
                Number = Number
            };
        }
        public override CartItem UpdateModel(CartItem modelToUpdate)
        {
            modelToUpdate.Number = Number;
            return modelToUpdate;
        }
    }
}
