namespace ApplicationCore.Identity
{
    /// <summary>
    /// Token and refresh token provided to the successfully authenticated user
    /// </summary>
    public class TokenPair
    {
        public string Token { get; set; }
        public string RefreshToken { get; set; }
    }
}
