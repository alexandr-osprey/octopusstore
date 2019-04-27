using ApplicationCore.Entities;
using ApplicationCore.Identity;
using ApplicationCore.Interfaces;
using ApplicationCore.Interfaces.Services;
using Infrastructure.Data;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;

namespace Infrastructure.Services
{
    public abstract class ImageService<TImage, TEntity>
       : FileInfoService<TImage, TEntity>,
        IImageService<TImage, TEntity>
        where TImage : Image<TEntity>
        where TEntity : Entity
    {
        protected override IEnumerable<string> _allowedContentTypes { get; } = new List<string>() { @"image/jpeg", @"image/jpg", @"image/png" };

        public ImageService(
            StoreContext context,
            IIdentityService identityService,
            IScopedParameters scopedParameters,
            IAuthorizationParameters<TImage> authoriationParameters,
            IConfiguration configuration,
            IAppLogger<Service<TImage>> logger)
           : base(context, identityService, scopedParameters, authoriationParameters, configuration, logger)
        {
        }
    }
}
