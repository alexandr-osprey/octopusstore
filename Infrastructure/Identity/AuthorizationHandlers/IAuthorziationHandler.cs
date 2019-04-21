namespace Infrastructure.Identity.AuthorizationHandlers
{
    /// <summary>
    /// Mainly used to denote authorization handler class when logging
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IAuthorziationHandler<T> where T: class
    {
    }
}
