using ApplicationCore.Entities;
using ApplicationCore.Exceptions;
using ApplicationCore.Identity;
using ApplicationCore.Interfaces;
using ApplicationCore.Interfaces.Services;
using ApplicationCore.Specifications;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class OrderItemService : Service<OrderItem>, IOrderItemService
    {
        public OrderItemService(
            StoreContext context,
            IIdentityService identityService,
            IScopedParameters scopedParameters,
            IAuthorizationParameters<OrderItem> authoriationParameters,
            IAppLogger<Service<OrderItem>> logger)
           : base(context, identityService, scopedParameters, authoriationParameters, logger)
        {
        }

        protected override async Task ValidationWithExceptionAsync(OrderItem orderItem)
        {
            await base.ValidationWithExceptionAsync(orderItem);
            if (orderItem.Number <= 0)
                throw new EntityValidationException("Number must be positive");
            var orderEntry = Context.Entry(orderItem);
            if (IsPropertyModified(orderEntry, o => o.OrderId, false)
                | IsPropertyModified(orderEntry, o => o.ItemVariantId, false))
            {
                var order = await Context.ReadSingleBySpecAsync(Logger, new EntitySpecification<Order>(orderItem.OrderId), false)
                    ?? throw new EntityValidationException($"Order {orderItem.OrderId} not found");
                if (IsPropertyModified(orderEntry, o => o.ItemVariantId, false))
                {
                    var itemVariant = await Context
                        .Set<ItemVariant>()
                        .Include(iv => iv.Item)
                            .ThenInclude(i => i.Store)
                        .FirstOrDefaultAsync(iv => iv.Id == orderItem.ItemVariantId)
                            ?? throw new EntityValidationException($"Item variant {orderItem.ItemVariantId} not found");
                    if (itemVariant.Item.Store.Id != order.StoreId)
                        throw new EntityValidationException("Incorrect item variant for store in order");
                }
            }

        }
    }
}
