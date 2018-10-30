using ApplicationCore.Entities;
using ApplicationCore.ViewModels;
using System.Threading.Tasks;

namespace ApplicationCore.Interfaces.Controllers
{
    public interface ICartItemsController: IController<CartItem, CartItemViewModel>
    {
        Task<CartItemViewModel> AddToCart(CartItemViewModel cartItemViewModel);
        Task<Response> RemoveFromCart(CartItemViewModel cartItemViewModel);
    }
}
