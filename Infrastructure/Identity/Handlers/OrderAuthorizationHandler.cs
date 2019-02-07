using ApplicationCore.Entities;
using ApplicationCore.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Identity
{
    public class OrderAuthorizationHandler: DefaultAuthorizationHandler<Order>
    {
        public OrderAuthorizationHandler(UserManager<ApplicationUser> userManager, IAppLogger<IAuthorziationHandler<Order>> appLogger) : base(userManager, appLogger)
        {
        }

        //protected override async Task<Store> GetStoreEntityAsync(Order order)
        //{
        //    var itemVariant = await _storeContext.ReadSingleBySpecAsync(_logger, new Specification<ItemVariant>(v => v.Id == order.ItemVariantId, v => v.Item), true);
        //    return await _storeContext.ReadByKeyAsync<Store, IAuthorziationHandler<Order>>(_logger, itemVariant.Item.StoreId, true);
        //}
    }
}
