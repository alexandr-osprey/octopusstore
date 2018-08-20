using ApplicationCore.Entities;
using ApplicationCore.Interfaces;

namespace Infrastructure.Services
{
    public class ItemImageService 
        : ImageService<ItemImage, Item>, 
        IItemImageService
    {
        public ItemImageService(
            IAsyncRepository<ItemImage> repository,
            IAppLogger<Service<ItemImage>> logger)
            : base(repository, logger)
        {   }
    }
}
