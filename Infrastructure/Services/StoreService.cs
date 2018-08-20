using ApplicationCore.Entities;
using ApplicationCore.Interfaces;
using ApplicationCore.Specifications;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class StoreService 
        : Service<Store>, 
        IStoreService
    {
        private readonly IItemService _itemService;

        public StoreService(
            IAsyncRepository<Store> repository, 
            IItemService itemService,
            IAppLogger<Service<Store>> logger)
            : base(repository, logger)
        {
            _itemService = itemService;
        }

        override public async Task DeleteAsync(ISpecification<Store> spec)
        {
            spec.AddInclude((s => s.Items));
            spec.Description += ", includes Items";
            await base.DeleteAsync(spec);
        }
        public override async Task DeleteRelatedEntitiesAsync(Store store)
        {
            var spec = new Specification<Item>(i => store.Items.Contains(i));
            spec.Description = $"Item with store id={store.Id}";
            await _itemService.DeleteAsync(spec);
            await base.DeleteRelatedEntitiesAsync(store);
        }
    }
}
