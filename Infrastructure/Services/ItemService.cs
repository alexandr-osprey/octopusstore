using ApplicationCore.Entities;
using ApplicationCore.Interfaces;
using ApplicationCore.Specifications;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class ItemService : Service<Item>, IItemService
    {
        private readonly IItemImageService _itemImageService;

        public ItemService(
            IAsyncRepository<Item> repository,
            IItemImageService itemImageService,
            IAppLogger<Service<Item>> logger)
            : base(repository, logger)
        {
            _itemImageService = itemImageService;
        }

        override public async Task DeleteAsync(ISpecification<Item> spec)
        {
            spec.AddInclude((i => i.Images));
            spec.AddInclude((i => i.ItemVariants));
            await base.DeleteAsync(spec);
        }
        override public async Task DeleteRelatedEntitiesAsync(Item item)
        {
            var imageDeleteSpec = new Specification<ItemImage>((i => item.Images.Contains(i)));
            imageDeleteSpec.Description = $"ItemImage with item id={item.Id}";
            await _itemImageService.DeleteAsync(imageDeleteSpec);
            //await _itemVariantService.DeleteAsync(new Specification<ItemVariant>(i => entity.ItemVariants.Contains(i)));
            await base.DeleteRelatedEntitiesAsync(item);
        }
    }
}
