using ApplicationCore.Entities;
using ApplicationCore.ViewModels;
using System.Threading.Tasks;

namespace ApplicationCore.Interfaces.Controllers
{
    public interface IOrdersController: IController<Order, OrderViewModel>
    {
        Task<IndexViewModel<OrderViewModel>> IndexAsync(int? page, int? pageSize, int? storeId, OrderStatus? orderStatus);
        Task<IndexViewModel<OrderThumbnailViewModel>> IndexThumbnailsAsync(int? page, int? pageSize, int? storeId, OrderStatus? orderStatus);
    }
}
