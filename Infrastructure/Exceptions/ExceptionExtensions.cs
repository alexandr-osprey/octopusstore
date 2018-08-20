using ApplicationCore.Entities;
using ApplicationCore.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;

namespace Infrastructure.Exceptions
{
    public static class ExceptionExtensions
    {
        public static DbUpdateConcurrencyException LogAndGetDbException<T, R>(this DbUpdateConcurrencyException concurrencyUpdateException, T entity, IAppLogger<R> logger)  where T : Entity
        {
            string message = $"Concurrency error updating entity: {entity.ToString()}";
            logger.Warn(concurrencyUpdateException, message);
            concurrencyUpdateException.Data.Add("context", message);
            return concurrencyUpdateException;
        }
        public static DbUpdateException LogAndGetDbException<T, R>(this DbUpdateException updateException, T entity, IAppLogger<R> logger) where T : Entity
        {
            string message = $"Error updating entity: {entity.ToString()}";
            logger.Warn(updateException, message);
            updateException.Data.Add("context", message);
            return updateException;
        }
        public static Exception LogAndGetDbException<R>(this Exception dbException, IAppLogger<R> logger, string context = "")
        {
            string message = "Error working with database";
            logger.Warn(dbException, message);
            return dbException;
        }
    }
}
