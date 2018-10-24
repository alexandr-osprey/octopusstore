using ApplicationCore.Entities;
using ApplicationCore.Exceptions;
using ApplicationCore.Identity;
using ApplicationCore.Interfaces;
using ApplicationCore.Specifications;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class ItemVariantCharacteristicValueService 
        : Service<ItemVariantCharacteristicValue>,
        IItemVariantCharacteristicValueService
    {
        protected IItemVariantService _itemVariantService;
        protected ICharacteristicValueService _characteristicValueService;

        public ItemVariantCharacteristicValueService(
            StoreContext context,
            IIdentityService identityService,
            ICharacteristicValueService characteristicValueService,
            IScopedParameters scopedParameters,
            IItemVariantService itemVariantService,
            IAuthoriationParameters<ItemVariantCharacteristicValue> authoriationParameters,
            IAppLogger<Service<ItemVariantCharacteristicValue>> logger)
            : base(context, identityService, scopedParameters, authoriationParameters, logger)
        {
            _itemVariantService = itemVariantService;
            _characteristicValueService = characteristicValueService;
        }

        public async Task<IEnumerable<ItemVariantCharacteristicValue>> EnumerateByItemVariantAsync(Specification<ItemVariant> itemVariantSpec)
        {
            return await _itemVariantService.EnumerateRelatedEnumAsync(itemVariantSpec, (v => v.ItemVariantCharacteristicValues));
        }
        public override async Task ValidateCreateWithExceptionAsync(ItemVariantCharacteristicValue itemVariantCharactecteristicValue)
        {
            await base.ValidateCreateWithExceptionAsync(itemVariantCharactecteristicValue);
            var itemVariant = await _context
                .NoTrackingSet<ItemVariant>()
                .Include(v => v.Item)
                .FirstOrDefaultAsync(v => v.Id == itemVariantCharactecteristicValue.ItemVariantId);
            if (itemVariant == null)
                throw new EntityValidationException($"Item variant {itemVariantCharactecteristicValue.ItemVariantId} does not exist");
            var possibleCharacteristicValues = await _characteristicValueService.EnumerateByCategoryAsync(new EntitySpecification<Category>(itemVariant.Item.CategoryId));
            var characteristicValue = await _context
                .NoTrackingSet<CharacteristicValue>()
                .FirstOrDefaultAsync(v => v.Id == itemVariantCharactecteristicValue.CharacteristicValueId);
            if (characteristicValue == null)
                throw new EntityValidationException($"Characteristic value {itemVariantCharactecteristicValue.CharacteristicValueId} does not exist");
            if (!possibleCharacteristicValues.Contains(characteristicValue))
                throw new EntityValidationException($"Characteristic value {itemVariantCharactecteristicValue.CharacteristicValueId} has the wrong category");
        }
        public override async Task ValidateUpdateWithExceptionAsync(ItemVariantCharacteristicValue itemVariantCharactecteristicValue)
        {
            await ValidateCreateWithExceptionAsync(itemVariantCharactecteristicValue);
        }
    }
}
