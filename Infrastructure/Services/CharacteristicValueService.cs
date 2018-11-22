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

namespace Infrastructure.Services
{
    public class CharacteristicValueService: Service<CharacteristicValue>, ICharacteristicValueService
    {
        protected ICharacteristicService СharacteristicService { get; }

        public CharacteristicValueService(
            StoreContext context,
            IIdentityService identityService,
            ICharacteristicService characteristicService,
            IScopedParameters scopedParameters,
            IAuthorizationParameters<CharacteristicValue> authoriationParameters,
            IAppLogger<Service<CharacteristicValue>> logger)
           : base(context, identityService, scopedParameters, authoriationParameters, logger)
        {
            this.СharacteristicService = characteristicService;
        }

        public async Task<IEnumerable<CharacteristicValue>> EnumerateByCategoryAsync(Specification<Category> spec)
        {
            var characteristics = await СharacteristicService.EnumerateByCategoryAsync(spec);
            return await EnumerateAsync(new CharacteristicValueByCharacteristicIdsSpecification(from c in characteristics select c.Id));
        }

        public async Task<IEnumerable<CharacteristicValue>> EnumerateByItemAsync(Specification<Item> spec)
        {
            var characteristics = await СharacteristicService.EnumerateAsync(spec);
            return await EnumerateAsync(new CharacteristicValueByCharacteristicIdsSpecification(from c in characteristics select c.Id));
        }

        public override async Task RelinkRelatedAsync(int id, int idToRelinkTo)
        {
            var characteristicValues = await Context.EnumerateRelatedEnumAsync(Logger, new EntitySpecification<CharacteristicValue>(id), b => b.ItemProperties);
            foreach (var value in characteristicValues)
                value.CharacteristicValueId = idToRelinkTo;
            await Context.SaveChangesAsync(Logger, "Relink CharacteristicValue");
        }

        protected override async Task ValidateCreateWithExceptionAsync(CharacteristicValue characteristicValue)
        {
            await base.ValidateCreateWithExceptionAsync(characteristicValue);
            await ValidateUpdateWithExceptionAsync(characteristicValue);
            if (!await Context.ExistsBySpecAsync(Logger, new EntitySpecification<Characteristic>(characteristicValue.CharacteristicId)))
                throw new EntityValidationException("Characteristic does not exist");
        }

        protected override async Task ValidateUpdateWithExceptionAsync(CharacteristicValue characteristicValue)
        {
            await base.ValidateUpdateWithExceptionAsync(characteristicValue);
            if (string.IsNullOrWhiteSpace(characteristicValue.Title))
                throw new EntityValidationException("Incorrect title");
        }
    }
}
