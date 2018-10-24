using System;

namespace ApplicationCore.Exceptions
{
    /// <summary>
    /// Exception used when user authenticated, but not authorized to execute request
    /// </summary>
    public class AuthorizationException : Exception
    {
        public AuthorizationException()
        {
        }
        public AuthorizationException(string message)
            : base(message)
        {
        }
        public AuthorizationException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}