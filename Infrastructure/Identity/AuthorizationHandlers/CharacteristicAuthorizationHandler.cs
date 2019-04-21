using ApplicationCore.Entities;
using ApplicationCore.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Identity.AuthorizationHandlers
{
    public class CharacteristicAuthorizationHandler: DefaultAuthorizationHandler<Characteristic>
    {
        public CharacteristicAuthorizationHandler(UserManager<ApplicationUser> userManager, IAppLogger<IAuthorziationHandler<Characteristic>> appLogger): base(userManager, appLogger)
        {
        }
    }
}
