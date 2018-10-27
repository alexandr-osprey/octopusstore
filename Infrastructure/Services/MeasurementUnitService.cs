using ApplicationCore.Entities;
using ApplicationCore.Identity;
using ApplicationCore.Interfaces;
using Infrastructure.Data;

namespace Infrastructure.Services
{
    public class MeasurementUnitService : Service<MeasurementUnit>, IMeasurementUnitService
    {
        public MeasurementUnitService(
            StoreContext context,
            IIdentityService identityService,
            IScopedParameters scopedParameters,
            IAuthorizationParameters<MeasurementUnit> authoriationParameters,
            IAppLogger<Service<MeasurementUnit>> logger)
           : base(context, identityService, scopedParameters, authoriationParameters, logger)
        {
        }
    }
}
