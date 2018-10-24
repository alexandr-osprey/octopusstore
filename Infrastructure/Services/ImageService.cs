using ApplicationCore.Entities;
using ApplicationCore.Identity;
using ApplicationCore.Interfaces;
using Infrastructure.Data;

namespace Infrastructure.Services
{
    public abstract class ImageService<TImage, TEntity>
        : FileInfoService<TImage, TEntity>,
        IImageService<TImage, TEntity>
        where TImage : Image<TEntity>
        where TEntity : Entity
    {
        public ImageService(
            StoreContext context,
            IIdentityService identityService,
            IScopedParameters scopedParameters,
            IAuthoriationParameters<TImage> authoriationParameters,
            IAppLogger<Service<TImage>> logger)
            : base(context, identityService, scopedParameters, authoriationParameters, logger)
        {
        }
    }
}
