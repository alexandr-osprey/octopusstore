namespace ApplicationCore.Identity
{
    /// <summary>
    /// Custom claim types to assign to user
    /// </summary>
    public static class CustomClaimTypes
    {
        public const string Administrator = nameof(Administrator);
        public const string Buyer = nameof(Buyer);
        public const string Seller = nameof(Seller);
        public const string StoreAdministrator = nameof(StoreAdministrator);
    }
}
