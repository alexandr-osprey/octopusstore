namespace ApplicationCore.Identity
{
    /// <summary>
    /// Refresh JSON token used to renew token. Associated with a user.
    /// </summary>
    public class RefreshToken
    {
        public string Token { get; set; }
        public string OwnerId { get; set; }

        public bool Equals(RefreshToken other) => null != other && (Token == other.Token);
        public override bool Equals(object obj) => Equals(obj as RefreshToken);
        public override int GetHashCode() => Token.GetHashCode();
    }
}
