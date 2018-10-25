using ApplicationCore.Entities;
using ApplicationCore.Identity;
using ApplicationCore.Interfaces;
using Infrastructure.Data;

namespace Infrastructure.Services
{
    public class BrandService: Service<Brand>, IBrandService
    {
        public BrandService(
            StoreContext context,
            IIdentityService identityService,
            IScopedParameters scopedParameters,
            IAuthorizationParameters<Brand> authoriationParameters,
            IAppLogger<Service<Brand>> logger)
           : base(context, identityService, scopedParameters, authoriationParameters, logger)
        {
        }
    }
}
