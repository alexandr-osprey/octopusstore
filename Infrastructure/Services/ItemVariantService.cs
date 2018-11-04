using ApplicationCore.Entities;
using ApplicationCore.Exceptions;
using ApplicationCore.Identity;
using ApplicationCore.Interfaces;
using ApplicationCore.Interfaces.Services;
using ApplicationCore.Specifications;
using Infrastructure.Data;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class ItemVariantService: Service<ItemVariant>, IItemVariantService
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

        protected override async Task ValidateCreateWithExceptionAsync(ItemVariant itemVariant)
        {
            await base.ValidateCreateWithExceptionAsync(itemVariant);
            await ValidateUpdateWithExceptionAsync(itemVariant);
            if (!await Context.ExistsBySpecAsync(Logger, new EntitySpecification<Item>(itemVariant.ItemId)))
                throw new EntityValidationException($"Item with Id {itemVariant.ItemId} does not exist. ");
        }

        protected override async Task ValidateUpdateWithExceptionAsync(ItemVariant itemVariant)
        {
            await base.ValidateUpdateWithExceptionAsync(itemVariant);
            if (string.IsNullOrWhiteSpace(itemVariant.Title))
                throw new EntityValidationException($"Incorrect title. ");
            if (itemVariant.Price <= 0)
                throw new EntityValidationException($"Price can't be zero or less. ");
        }
    }
}
