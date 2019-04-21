using ApplicationCore.Entities;
using ApplicationCore.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Identity.AuthorizationHandlers
{
    public class CharacteristicValueAuthorizationHandler: DefaultAuthorizationHandler<CharacteristicValue>
    {
        public CharacteristicValueAuthorizationHandler(UserManager<ApplicationUser> userManager, IAppLogger<IAuthorziationHandler<CharacteristicValue>> appLogger)
           : base(userManager, appLogger)
        {
        }
    }
}
