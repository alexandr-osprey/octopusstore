using ApplicationCore.Entities;
using ApplicationCore.Exceptions;
using ApplicationCore.Identity;
using ApplicationCore.Interfaces;
using ApplicationCore.Interfaces.Services;
using ApplicationCore.Specifications;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Linq;
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

        protected override async Task ValidationWithExceptionAsync(Order order)
        {
            await base.ValidationWithExceptionAsync(order);
            if (order.Sum < 0)
                throw new EntityValidationException($"Order sum can't be negative");
            if (!await Context.ExistsBySpecAsync(Logger, new EntitySpecification<Store>(order.StoreId)))
                throw new EntityValidationException($"Store {order.StoreId} does not exist");
            var orderEntry = Context.Entry(order);
            if (IsPropertyModified(orderEntry, o => o.StoreId, false)
                && !await Context.ExistsBySpecAsync(Logger, new EntitySpecification<Store>(order.StoreId), false))
                throw new EntityValidationException($"Store with id {order.StoreId} does not exist");
            ValidateStatusWithException(orderEntry);
        }

        public async Task<Order> RecalculateSumAsync(int orderId)
        {
            var spec = new EntitySpecification<Order>(orderId);
            spec.AddInclude("OrderItems.ItemVariant");
            var order = await Context.ReadSingleBySpecAsync(Logger, spec, true);
            order.Sum = (from o in order.OrderItems select o.ItemVariant.Price * o.Number).Sum();
            await Context.SaveChangesAsync(Logger, $"Recalculating order {orderId} sum");
            return order;
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
            order.Status = orderStatus;
            await UpdateAsync(order);
            return order;
        }

        protected void ValidateStatusWithException(EntityEntry<Order> orderEntry)
        {
            var statusProperty = orderEntry.Property(o => o.Status);
            if (statusProperty.IsModified)
            {
                if (statusProperty.CurrentValue == OrderStatus.Created)
                    throw new EntityValidationException("Can't set Created status Manually");
                if (statusProperty.OriginalValue == OrderStatus.Created)
                {
                    if (statusProperty.CurrentValue == OrderStatus.Cancelled)
                        orderEntry.Entity.DateTimeCancelled = DateTime.UtcNow;
                    else if (statusProperty.CurrentValue == OrderStatus.Finished)
                        orderEntry.Entity.DateTimeFinished = DateTime.UtcNow;
                    else
                        throw new EntityValidationException("Unsupported order status " + orderEntry.Entity.Status);
                }
                else if (statusProperty.OriginalValue == OrderStatus.Finished)
                    throw new EntityValidationException("Order already finished, can't change status");
                else if (statusProperty.OriginalValue == OrderStatus.Cancelled)
                    throw new EntityValidationException("Order already cancelled, can't change status");
            }
        }
    }
}
