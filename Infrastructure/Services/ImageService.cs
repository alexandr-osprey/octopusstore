using ApplicationCore.Entities;
using ApplicationCore.Interfaces;

namespace Infrastructure.Services
{
    public abstract class ImageService<TFileDetails, TEntity> 
        : FileDetailsService<TFileDetails, TEntity>, 
        IImageService<TFileDetails, TEntity>
        where TFileDetails : Image<TEntity> 
        where TEntity : Entity
    {
        public ImageService(
            IAsyncRepository<TFileDetails> repository,
            IAppLogger<Service<TFileDetails>> logger)
            : base(repository, logger)
        {  }
    }
}
