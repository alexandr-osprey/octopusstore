using ApplicationCore.Entities;
using ApplicationCore.Exceptions;
using ApplicationCore.Identity;
using ApplicationCore.Interfaces;
using ApplicationCore.Interfaces.Services;
using ApplicationCore.Specifications;
using Infrastructure.Data;
using System;
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
            await ValidateUpdateWithExceptionAsync(order);
            if (!await Context.ExistsBySpecAsync(Logger, new EntitySpecification<Store>(order.StoreId)))
                throw new EntityValidationException($"Store {order.StoreId} does not exist");
        }

        protected override Task ValidateUpdateWithExceptionAsync(Order order)
        {
            if (order.Sum < 0)
                throw new EntityValidationException($"Order sum can't be negative");
            return Task.CompletedTask;
        }

        public override async Task RelinkRelatedAsync(int id, int idToRelinkTo)
        {
            var orderItems = await Context.EnumerateRelatedEnumAsync(Logger, new EntitySpecification<Order>(id), b => b.OrderItems);
            foreach (var orderItem in orderItems)
                orderItem.OrderId = idToRelinkTo;
            await Context.SaveChangesAsync(Logger, "Relink Order");
        }

        public async Task<Order> SetStatusAsync(int orderId, OrderStatus orderStatus)
        {
            var order = await Context.ReadSingleBySpecAsync(Logger, new EntitySpecification<Order>(orderId), true);
            if (orderStatus == OrderStatus.Created)
                throw new BadRequestException("Can't set Created status Manually");
            if (order.Status == OrderStatus.Created)
            {
                if (orderStatus == OrderStatus.Cancelled)
                {
                    order.Status = orderStatus;
                    order.DateTimeCancelled = DateTime.UtcNow;
                }
                else if (orderStatus == OrderStatus.Finished)
                {
                    order.Status = orderStatus;
                    order.DateTimeFinished = DateTime.UtcNow;
                }
                else
                    throw new BadRequestException("Unsupported order status " + orderStatus);
                await Context.UpdateSingleAsync(Logger, order, false);
            }
            else if (order.Status == OrderStatus.Finished)
                throw new BadRequestException("Order already finished, can't change status");
            else if (order.Status == OrderStatus.Cancelled)
                throw new BadRequestException("Order already cancelled, can't change status");
            return order;
        }
    }
}
