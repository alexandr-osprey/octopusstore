using ApplicationCore.Entities;
using ApplicationCore.ViewModels;
using System.Threading.Tasks;

namespace ApplicationCore.Interfaces.Controllers
{
    public interface IOrdersController: IController<Order, OrderViewModel>
    {
        Task<IndexViewModel<OrderViewModel>> IndexAsync();
    }
}
