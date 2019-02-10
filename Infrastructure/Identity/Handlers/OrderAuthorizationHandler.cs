using ApplicationCore.Entities;
using ApplicationCore.Interfaces;
using ApplicationCore.Specifications;
using Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace Infrastructure.Identity
{
    public class OrderAuthorizationHandler: StoreEntityAuthorizationHandler<Order>
    {
        public OrderAuthorizationHandler(UserManager<ApplicationUser> userManager, StoreContext storeContext, IAppLogger<IAuthorziationHandler<Order>> appLogger) : base(userManager, storeContext, appLogger)
        {
            ValidateRightsOnEnityProperties = false;
        }

        protected override async Task<int> GetStoreIdAsync(Order order)
        {
            var itemVariant = await _storeContext.ReadSingleBySpecAsync(_logger, new Specification<ItemVariant>(v => v.Id == order.ItemVariantId, v => v.Item), true);
            return itemVariant.Item.StoreId;
        }

        protected override Task<bool> CreateAsync(AuthorizationHandlerContext context, Order entity) => Task.FromResult(IsAuthenticated(context));
        protected override async Task<bool> ReadAsync(AuthorizationHandlerContext context, Order entity) => IsAuthenticated(context) && (IsOwner(context, entity) || await IsStoreAdministratorAsync(context, entity));
    }
}