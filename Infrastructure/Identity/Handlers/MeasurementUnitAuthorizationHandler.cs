using ApplicationCore.Entities;
using ApplicationCore.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Identity
{
    public class MeasurementUnitAuthorizationHandler : DefaultAuthorizationHandler<MeasurementUnit>
    {
        public MeasurementUnitAuthorizationHandler(UserManager<ApplicationUser> userManager, IAppLogger<IAuthorziationHandler<MeasurementUnit>> appLogger)
            : base(userManager, appLogger)
        {
        }
    }
}
