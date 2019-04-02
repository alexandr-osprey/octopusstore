using ApplicationCore.Entities;
using ApplicationCore.Exceptions;
using ApplicationCore.Identity;
using ApplicationCore.Interfaces;
using ApplicationCore.Interfaces.Services;
using ApplicationCore.Specifications;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
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

        protected override async Task ValidateWithExceptionAsync(EntityEntry<Order> orderEntry)
        {
            await base.ValidateWithExceptionAsync(orderEntry);
            var order = orderEntry.Entity;
            if (order.Sum < 0)
                throw new EntityValidationException($"Order sum can't be negative");
            if (order.Number <= 0)
                throw new EntityValidationException("Number must be positive");
            if (IsPropertyModified(orderEntry, o => o.ItemVariantId, false))
            {
                if (IsPropertyModified(orderEntry, o => o.ItemVariantId, false))
                {
                    var itemVariant = await GetItemVariantAsync(order);
                }
            }
            ValidateStatusWithException(orderEntry);
        }


        protected override async Task ModifyBeforeSaveAsync(EntityEntry<Order> entry)
        {
            await base.ModifyBeforeSaveAsync(entry);
            var order = entry.Entity;
            if (entry.State == EntityState.Added)
            {

                // set sum from database, not model
                var itemVariant = await GetItemVariantAsync(order);
                order.Sum = itemVariant.Price * order.Number;
                order.DateTimeCreated = DateTime.UtcNow;
            }
            var statusProperty = entry.Property(o => o.Status);
            if (statusProperty.IsModified)
            {
                if (statusProperty.OriginalValue == OrderStatus.Created)
                {
                    if (statusProperty.CurrentValue == OrderStatus.Cancelled)
                        order.DateTimeCancelled = DateTime.UtcNow;
                    else if (statusProperty.CurrentValue == OrderStatus.Finished)
                        order.DateTimeFinished = DateTime.UtcNow;
                }
            }
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
                if (statusProperty.OriginalValue == OrderStatus.Created
                    && !(statusProperty.CurrentValue == OrderStatus.Cancelled || statusProperty.CurrentValue == OrderStatus.Finished))
                {
                    throw new EntityValidationException("Unsupported order status " + orderEntry.Entity.Status);
                }
                else if (statusProperty.OriginalValue == OrderStatus.Finished)
                    throw new EntityValidationException("Order already finished, can't change status");
                else if (statusProperty.OriginalValue == OrderStatus.Cancelled)
                    throw new EntityValidationException("Order already cancelled, can't change status");
            }
        }

        protected async Task<ItemVariant> GetItemVariantAsync(Order order)
        {
            if (order == null)
                throw new ArgumentNullException(nameof(order));

            var itemVariant = await Context
                        .Set<ItemVariant>()
                        .Include(iv => iv.Item)
                            .ThenInclude(i => i.Store)
                        .FirstOrDefaultAsync(iv => iv.Id == order.ItemVariantId)
                            ?? throw new EntityValidationException($"Item variant {order.ItemVariantId} not found");
            return itemVariant;
        }

        public async Task<OrderIndexSpecification> GetSpecificationAccordingToAuthorizationAsync(int page, int pageSize, int? storeId, OrderStatus? orderStatus)
        {
            string ownerId = null;
            if (!storeId.HasValue)
                ownerId = ScopedParameters.CurrentUserId;
            else
            {
                if (!(await IdentityService.IsStoreAdministratorAsync(ScopedParameters.CurrentUserId, storeId.Value)
                        || await IdentityService.IsContentAdministratorAsync(ScopedParameters.CurrentUserId)))
                {
                    throw new AuthorizationException("Current user has no authorization to read orders from this store");
                }
            }
            return new OrderIndexSpecification(page, pageSize, storeId, orderStatus, ownerId);
        }
    }
}
