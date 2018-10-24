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
        public static CustomDbException LogAndGetDbCreateException<TEntity, TService>(this Exception createException, TEntity entity, IAppLogger<TService> logger, string context)
        {
            string message = $"Error creating entity: {entity}. Context: {context}. ";
            return LogAndGetCustomDbException(createException, logger, message);
        }
        public static CustomDbException LogAndGetDbReadException<TService>(this Exception readException, IAppLogger<TService> logger, string context)
        {
            string message = $"Error reading entity (entities). Context: {context}. ";
            return LogAndGetCustomDbException(readException, logger, message);
        }
        public static CustomDbException LogAndGetDbUpdateException<TEntity, TService>(this DbUpdateException updateException, TEntity entity, IAppLogger<TService> logger, string context) where TEntity : class
        {
            string message = $"Error updating entity: {entity}. Context: {context}. ";
            return LogAndGetCustomDbException(updateException, logger, message);
        }
        public static CustomDbException LogAndGetDbUpdateConcurrencyException<TEntity, TService>(this DbUpdateConcurrencyException concurrencyUpdateException, TEntity entity, IAppLogger<TService> logger, string context)  where TEntity : class
        {
            string message = $"Error concurrent updating entity: {entity}. Context: {context}. ";
            return LogAndGetCustomDbException(concurrencyUpdateException, logger, message);
        }
        public static CustomDbException LogAndGetDbDeleteException<TEntity, TService>(this Exception deleteException, TEntity entity, IAppLogger<TService> logger, string context)
        {
            string message = $"Error deleting entity: {entity}. Context: {context}. ";
            return LogAndGetCustomDbException(deleteException, logger, message);
        }
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
