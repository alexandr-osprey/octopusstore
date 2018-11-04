using ApplicationCore.Exceptions;
using ApplicationCore.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;

namespace Infrastructure.Exceptions
{
    /// <summary>
    /// Extension methods for logging and throwing more specific Exceptions
    /// </summary>
    public static class ExceptionExtensions
    {
        public static CustomDbException LogAndGetDbException<TService>(this Exception exception, IAppLogger<TService> logger, string context)
        {
            string message = $"Error working with database. Context: {context}. ";
            return LogAndGetCustomDbException(exception, logger, message);
        }

        private static CustomDbException LogAndGetCustomDbException<TService>(this Exception dbException, IAppLogger<TService> logger, string message)
        {
            logger.Warn(dbException, message);
            return new CustomDbException(message, dbException);
        }
    }
}
