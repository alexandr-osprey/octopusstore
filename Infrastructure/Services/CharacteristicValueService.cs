using ApplicationCore.Entities;
using System.Threading.Tasks;
using ApplicationCore.Specifications;
using System.Linq;
using Infrastructure.Data;
using ApplicationCore.Interfaces;
using System.Collections.Generic;
using ApplicationCore.Identity;
using ApplicationCore.Exceptions;
using ApplicationCore.Interfaces.Services;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Infrastructure.Services
{
    public class CharacteristicValueService: Service<CharacteristicValue>, ICharacteristicValueService
    {
        protected ICharacteristicService _characteristicService { get; }

        public CharacteristicValueService(
            StoreContext context,
            IIdentityService identityService,
            ICharacteristicService characteristicService,
            IScopedParameters scopedParameters,
            IAuthorizationParameters<CharacteristicValue> authoriationParameters,
            IAppLogger<Service<CharacteristicValue>> logger)
           : base(context, identityService, scopedParameters, authoriationParameters, logger)
        {
            _characteristicService = characteristicService;
        }

        public async Task<IEnumerable<CharacteristicValue>> EnumerateByCategoryAsync(Specification<Category> spec)
        {
            var characteristics = await _characteristicService.EnumerateByCategoryAsync(spec);
            return await EnumerateAsync(new CharacteristicValueByCharacteristicIdsSpecification(from c in characteristics select c.Id));
        }

        public async Task<IEnumerable<CharacteristicValue>> EnumerateByItemAsync(Specification<Item> spec)
        {
            var characteristics = await _characteristicService.EnumerateAsync(spec);
            return await EnumerateAsync(new CharacteristicValueByCharacteristicIdsSpecification(from c in characteristics select c.Id));
        }

        protected override async Task ValidateWithExceptionAsync(EntityEntry<CharacteristicValue> entityEntry)
        {
            await base.ValidateWithExceptionAsync(entityEntry);
            var characteristicValue = entityEntry.Entity;
            if (string.IsNullOrWhiteSpace(characteristicValue.Title))
                throw new EntityValidationException("Incorrect title");
            if (IsPropertyModified(entityEntry, c => c.CharacteristicId, false) 
                && !await _сontext.ExistsBySpecAsync(_logger, new EntitySpecification<Characteristic>(characteristicValue.CharacteristicId)))
                throw new EntityValidationException("Characteristic does not exist");
        }
    }
}
