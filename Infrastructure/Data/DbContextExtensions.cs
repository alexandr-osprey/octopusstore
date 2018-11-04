using ApplicationCore.Exceptions;
using ApplicationCore.Interfaces;
using ApplicationCore.Specifications;
using Infrastructure.Exceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Infrastructure.Data
{
    /// <summary>
    /// Extension methods containing typical operation on database
    /// </summary>
    public static class DbContextExtensions
    {
        /// <summary>
        /// Retrieves a single entity based on specification asynchronously. May throw exception if a requested entity does not exist.
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <typeparam name="TService"></typeparam>
        /// <param name="context"></param>
        /// <param name="logger"></param>
        /// <param name="spec"></param>
        /// <param name="checkExistence"></param>
        /// <returns></returns>
        public static async Task<TEntity> ReadSingleBySpecAsync<TEntity, TService>(this DbContext context, IAppLogger<TService> logger, Specification<TEntity> spec, bool checkExistence = true) where TEntity: class
        {
            if (spec == null)
                throw new ArgumentNullException(nameof(spec));
            TEntity entity = null;
            try
            {
                entity = await context.GetQueryBySpecWithIncludes(spec).FirstOrDefaultAsync();
            }
            catch (Exception readException)
            {
                throw readException.LogAndGetDbReadException(logger, $"Function: {nameof(ReadSingleAsync)}, {nameof(spec)}: {spec}");
            }
            if (checkExistence && entity == null)
                throw new EntityNotFoundException($"{typeof(TEntity).Name} with spec {spec} not found");
            return entity;
        }
        /// <summary>
        /// Retrieves a single entity based on specification synchronously. May throw exception if a requested entity does not exist.
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <typeparam name="TService"></typeparam>
        /// <param name="context"></param>
        /// <param name="logger"></param>
        /// <param name="spec"></param>
        /// <param name="checkExistence"></param>
        /// <returns></returns>
        public static TEntity ReadSingleBySpec<TEntity, TService>(this DbContext context, IAppLogger<TService> logger, Specification<TEntity> spec, bool checkExistence = true) where TEntity : class
        {
            if (spec == null)
                throw new ArgumentNullException(nameof(spec));
            TEntity entity = null;
            try
            {
                entity = context.GetQueryBySpecWithIncludes(spec).FirstOrDefault();
            }
            catch (Exception readException)
            {
                throw readException.LogAndGetDbReadException(logger, $"Function: {nameof(ReadSingleAsync)}, {nameof(spec)}: {spec}");
            }
            if (checkExistence && entity == null)
                throw new EntityNotFoundException($"{typeof(TEntity).Name} with spec {spec} not found");
            return entity;
        }
        /// <summary>
        /// Adds a single entity into database. May check existence before adding and throw an exception if it already exists.
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <typeparam name="TService"></typeparam>
        /// <param name="context"></param>
        /// <param name="logger"></param>
        /// <param name="entityToAdd"></param>
        /// <param name="checkExistence"></param>
        /// <returns></returns>
        public static async Task<TEntity> CreateAsync<TEntity, TService>(this DbContext context, IAppLogger<TService> logger, TEntity entityToAdd, bool checkExistence = true) where TEntity: class
        {
            TEntity created = null;
            if (entityToAdd == null)
                throw new ArgumentNullException(nameof(entityToAdd));
            if (checkExistence && await context.ExistsAsync(logger, entityToAdd))
                throw new EntityAlreadyExistsException($"{typeof(TEntity).Name} {entityToAdd} already exists");
            try
            {
                created = context.Set<TEntity>().Add(entityToAdd).Entity;
                await context.SaveChangesAsync();
            }
            catch (DbUpdateException updateException)
            {
                throw updateException.LogAndGetDbCreateException(entityToAdd, logger, nameof(CreateAsync));
            }
            catch (Exception exception)
            {
                throw exception.LogAndGetDbException(logger, $"Function: {nameof(CreateAsync)}, entity: {entityToAdd}");
            }
            return created;
        }

        /// <summary>
        /// Retrives a single entity by multiple keys. May throw exception if entity not found.
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <typeparam name="TService"></typeparam>
        /// <param name="context"></param>
        /// <param name="logger"></param>
        /// <param name="checkExistence"></param>
        /// <param name="keys"></param>
        /// <returns></returns>
        public static async Task<TEntity> ReadByKeysAsync<TEntity, TService>(this DbContext context, IAppLogger<TService> logger, bool checkExistence = true, params object[] keys) where TEntity: class
        {
            if (keys == null || keys.Length == 0)
                throw new ArgumentException(nameof(keys));
            string keysRepresentaion = string.Join(", ", keys);
            TEntity entity = null;
            try
            {
                entity = await context.Set<TEntity>().FindAsync(keys);
                context.Entry(entity).State = EntityState.Detached;
            }
            catch (Exception readException)
            {
                throw readException.LogAndGetDbReadException(logger, $"Function: {nameof(ReadByKeysAsync)}, keys: {keysRepresentaion}");
            }
            if (checkExistence && entity == null)
                throw new EntityNotFoundException($"{typeof(TEntity).Name} with key(s) {keysRepresentaion} not found");
            return entity;
        }
        /// <summary>
        /// Retrives a single entity by a key. May throw exception if entity not found.
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <typeparam name="TService"></typeparam>
        /// <param name="context"></param>
        /// <param name="logger"></param>
        /// <param name="key"></param>
        /// <param name="checkExistence"></param>
        /// <returns></returns>
        public static async Task<TEntity> ReadByKeyAsync<TEntity, TService>(this DbContext context, IAppLogger<TService> logger, object key, bool checkExistence = true) where TEntity: class
        {
            return await context.ReadByKeysAsync<TEntity, TService>(logger, checkExistence, key);
        }
        /// <summary>
        /// Retrieves a single entity based on a passed one. Requires overriding of Equals and GetHashCode. May throw exception if entity not found.
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <typeparam name="TService"></typeparam>
        /// <param name="context"></param>
        /// <param name="logger"></param>
        /// <param name="entity"></param>
        /// <param name="checkExistence"></param>
        /// <returns></returns>
        public static async Task<TEntity> ReadSingleAsync<TEntity, TService>(this DbContext context, IAppLogger<TService> logger, TEntity entity, bool checkExistence = true) where TEntity: class
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));
            TEntity retrieved = null;
            try
            {
                retrieved = await context.Set<TEntity>().AsNoTracking().Where(e => e == entity).FirstOrDefaultAsync();
            }
            catch (Exception readException)
            {
                throw readException.LogAndGetDbReadException(logger, $"Function: {nameof(ReadSingleAsync)}, {nameof(entity)}: {entity}");
            }
            if (checkExistence && retrieved == null)
                throw new EntityNotFoundException($"{typeof(TEntity).Name} {entity} not found");
            return retrieved;
        }
        /// <summary>
        /// Updates a single entity. May throw exception if entity not found.
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <typeparam name="TService"></typeparam>
        /// <param name="context"></param>
        /// <param name="logger"></param>
        /// <param name="entityToUpdate"></param>
        /// <param name="checkExistence"></param>
        /// <returns></returns>
        public static async Task<TEntity> UpdateAsync<TEntity, TService>(this DbContext context, IAppLogger<TService> logger, TEntity entityToUpdate, bool checkExistence = true) where TEntity: class
        {
            if (entityToUpdate == null)
                throw new ArgumentNullException(nameof(entityToUpdate));
            if (checkExistence && !await context.ExistsAsync(logger, entityToUpdate))
                throw new EntityNotFoundException($"{typeof(TEntity).Name} {entityToUpdate} does not exist");
            try
            {
                context.Entry(entityToUpdate).State = EntityState.Modified;
                await context.SaveChangesAsync();
                context.Entry(entityToUpdate).State = EntityState.Detached;
                return entityToUpdate;
            }
            catch (DbUpdateConcurrencyException concurrencyUpdateException)
            {
                throw concurrencyUpdateException.LogAndGetDbUpdateConcurrencyException(entityToUpdate, logger, $"Function: {nameof(UpdateAsync)}");
            }
            catch (DbUpdateException updateException)
            {
                throw updateException.LogAndGetDbUpdateException(entityToUpdate, logger, $"Function: {nameof(UpdateAsync)}");
            }
            catch (Exception exception)
            {
                throw exception.LogAndGetDbException(logger, $"Function: {nameof(UpdateAsync)}, entity: {entityToUpdate}");
            }
        }

        public static async Task DeleteAsync<TEntity, TService>(this DbContext context, IAppLogger<TService> logger, TEntity entityToDelete, bool checkExistence = true) where TEntity: class
        {
            if (entityToDelete == null)
                throw new ArgumentNullException(nameof(entityToDelete));
            if (checkExistence && !await context.ExistsAsync(logger, entityToDelete))
                throw new EntityNotFoundException($"{typeof(TEntity).Name} {entityToDelete} does not exist");
            try
            {
                context.Set<TEntity>().Remove(entityToDelete);
                await context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException concurrencyUpdateException)
            {
                throw concurrencyUpdateException.LogAndGetDbUpdateConcurrencyException(entityToDelete, logger, $"Function: {nameof(DeleteAsync)}");
            }
            catch (DbUpdateException updateException)
            {
                throw updateException.LogAndGetDbUpdateException(entityToDelete, logger, $"Function: {nameof(DeleteAsync)}");
            }
        }
        public static async Task<int> DeleteAsync<TEntity, TService>(this DbContext context, IAppLogger<TService> logger, Specification<TEntity> spec) where TEntity: class
        {
            if (spec == null)
                throw new ArgumentNullException(nameof(spec));
            try
            {
                var entitiesToDelete = await context.EnumerateAsync(logger, spec);
                context.Set<TEntity>().RemoveRange(entitiesToDelete);
                await context.SaveChangesAsync();
                return entitiesToDelete.Count();
            }
            catch (Exception exception)
            {
                throw exception.LogAndGetDbException(logger, $"Function: {nameof(DeleteAsync)} error deleting according to spec {spec}");
            }
        }

        public static async Task<int> CountAsync<TEntity, TService>(this DbContext context, IAppLogger<TService> logger, Specification<TEntity> countSpec) where TEntity: class
        {
            if (countSpec == null)
                throw new ArgumentNullException(nameof(countSpec));
            try
            {
                return await context.GetQueryBySpecWithIncludes(countSpec).CountAsync();
            }
            catch (Exception readException)
            {
                throw readException.LogAndGetDbReadException(logger, $"Function: {nameof(CountAsync)}, {nameof(countSpec)}: {countSpec}");
            }
        }
        public static async Task<bool> ExistsBySpecAsync<TEntity, TService>(this DbContext context, IAppLogger<TService> logger, Specification<TEntity> specToCheck, bool throwNotFound = false) where TEntity: class
        {
            if (specToCheck == null)
                throw new ArgumentNullException(nameof(specToCheck));
            bool exists = false;
            try
            {
                exists = await context.GetQueryBySpecWithIncludes(specToCheck).AnyAsync();
            }
            catch (Exception readException)
            {
                throw readException.LogAndGetDbReadException(logger, $"Function: {nameof(ExistsAsync)}, {nameof(specToCheck)}: {specToCheck}");
            }
            if (!exists && throwNotFound)
                throw new EntityNotFoundException($"{typeof(TEntity).Name} with spec {specToCheck} not found");
            return exists;
        }
        public static IQueryable<T> NoTrackingSet<T>(this DbContext context) where T: class
        {
            return context.Set<T>().AsNoTracking();
        }
        public static async Task<bool> ExistsAsync<TEntity, TService>(this DbContext context, IAppLogger<TService> logger, TEntity entityToCheck, bool throwNotFound = false) where TEntity: class
        {
            if (entityToCheck == null)
                throw new ArgumentNullException(nameof(entityToCheck));
            bool exists = false;
            try
            {
                exists = await context.Set<TEntity>().AnyAsync(e => e.GetHashCode() == entityToCheck.GetHashCode());
            }
            catch (Exception readException)
            {
                throw readException.LogAndGetDbReadException(logger, $"Function: {nameof(ExistsAsync)}, {nameof(entityToCheck)}: {entityToCheck}");
            }
            if (!exists && throwNotFound)
                throw new EntityNotFoundException($"{typeof(TEntity).Name} {entityToCheck} not found");
            return exists;
        }

        public static async Task<IEnumerable<TEntity>> EnumerateAllAsync<TEntity, TService>(this DbContext context, IAppLogger<TService> logger) where TEntity: class
        {
            try
            {
                var all = context.Set<TEntity>();
                await all.LoadAsync();
                return all;
            }
            catch (Exception readException)
            {
                throw readException.LogAndGetDbReadException(logger, $"Function: {nameof(EnumerateAllAsync)}");
            }
        }

        public static async Task<IEnumerable<TEntity>> EnumerateAsync<TEntity, TService>(this DbContext context, IAppLogger<TService> logger, Specification<TEntity> listSpec) where TEntity: class
        {
            if (listSpec == null)
                throw new ArgumentNullException(nameof(listSpec));
            try
            {
                var entities = await context.GetQueryBySpecSkipAndTake(listSpec).ToListAsync();
                //await entities.LoadAsync();
                return entities;
            }
            catch (Exception readException)
            {
                throw readException.LogAndGetDbReadException(logger, $"Function: {nameof(EnumerateAsync)}, {nameof(listSpec)}: {listSpec}");
            }
        }

        public static async Task<IEnumerable<TRelated>> EnumerateRelatedAsync<TEntity, TRelated, TService>(
            this DbContext context, IAppLogger<TService> logger,
            Specification<TEntity> listRelatedSpec,
            Expression<Func<TEntity, TRelated>> relatedSelect) where TEntity: class where TRelated: class
        {
            if (listRelatedSpec == null)
                throw new ArgumentNullException(nameof(listRelatedSpec));
            if (relatedSelect == null)
                throw new ArgumentNullException(nameof(relatedSelect));
            try
            {
                var relatedEntities = await context.GetQueryBySpecWithIncludes(listRelatedSpec).Select(relatedSelect).Distinct().ToListAsync();
                //await relatedEntities.LoadAsync();
                return relatedEntities;
            }
            catch (Exception readException)
            {
                throw readException.LogAndGetDbReadException(logger, $"Function: {nameof(EnumerateRelatedAsync)}, {nameof(listRelatedSpec)}: {listRelatedSpec}, {nameof(relatedSelect)}: {relatedSelect}");
            }
        }

        public static async Task<IEnumerable<TRelated>> EnumerateRelatedEnumAsync<TEntity, TRelated, TService>(
            this DbContext context, IAppLogger<TService> logger,
            Specification<TEntity> listRelatedSpec,
            Expression<Func<TEntity, IEnumerable<TRelated>>> relatedEnumSelect) where TEntity: class where TRelated: class
        {
            if (listRelatedSpec == null)
                throw new ArgumentNullException(nameof(listRelatedSpec));
            try
            {
                var  relatedEntities = await context.GetQueryBySpecWithIncludes(listRelatedSpec)
                    .SelectMany(relatedEnumSelect)
                    .Distinct().ToListAsync();
                //await relatedEntities.LoadAsync();
                return relatedEntities;
            }
            catch (Exception readException)
            {
                throw readException.LogAndGetDbReadException(logger, $"Function: {nameof(EnumerateRelatedEnumAsync)}, {nameof(listRelatedSpec)}: {listRelatedSpec}, {nameof(relatedEnumSelect)}: {relatedEnumSelect}");
            }
        }

        public static IQueryable<T> GetQueryBySpecSkipAndTake<T>(this DbContext context, Specification<T> spec) where T: class
        {
            var result = context.GetQueryBySpecWithIncludes(spec);
            result = result.Skip(spec.Skip);
            return spec.Take > 0 ? result.Take(spec.Take): result;
        }
        public static IQueryable<T> GetQueryBySpecWithIncludes<T>(this DbContext context, Specification<T> spec) where T: class
        {
            // fetch a Queryable that includes all expression-based includes
            var queryableResultWithIncludes = spec.Includes
                .Aggregate(context.Set<T>().AsNoTracking().AsQueryable(),
                    (current, include) => current.Include(include));
            // modify the IQueryable to include any string-based include statements
            var secondaryResult = spec.IncludeStrings
                .Aggregate(queryableResultWithIncludes,
                    (current, include) => current.Include(include));
            return secondaryResult.Where(spec.Criteria);
        }

    }
}
