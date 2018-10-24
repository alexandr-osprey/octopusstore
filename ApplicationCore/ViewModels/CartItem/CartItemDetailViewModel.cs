using ApplicationCore.Entities;

namespace ApplicationCore.ViewModels
{
    public class CartItemDetailViewModel : EntityDetailViewModel<CartItem>
    {
        public int ItemVariantId { get; set; }
        public int Number { get; set; }

        public CartItemDetailViewModel(CartItem cartItem): base(cartItem)
        {
            ItemVariantId = cartItem.ItemVariantId;
            Number = cartItem.Number;
        }

        public override CartItem ToModel()
        {
            return new CartItem()
            {
                Id = Id,
                ItemVariantId = ItemVariantId,
                Number = Number
            };
        }
        public override CartItem UpdateModel(CartItem modelToUpdate)
        {
            modelToUpdate.Number = modelToUpdate.Number;
            return modelToUpdate;
        }
    }
}
