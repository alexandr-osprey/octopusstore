using ApplicationCore.Entities;
using ApplicationCore.Exceptions;
using ApplicationCore.Identity;
using ApplicationCore.Interfaces;
using ApplicationCore.Specifications;
using Infrastructure.Data;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class ItemVariantService : Service<ItemVariant>, IItemVariantService
    {
        public ItemVariantService(
            StoreContext context,
            IIdentityService identityService,
            IScopedParameters scopedParameters,
            IAuthorizationParameters<ItemVariant> authoriationParameters,
            IAppLogger<Service<ItemVariant>> logger)
           : base(context, identityService, scopedParameters, authoriationParameters, logger)
        {
        }

        override public async Task<int> DeleteAsync(Specification<ItemVariant> spec)
        {
            spec.AddInclude((i => i.ItemVariantCharacteristicValues));
            spec.Description += " include ItemVariantCharacteristicValues";
            return await base.DeleteAsync(spec);
        }

        protected override async Task ValidateCreateWithExceptionAsync(ItemVariant itemVariant)
        {
            if (!await _context.ExistsBySpecAsync(_logger, new EntitySpecification<Item>(itemVariant.ItemId)))
                throw new EntityValidationException($"Item with Id {itemVariant.ItemId} does not exist. ");
            if (itemVariant.Price <= 0)
                throw new EntityValidationException($"Price can't be zero or less. ");
            await base.ValidateCreateWithExceptionAsync(itemVariant);
        }
    }
}
