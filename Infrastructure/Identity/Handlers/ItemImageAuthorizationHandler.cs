using System.Threading.Tasks;
using ApplicationCore.Entities;
using ApplicationCore.Interfaces;
using ApplicationCore.Specifications;
using Infrastructure.Data;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Identity
{
    public class ItemImageAuthorizationHandler : StoreEntityAuthorizationHandler<ItemImage>
    {
        public ItemImageAuthorizationHandler(UserManager<ApplicationUser> userManager, StoreContext storeContext, IAppLogger<IAuthorziationHandler<ItemImage>> appLogger)
            : base(userManager, storeContext, appLogger)
        {
        }

        protected override async Task<Store> GetStoreEntityAsync(ItemImage itemImage)
        {
            var spec = new EntitySpecification<Item>((i => i.Id == itemImage.RelatedId), (i => i.Store));
            var item = await _storeContext.ReadSingleBySpecAsync(_logger, spec, true);
            return item.Store;
        }
    }
}
