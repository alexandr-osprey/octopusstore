using ApplicationCore.Entities;
using Infrastructure.Data;
using ApplicationCore.Interfaces;
using ApplicationCore.Identity;
using ApplicationCore.Interfaces.Services;

namespace Infrastructure.Services
{
    public class ItemImageService: ImageService<ItemImage, Item>, IItemImageService
    {
        public ItemImageService(
            StoreContext context,
            IIdentityService identityService,
            IScopedParameters scopedParameters,
            IAuthorizationParameters<ItemImage> authoriationParameters,
            IAppLogger<Service<ItemImage>> logger)
           : base(context, identityService, scopedParameters, authoriationParameters, logger)
        {
        }
    }
}
