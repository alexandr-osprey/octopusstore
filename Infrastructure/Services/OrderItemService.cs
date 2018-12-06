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

        protected override async Task FullValidationWithExceptionAsync(OrderItem orderItem)
        {
            await base.FullValidationWithExceptionAsync(orderItem);
            var order = await Context.ReadSingleBySpecAsync(Logger, new EntitySpecification<Order>(orderItem.OrderId), false)
                ?? throw new EntityValidationException($"Order {orderItem.OrderId} not found");
            var itemVariant = await Context
                .Set<ItemVariant>()
                .Include(iv => iv.Item)
                    .ThenInclude(i => i.Store)
                .FirstOrDefaultAsync(iv => iv.Id == orderItem.ItemVariantId) 
                ?? throw new EntityValidationException($"Item variant {orderItem.ItemVariantId} not found");
            if (itemVariant.Item.Store.Id != order.StoreId)
                throw new EntityValidationException("Incorrect item variant for store in order");
        }

        protected override async Task PartialValidationWithExceptionAsync(OrderItem orderItem)
        {
            await base.PartialValidationWithExceptionAsync(orderItem);
            if (orderItem.Number <= 0)
                throw new EntityValidationException("Number must be positive");
        }
    }
}
