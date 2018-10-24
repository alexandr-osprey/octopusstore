using ApplicationCore.Entities;
using System.Threading.Tasks;
using ApplicationCore.Specifications;
using System.Linq;
using Infrastructure.Data;
using ApplicationCore.Interfaces;
using System.Collections.Generic;
using ApplicationCore.Identity;

namespace Infrastructure.Services
{
    public class CharacteristicValueService : Service<CharacteristicValue>, ICharacteristicValueService
    {
        protected ICharacteristicService _сharacteristicService;

        public CharacteristicValueService(
            StoreContext context,
            IIdentityService identityService,
            ICharacteristicService characteristicService,
            IScopedParameters scopedParameters,
            IAuthoriationParameters<CharacteristicValue> authoriationParameters,
            IAppLogger<Service<CharacteristicValue>> logger)
            : base(context, identityService, scopedParameters, authoriationParameters, logger)
        {
            _сharacteristicService = characteristicService;
        }

        public async Task<IEnumerable<CharacteristicValue>> EnumerateByCategoryAsync(Specification<Category> spec)
        {
            var characteristics = await _сharacteristicService.EnumerateByCategoryAsync(spec);
            return await EnumerateAsync(new CharacteristicValueByCharacteristicIdsSpecification(from c in characteristics select c.Id));
        }
        public async Task<IEnumerable<CharacteristicValue>> EnumerateByItemAsync(Specification<Item> spec)
        {
            var characteristics = await _сharacteristicService.EnumerateAsync(spec);
            return await EnumerateAsync(new CharacteristicValueByCharacteristicIdsSpecification(from c in characteristics select c.Id));
        }
    }
}
