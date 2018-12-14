using ApplicationCore.Entities;
using ApplicationCore.ViewModels;
using System.Threading.Tasks;

namespace ApplicationCore.Interfaces.Controllers
{
    public interface IOrderItemsController: IController<OrderItem, OrderItemViewModel>
    {
        Task<IndexViewModel<OrderItemViewModel>> IndexAsync();
    }
}
