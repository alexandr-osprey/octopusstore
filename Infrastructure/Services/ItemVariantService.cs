using ApplicationCore.Entities;
using ApplicationCore.Exceptions;
using ApplicationCore.Identity;
using ApplicationCore.Interfaces;
using ApplicationCore.Interfaces.Services;
using ApplicationCore.Specifications;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore.ChangeTracking;
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
            var itemVariantImages = await Context.EnumerateRelatedEnumAsync(Logger, new EntitySpecification<ItemVariant>(id), b => b.Images);
            foreach (var image in itemVariantImages)
                image.RelatedId = idToRelinkTo;
            await Context.SaveChangesAsync(Logger, "Relink ItemVariant");
        }

        protected override async Task ValidateWithExceptionAsync(EntityEntry<ItemVariant> entry)
        {
            await base.ValidateWithExceptionAsync(entry);
            var itemVariant = entry.Entity;
            if (string.IsNullOrWhiteSpace(itemVariant.Title))
                throw new EntityValidationException($"Incorrect title. ");
            if (itemVariant.Price <= 0)
                throw new EntityValidationException($"Price can't be zero or less. ");
            var entityEntry = Context.Entry(itemVariant);
            if (IsPropertyModified(entry, v => v.ItemId, false) 
                && !await Context.ExistsBySpecAsync(Logger, new EntitySpecification<Item>(entry.Entity.ItemId)))
                throw new EntityValidationException($"Item with Id {entry.Entity.ItemId} does not exist. ");
        }
    }
}
