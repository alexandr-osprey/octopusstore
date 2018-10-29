using ApplicationCore.Entities;
using ApplicationCore.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Identity
{
    public class CartItemAuthorizationHandler : DefaultAuthorizationHandler<CartItem>
    {
        public CartItemAuthorizationHandler(UserManager<ApplicationUser> userManager, IAppLogger<IAuthorziationHandler<CartItem>> appLogger): base(userManager, appLogger)
        {
        }
        //override defaults
    }
}
