using System.Threading.Tasks;
using ApplicationCore.Entities;
using ApplicationCore.Interfaces;
using ApplicationCore.Specifications;
using Infrastructure.Data;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Identity.AuthorizationHandlers
{
    public class ItemVariantImageAuthorizationHandler: StoreEntityAuthorizationHandler<ItemVariantImage>
    {
        public ItemVariantImageAuthorizationHandler(UserManager<ApplicationUser> userManager, StoreContext storeContext, IAppLogger<IAuthorziationHandler<ItemVariantImage>> appLogger)
           : base(userManager, storeContext, appLogger)
        {
        }

        protected override async Task<int> GetStoreIdAsync(ItemVariantImage itemVariantImage)
        {
            var itemVariant = await _storeContext.ReadSingleBySpecAsync(_logger, 
                new EntitySpecification<ItemVariant>(i => i.Id == itemVariantImage.RelatedId, i => i.Item), true);
            return itemVariant.Item.StoreId;
        }
    }
}
