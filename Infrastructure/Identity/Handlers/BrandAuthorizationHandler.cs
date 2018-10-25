using ApplicationCore.Entities;
using ApplicationCore.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Identity
{
    public class BrandAuthorizationHandler: DefaultAuthorizationHandler<Brand>
    {
        public BrandAuthorizationHandler(UserManager<ApplicationUser> userManager, IAppLogger<IAuthorziationHandler<Brand>> appLogger)
           : base(userManager, appLogger)
        {
        }
    }
}
