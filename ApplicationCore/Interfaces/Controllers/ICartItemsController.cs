using ApplicationCore.Entities;
using ApplicationCore.ViewModels;
using System.Threading.Tasks;

namespace ApplicationCore.Interfaces.Controllers
{
    public interface ICartItemsController: IController<CartItem, CartItemViewModel>
    {
        Task<IndexViewModel<CartItemViewModel>> IndexAsync();
        Task<IndexViewModel<CartItemThumbnailViewModel>> IndexThumbnailsAsync();
        Task<CartItemThumbnailViewModel> AddToCartAsync(CartItemViewModel cartItemViewModel);
        Task<Response> RemoveFromCartAsync(CartItemViewModel cartItemViewModel);
    }
}
