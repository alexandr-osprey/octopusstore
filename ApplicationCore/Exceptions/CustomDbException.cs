using System;

namespace ApplicationCore.Exceptions
{
    /// <summary>
    /// Exception used when some error occurred when trying to query the databasae
    /// </summary>
    public class CustomDbException : Exception
    {
        public CustomDbException()
        {
        }
        public CustomDbException(string message)
            : base(message)
        {
        }
        public CustomDbException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
