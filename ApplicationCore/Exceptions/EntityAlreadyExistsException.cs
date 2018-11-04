using System;

namespace ApplicationCore.Exceptions
{
    /// <summary>
    /// Exception used when entity being added to database already exists, therefore request could not be executed
    /// </summary>
    public class EntityAlreadyExistsException: Exception
    {
        public EntityAlreadyExistsException()
        {
        }

        public EntityAlreadyExistsException(string message): base(message)
        {
        }

        public EntityAlreadyExistsException(string message, Exception inner): base(message, inner)
        {
        }
    }
}
