using System.Threading.Tasks;
using ApplicationCore.Entities;
using ApplicationCore.Exceptions;
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

        protected override async Task ValidateCreateWithExceptionAsync(MeasurementUnit measurementUnit)
        {
            await base.ValidateCreateWithExceptionAsync(measurementUnit);
            await ValidateUpdateWithExceptionAsync(measurementUnit);
        }

        protected override async Task ValidateUpdateWithExceptionAsync(MeasurementUnit measurementUnit)
        {
            await base.ValidateUpdateWithExceptionAsync(measurementUnit);
            if (string.IsNullOrWhiteSpace(measurementUnit.Title))
                throw new EntityValidationException("Incorrect title");
        }
    }
}
