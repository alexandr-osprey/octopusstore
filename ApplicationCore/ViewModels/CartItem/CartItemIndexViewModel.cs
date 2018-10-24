using ApplicationCore.Entities;
using System.Collections.Generic;
using System.Linq;

namespace ApplicationCore.ViewModels
{
    public class CartItemIndexViewModel : EntityIndexViewModel<CartItemViewModel, CartItem>
    {
        public CartItemIndexViewModel(int page, int totalPages, int totalCount, IEnumerable<CartItem> cartItems)
            : base(page, totalPages, totalCount, from cartItem in cartItems select new CartItemViewModel(cartItem))
        {
        }
    }
}
