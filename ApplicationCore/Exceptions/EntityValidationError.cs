using System;

namespace ApplicationCore.Exceptions
{
    public class EntityValidationError : Exception
    {
        public EntityValidationError()
        { }
        public EntityValidationError(string message)
            : base(message)
        { }
        public EntityValidationError(string message, Exception inner)
            : base(message, inner)
        { }
    }
}
