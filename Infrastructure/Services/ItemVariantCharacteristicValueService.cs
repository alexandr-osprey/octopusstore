using ApplicationCore.Entities;
using ApplicationCore.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class ItemVariantCharacteristicValueService 
        : Service<ItemVariantCharacteristicValue>, 
        IItemVariantCharacteristicValueService
    {
        private readonly IAsyncRepository<ItemVariant> _itemVariantRepository;
        public ItemVariantCharacteristicValueService(
            IAsyncRepository<ItemVariantCharacteristicValue> repository,
            IAsyncRepository<ItemVariant> itemVariantRepository,
            IAppLogger<Service<ItemVariantCharacteristicValue>> logger)
            : base(repository, logger)
        {
            _itemVariantRepository = itemVariantRepository;
        }

        public async Task<IEnumerable<ItemVariantCharacteristicValue>> ListByItemVariantAsync(
            ISpecification<ItemVariant> itemVariantSpec)
        {
            var itemVariants = await _itemVariantRepository.ListRelatedEnumAsync(itemVariantSpec, (v => v.ItemVariantCharacteristicValues));
            return itemVariants;
        }
    }
}
