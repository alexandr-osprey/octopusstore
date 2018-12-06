using System.Threading.Tasks;
using ApplicationCore.Entities;
using ApplicationCore.Exceptions;
using ApplicationCore.Identity;
using ApplicationCore.Interfaces;
using ApplicationCore.Interfaces.Services;
using ApplicationCore.Specifications;
using Infrastructure.Data;

namespace Infrastructure.Services
{
    public class MeasurementUnitService: Service<MeasurementUnit>, IMeasurementUnitService
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

        public override async Task RelinkRelatedAsync(int id, int idToRelinkTo)
        {
            var muItems = await Context.EnumerateRelatedEnumAsync(Logger, new EntitySpecification<MeasurementUnit>(id), b => b.Items);
            foreach (var item in muItems)
                item.MeasurementUnitId = idToRelinkTo;
            await Context.SaveChangesAsync(Logger, "Relink MeasurementUnit");
        }

        protected override async Task PartialValidationWithExceptionAsync(MeasurementUnit measurementUnit)
        {
            await base.PartialValidationWithExceptionAsync(measurementUnit);
            if (string.IsNullOrWhiteSpace(measurementUnit.Title))
                throw new EntityValidationException("Incorrect title");
        }
    }
}
