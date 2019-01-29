using ApplicationCore.Entities;
using System.Threading.Tasks;

namespace ApplicationCore.Interfaces.Services
{
    public interface IOrderService : IService<Order>
    {
        Task<Order> SetStatusAsync(int orderId, OrderStatus orderStatus);
        Task<Order> RecalculateSumAsync(int orderId);
    }
}
