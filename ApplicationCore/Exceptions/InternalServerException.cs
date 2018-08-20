using System;

namespace ApplicationCore.Exceptions
{
    public class InternalServerException : Exception
    {
        public InternalServerException()
        { }
        public InternalServerException(string message)
            : base(message)
        { }
        public InternalServerException(string message, Exception inner)
            : base(message, inner)
        { }
    }
}
