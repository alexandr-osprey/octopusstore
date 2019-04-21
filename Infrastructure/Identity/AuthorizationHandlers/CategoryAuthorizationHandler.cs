using ApplicationCore.Entities;
using ApplicationCore.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Identity.AuthorizationHandlers
{
    public class CategoryAuthorizationHandler: DefaultAuthorizationHandler<Category>
    {
        public CategoryAuthorizationHandler(UserManager<ApplicationUser> userManager, IAppLogger<IAuthorziationHandler<Category>> appLogger): base(userManager, appLogger)
        {
        }
    }
}
