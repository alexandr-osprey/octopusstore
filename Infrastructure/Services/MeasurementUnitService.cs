using ApplicationCore.Entities;
using ApplicationCore.Interfaces;

namespace Infrastructure.Services
{
    public class MeasurementUnitService 
        : Service<MeasurementUnit>, 
        IMeasurementUnitService
    {
        public MeasurementUnitService(
            IAsyncRepository<MeasurementUnit> repository,
            IAppLogger<Service<MeasurementUnit>> logger)
            : base(repository, logger)
        {  }
    }
}
