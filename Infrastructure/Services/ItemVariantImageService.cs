using ApplicationCore.Entities;
using Infrastructure.Data;
using ApplicationCore.Interfaces;
using ApplicationCore.Identity;
using ApplicationCore.Interfaces.Services;

namespace Infrastructure.Services
{
    public class ItemVariantImageService: ImageService<ItemVariantImage, ItemVariant>, IItemVariantImageService
    {
        public ItemVariantImageService(
            StoreContext context,
            IIdentityService identityService,
            IScopedParameters scopedParameters,
            IAuthorizationParameters<ItemVariantImage> authoriationParameters,
            IAppLogger<Service<ItemVariantImage>> logger)
           : base(context, identityService, scopedParameters, authoriationParameters, logger)
        {
        }
    }
}
