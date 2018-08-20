using ApplicationCore.Entities;
using ApplicationCore.Interfaces;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class ItemVariantService 
        : Service<ItemVariant>, 
        IItemVariantService
    {

        public ItemVariantService(
            IAsyncRepository<ItemVariant> repository,
            IAppLogger<Service<ItemVariant>> logger)
            : base(repository, logger)
        {  }
        
        override public async Task DeleteAsync(ISpecification<ItemVariant> spec)
        {
            spec.AddInclude((i => i.ItemVariantCharacteristicValues));
            await base.DeleteAsync(spec);
        }
    }
}
