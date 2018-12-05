using ApplicationCore.Entities;
using ApplicationCore.Exceptions;
using ApplicationCore.Identity;
using ApplicationCore.Interfaces;
using ApplicationCore.Interfaces.Services;
using ApplicationCore.Specifications;
using Infrastructure.Data;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class OrderService : Service<Order>, IOrderService
    {
        public OrderService(
            StoreContext context,
            IIdentityService identityService,
            IScopedParameters scopedParameters,
            IAuthorizationParameters<Order> authoriationParameters,
            IAppLogger<Service<Order>> logger)
           : base(context, identityService, scopedParameters, authoriationParameters, logger)
        {
        }

        protected override async Task ValidateCreateWithExceptionAsync(Order order)
        {
            await base.ValidateCreateWithExceptionAsync(order);
            if (order.Sum < 0)
            {
                throw new EntityValidationException($"Order sum can't be negative");
            }
            if (!await Context.ExistsBySpecAsync(Logger, new EntitySpecification<Store>(order.StoreId)))
            {
                throw new EntityValidationException($"Store {order.StoreId} does not exist");
            }
        }

        protected override Task ValidateUpdateWithExceptionAsync(Order order)
        {
            return ValidateCreateWithExceptionAsync(order);
        }

        public override async Task RelinkRelatedAsync(int id, int idToRelinkTo)
        {
            var orderItems = await Context.EnumerateRelatedEnumAsync(Logger, new EntitySpecification<Order>(id), b => b.OrderItems);
            foreach (var orderItem in orderItems)
                orderItem.OrderId = idToRelinkTo;
            await Context.SaveChangesAsync(Logger, "Relink Order");
        }
    }
}
