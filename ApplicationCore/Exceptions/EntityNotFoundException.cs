using System;

namespace ApplicationCore.Exceptions
{
    /// <summary>
    /// Exception used when required entity not found in the database, therefore request could not be executed
    /// </summary>
    public class EntityNotFoundException: Exception
    {
        public EntityNotFoundException()
        {
        }
        public EntityNotFoundException(string message)
           : base(message)
        {
        }
        public EntityNotFoundException(string message, Exception inner)
           : base(message, inner)
        {
        }
    }
}
