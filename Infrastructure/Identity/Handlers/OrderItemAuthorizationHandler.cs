using System.Threading.Tasks;
using ApplicationCore.Entities;
using ApplicationCore.Interfaces;
using Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Identity
{
    public class OrderItemAuthorizationHandler: StoreEntityAuthorizationHandler<OrderItem>
    {
        public OrderItemAuthorizationHandler(UserManager<ApplicationUser> userManager, StoreContext storeContext, IAppLogger<IAuthorziationHandler<OrderItem>> appLogger)
           : base(userManager, storeContext, appLogger)
        {
        }

        protected override async Task<Store> GetStoreEntityAsync(OrderItem order)
        {
            var orderItem = await _storeContext
                .Set<OrderItem>()
                .Include(i => i.Order)
                    .ThenInclude(o => o.Store)
                .FirstAsync();
            return orderItem.Order.Store;
        }
    }
}
