using ApplicationCore.Entities;

namespace Infrastructure.Identity
{
    public class CartItemAuthorizationParameters: AuthorizationParameters<CartItem>
    {
        public override bool ReadAuthorizationRequired { get; set; } = true;
    }
}
