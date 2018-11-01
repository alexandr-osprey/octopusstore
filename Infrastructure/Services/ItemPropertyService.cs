using ApplicationCore.Entities;
using ApplicationCore.Exceptions;
using ApplicationCore.Identity;
using ApplicationCore.Interfaces;
using ApplicationCore.Interfaces.Services;
using ApplicationCore.Specifications;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class ItemPropertyService
       : Service<ItemProperty>,
        IItemPropertyService
    {
        protected readonly IItemVariantService _itemVariantService;
        protected readonly ICharacteristicValueService _characteristicValueService;

        public ItemPropertyService(
            StoreContext context,
            IIdentityService identityService,
            ICharacteristicValueService characteristicValueService,
            IScopedParameters scopedParameters,
            IItemVariantService itemVariantService,
            IAuthorizationParameters<ItemProperty> authoriationParameters,
            IAppLogger<Service<ItemProperty>> logger)
           : base(context, identityService, scopedParameters, authoriationParameters, logger)
        {
            _itemVariantService = itemVariantService;
            _characteristicValueService = characteristicValueService;
        }

        protected override async Task ValidateCustomUniquinessWithException(ItemProperty itemProperty)
        {
            await base.ValidateCustomUniquinessWithException(itemProperty);
            var characteristicValue = await _context.ReadSingleBySpecAsync(_logger, new EntitySpecification<CharacteristicValue>(itemProperty.CharacteristicValueId));
            var itemVariantProperties = await _context.Set<ItemProperty>()
                .Where(p => p.ItemVariantId == itemProperty.ItemVariantId)
                .Include(p => p.CharacteristicValue).ToListAsync();
            if (itemVariantProperties.Select(i => i.CharacteristicValue.CharacteristicId).Contains(characteristicValue.CharacteristicId))
                throw new EntityAlreadyExistsException($"Item property with characteristic {characteristicValue.CharacteristicId} in item variant {itemProperty.ItemVariantId} already exists. ");
        }

        public async Task<IEnumerable<ItemProperty>> EnumerateByItemVariantAsync(Specification<ItemVariant> itemVariantSpec)
        {
            return await _itemVariantService.EnumerateRelatedEnumAsync(itemVariantSpec, (v => v.ItemProperties));
        }

        protected override async Task ValidateCreateWithExceptionAsync(ItemProperty itemProperty)
        {
            await base.ValidateCreateWithExceptionAsync(itemProperty);
            var itemVariant = await _context
                .NoTrackingSet<ItemVariant>()
                .Include(v => v.Item)
                .FirstOrDefaultAsync(v => v.Id == itemProperty.ItemVariantId) 
                    ?? throw new EntityValidationException($"Item variant {itemProperty.ItemVariantId} does not exist");
            var possibleCharacteristicValues = await _characteristicValueService.EnumerateByCategoryAsync(new EntitySpecification<Category>(itemVariant.Item.CategoryId));
            var characteristicValue = await _context
                .NoTrackingSet<CharacteristicValue>()
                .FirstOrDefaultAsync(v => v.Id == itemProperty.CharacteristicValueId) 
                    ?? throw new EntityValidationException($"Characteristic value {itemProperty.CharacteristicValueId} does not exist");
            if (!possibleCharacteristicValues.Contains(characteristicValue))
                throw new EntityValidationException($"Characteristic value {itemProperty.CharacteristicValueId} has the wrong category");
        }

        protected override async Task ValidateUpdateWithExceptionAsync(ItemProperty itemProperty)
        {
            await base.ValidateUpdateWithExceptionAsync(itemProperty);
            await ValidateCreateWithExceptionAsync(itemProperty);
        }
    }
}
