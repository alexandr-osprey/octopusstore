using ApplicationCore.Entities;
using ApplicationCore.Specifications;
using System.Threading.Tasks;

namespace ApplicationCore.Interfaces.Services
{
    public interface IOrderService : IService<Order>
    {
        Task<Order> SetStatusAsync(int orderId, OrderStatus orderStatus);
        Task<OrderIndexSpecification> GetSpecificationAccordingToAuthorizationAsync(int page, int pageSize, int? storeId, OrderStatus? orderStatus);
    }
}
