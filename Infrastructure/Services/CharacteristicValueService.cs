using ApplicationCore.Entities;
using ApplicationCore.Interfaces;

namespace Infrastructure.Services
{
    public class CharacteristicValueService : Service<CharacteristicValue>, ICharacteristicValueService
    {
        public CharacteristicValueService(
            IAsyncRepository<CharacteristicValue> repository,
            IAppLogger<Service<CharacteristicValue>> logger)
            : base(repository, logger)
        {  }
    }
}
