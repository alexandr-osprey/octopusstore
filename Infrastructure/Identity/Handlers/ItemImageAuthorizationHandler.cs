using System.Threading.Tasks;
using ApplicationCore.Entities;
using ApplicationCore.Interfaces;
using ApplicationCore.Specifications;
using Infrastructure.Data;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Identity
{
    public class ItemImageAuthorizationHandler: StoreEntityAuthorizationHandler<ItemImage>
    {
        public ItemImageAuthorizationHandler(UserManager<ApplicationUser> userManager, StoreContext storeContext, IAppLogger<IAuthorziationHandler<ItemImage>> appLogger)
           : base(userManager, storeContext, appLogger)
        {
        }

        protected override async Task<int> GetStoreIdAsync(ItemImage itemImage)
        {
            var item = await _storeContext.ReadSingleBySpecAsync(_logger, new EntitySpecification<Item>(itemImage.RelatedId), true);
            return item.StoreId;
        }
    }
}
