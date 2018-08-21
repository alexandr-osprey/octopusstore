using System;

namespace ApplicationCore.Exceptions
{
    public class CredentialsExeption : Exception
    {
        public CredentialsExeption()
        { }
        public CredentialsExeption(string message)
            : base(message)
        { }
        public CredentialsExeption(string message, Exception inner)
            : base(message, inner)
        { }
    }
}
