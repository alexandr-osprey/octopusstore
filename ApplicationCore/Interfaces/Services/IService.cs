using ApplicationCore.Identity;
using ApplicationCore.Specifications;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ApplicationCore.Interfaces
{
    /// <summary>
    /// Maintains full lifecycle of entities
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public interface IService<TEntity> where TEntity: class
    {
        /// <summary>
        /// Service name for logging
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// Manager authorization and authentication
        /// </summary>
        IIdentityService IdentityService { get; }

        /// <summary>
        /// Adds entity to repository
        /// </summary>
        /// <param name="entity"></param>
        /// <returns>Added entity</returns>
        Task<TEntity> CreateAsync(TEntity entity);
        /// <summary>
        /// Retrieves a single entity based on specification
        /// </summary>
        /// <param name="spec"></param>
        /// <returns></returns>
        Task<TEntity> ReadSingleAsync(Specification<TEntity> spec);
        /// <summary>
        /// Retrieves a single entity based on entity provided (required Equals override for comparison by primary keys)
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<TEntity> ReadSingleAsync(TEntity entity);
        /// <summary>
        /// Updates an existing entity with entity provided. Existence of entity should be verified in advance.
        /// Throws an exception from repository if entity does not exist.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<TEntity> UpdateAsync(TEntity entity);
        /// <summary>
        /// Deletes a single entity. Existence of entity should be verified in advance.
        /// Throws an exception from repository if entity does not exist.
        /// </summary>
        /// <param name="spec"></param>
        /// <returns></returns>
        Task DeleteSingleAsync(Specification<TEntity> spec);
        /// <summary>
        /// Deletes entities based on specification
        /// </summary>
        /// <param name="spec"></param>
        /// <returns>Number of entities deleted</returns>
        Task<int> DeleteAsync(Specification<TEntity> spec);
        /// <summary>
        /// Counts a total number of pages based on specification
        /// </summary>
        /// <param name="spec"></param>
        /// <returns></returns>
        Task<int> PageCountAsync(Specification<TEntity> spec);
        /// <summary>
        /// Deletes related to provided entity entities. Used to avoid remaining any leftovers from deleted entities.
        /// Existence of entity should be verified in advance.
        /// Throws an exception from repository if entity does not exist.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task DeleteRelatedEntitiesAsync(TEntity entity);
        /// <summary>
        /// Counts a total number entities based on specification
        /// </summary>
        /// <param name="spec"></param>
        /// <returns></returns>
        Task<int> CountTotalAsync(Specification<TEntity> spec);
        /// <summary>
        /// Verifies existence of at least one entity based on specification
        /// </summary>
        /// <param name="spec"></param>
        /// <returns></returns>
        Task<bool> ExistsAsync(Specification<TEntity> spec);
        /// <summary>
        /// Verifies existence of provided entity
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<bool> ExistsAsync(TEntity entity);
        /// <summary>
        /// Verifies data integrity of entity before creating
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task ValidateCreateWithExceptionAsync(TEntity entity);
        /// <summary>
        /// Verifies data integrity of entity before updating
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task ValidateUpdateWithExceptionAsync(TEntity entity);
        /// <summary>
        /// Retrieves entities based on specification
        /// </summary>
        /// <param name="spec"></param>
        /// <returns>Entities fulfilling specification</returns>
        Task<IEnumerable<TEntity>> EnumerateAsync(Specification<TEntity> spec);
        /// <summary>
        /// Retrieves entities related to repository entity (based on specification) by select expression
        /// </summary>
        /// <typeparam name="TRelated"></typeparam>
        /// <param name="spec"></param>
        /// <param name="relatedSelect"></param>
        /// <returns></returns>
        Task<IEnumerable<TRelated>> EnumerateRelatedAsync<TRelated>(Specification<TEntity> spec, Expression<Func<TEntity, TRelated>> relatedSelect) where TRelated: class;
        /// <summary>
        /// Enumerates entities related to repository entity (based on specification) by select expression
        /// </summary>
        /// <typeparam name="TRelated"></typeparam>
        /// <param name="listRelatedSpec"></param>
        /// <param name="relatedEnumSelect"></param>
        /// <returns></returns>
        Task<IEnumerable<TRelated>> EnumerateRelatedEnumAsync<TRelated>(
           Specification<TEntity> listRelatedSpec,
           Expression<Func<TEntity, IEnumerable<TRelated>>> relatedEnumSelect) where TRelated: class;
    }
}
