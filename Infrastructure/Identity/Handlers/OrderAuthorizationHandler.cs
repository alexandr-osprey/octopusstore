using System.Threading.Tasks;
using ApplicationCore.Entities;
using ApplicationCore.Interfaces;
using Infrastructure.Data;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Identity
{
    public class OrderAuthorizationHandler: StoreEntityAuthorizationHandler<Order>
    {
        public OrderAuthorizationHandler(UserManager<ApplicationUser> userManager, StoreContext storeContext, IAppLogger<IAuthorziationHandler<Order>> appLogger)
           : base(userManager, storeContext, appLogger)
        {
        }

        protected override async Task<Store> GetStoreEntityAsync(Order order)
        {
            return await _storeContext.ReadByKeyAsync<Store, IAuthorziationHandler<Order>>(_logger, order.StoreId, true);
        }
    }
}
