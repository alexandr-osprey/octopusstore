using System;

namespace ApplicationCore.Exceptions
{
    /// <summary>
    /// Exception used when user provided wrong information and request could not be executed
    /// </summary>
    public class BadRequestException: Exception
    {
        public BadRequestException()
        {
        }

        public BadRequestException(string message): base(message)
        {
        }

        public BadRequestException(string message, Exception inner): base(message, inner)
        {
        }
    }
}
