using ApplicationCore.Entities;

namespace Infrastructure.Identity
{
    public class CartItemAuthorizationParameters: AuthorizationParameters<CartItem>
    {
        public override bool CreateAuthorizationRequired { get; set; } = true;
        public override bool ReadAuthorizationRequired { get; set; } = true;
        public override bool UpdateAuthorizationRequired { get; set; } = true;
        public override bool DeleteAuthorizationRequired { get; set; } = true;
    }
}
