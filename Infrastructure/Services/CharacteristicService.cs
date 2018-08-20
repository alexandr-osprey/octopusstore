using ApplicationCore.Entities;
using ApplicationCore.Interfaces;

namespace Infrastructure.Services
{
    public class CharacteristicService : Service<Characteristic>, ICharacteristicService
    {
        public CharacteristicService(
            IAsyncRepository<Characteristic> repository,
            IAppLogger<Service<Characteristic>> logger)
            : base(repository, logger)
        {   }
    }
}
