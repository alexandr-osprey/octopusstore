using System;

namespace ApplicationCore.Exceptions
{
    /// <summary>
    /// Exception used when provided entity does not fulfil requirements, therefore request could not be executed
    /// </summary>
    public class EntityValidationException : Exception
    {
        public EntityValidationException()
        {
        }
        public EntityValidationException(string message)
            : base(message)
        {
        }
        public EntityValidationException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
