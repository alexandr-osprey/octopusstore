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

        public override async Task RelinkRelatedAsync(int id, int idToRelinkTo)
        {
            var itemProperties = await Context.EnumerateRelatedEnumAsync(Logger, new EntitySpecification<ItemVariant>(id), b => b.ItemProperties);
            foreach (var property in itemProperties)
                property.ItemVariantId = idToRelinkTo;
            await Context.SaveChangesAsync(Logger, "Relink ItemVariant");
        }

        protected override async Task FullValidationWithExceptionAsync(ItemVariant itemVariant)
        {
            await base.FullValidationWithExceptionAsync(itemVariant);
            await PartialValidationWithExceptionAsync(itemVariant);
            if (!await Context.ExistsBySpecAsync(Logger, new EntitySpecification<Item>(itemVariant.ItemId)))
                throw new EntityValidationException($"Item with Id {itemVariant.ItemId} does not exist. ");
        }

        protected override async Task PartialValidationWithExceptionAsync(ItemVariant itemVariant)
        {
            await base.PartialValidationWithExceptionAsync(itemVariant);
            if (string.IsNullOrWhiteSpace(itemVariant.Title))
                throw new EntityValidationException($"Incorrect title. ");
            if (itemVariant.Price <= 0)
                throw new EntityValidationException($"Price can't be zero or less. ");
        }
    }
}
