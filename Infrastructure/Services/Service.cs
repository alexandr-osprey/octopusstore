using ApplicationCore.Entities;
using ApplicationCore.Exceptions;
using ApplicationCore.Identity;
using ApplicationCore.Interfaces;
using ApplicationCore.Interfaces.Services;
using ApplicationCore.Specifications;
using Infrastructure.Data;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public abstract class Service<TEntity>: IService<TEntity> where TEntity: Entity
    {
        protected DbContext _сontext { get; }
        protected IAppLogger<Service<TEntity>> _logger { get; }
        protected IScopedParameters _scopedParameters { get; }
        protected IAuthorizationParameters<TEntity> _authoriationParameters { get; }

        protected IIdentityService _identityService { get; }
        public string Name { get; set; }

        public Service(
            DbContext context,
            IIdentityService identityService,
            IScopedParameters scopedParameters,
            IAuthorizationParameters<TEntity> authoriationParameters,
            IAppLogger<Service<TEntity>> logger)
        {
            _сontext = context;
            _scopedParameters = scopedParameters;
            _authoriationParameters = authoriationParameters;
            _identityService = identityService;
            _logger = logger;
            Name = typeof(TEntity).Name + "Service";
        }

        public virtual async Task<TEntity> CreateAsync(TEntity entity)
        {
            var entry = _сontext.Entry(entity);
            var result = await _сontext.Add(_logger, entity, true, false);
            await ValidateWithExceptionAsync(entry);
            entity.OwnerId = _scopedParameters.CurrentUserId ?? throw new Exception("User identity not provided for entity creation");
            await ValidateCustomUniquinessWithException(entity);
            if (_authoriationParameters.CreateAuthorizationRequired)
                await AuthorizeWithException(_authoriationParameters.CreateOperationRequirement, entity);
            await ModifyBeforeSaveAsync(entry);
            await _сontext.SaveChangesAsync();
            _logger.Trace("{Name} added entity {entity}", Name, result);
            return result;
        }

        public virtual async Task<TEntity> ReadSingleAsync(Specification<TEntity> spec)
        {
            var entity = await _сontext.ReadSingleBySpecAsync(_logger, spec, true);
            if (_authoriationParameters.ReadAuthorizationRequired)
                await AuthorizeWithException(_authoriationParameters.ReadOperationRequirement, entity);
            _logger.Trace("{Name} retreived single entity {entity} by spec: {spec}", Name, entity, spec);
            return entity;
        }

        public virtual async Task<TEntity> ReadSingleAsync(TEntity entity)
        {
            entity = await _сontext.ReadSingleAsync(_logger, entity, true);
            if (_authoriationParameters.ReadAuthorizationRequired)
                await AuthorizeWithException(_authoriationParameters.ReadOperationRequirement, entity);
            _logger.Trace("{Name} retreived single entity {entity}", Name, entity);
            return entity;
        }

        public virtual async Task<TEntity> UpdateAsync(TEntity entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));
            var entityEntry = _сontext.Entry(entity);
            
            if (entityEntry.State == EntityState.Detached)
                throw new EntityValidationException($"Entity {entity} not being tracked");
            await ValidateWithExceptionAsync(entityEntry);
            if (_authoriationParameters.UpdateAuthorizationRequired)
            {
                await AuthorizeWithException(_authoriationParameters.UpdateOperationRequirement, entity);
            }
            await ModifyBeforeSaveAsync(entityEntry);
            var result = await _сontext.UpdateSingleAsync(_logger, entity);
            _logger.Trace("{Name} updated entity {entity}", Name, result);
            return result;
        }

        public virtual async Task<IEnumerable<TEntity>> EnumerateAsync(Specification<TEntity> spec)
        {
            var entities = await _сontext.EnumerateAsync(_logger, spec);
            if (_authoriationParameters.ReadAuthorizationRequired)
                entities = await ReadAuthorizedOnlyFilter(entities);
            _logger.Trace("{Name} listed: {resultCount} entities by spec: {spec}", Name, entities.Count(), spec);
            return entities;
        }

        public virtual async Task<IEnumerable<TRelated>> EnumerateRelatedAsync<TRelated>(Specification<TEntity> spec, Expression<Func<TEntity, TRelated>> relatedSelect) where TRelated: class
        {
            var relatedEntities = await _сontext.EnumerateRelatedAsync(_logger, spec, relatedSelect);
            if (_authoriationParameters.ReadAuthorizationRequired)
                relatedEntities = await ReadAuthorizedOnlyFilter(relatedEntities);
            _logger.Trace("{Name} listed related: {resultCount} entities by spec: {spec}", Name, relatedEntities.Count(), spec);
            return relatedEntities;
        }

        public async Task<IEnumerable<TRelated>> EnumerateRelatedEnumAsync<TRelated>(
           Specification<TEntity> listRelatedSpec,
           Expression<Func<TEntity, IEnumerable<TRelated>>> relatedEnumSelect) where TRelated: class
        {
            var relatedEntities = await _сontext.EnumerateRelatedEnumAsync(_logger, listRelatedSpec, relatedEnumSelect);
            if (_authoriationParameters.ReadAuthorizationRequired)
                relatedEntities = await ReadAuthorizedOnlyFilter(relatedEntities);
            _logger.Trace("{Name} listed related enum: {resultCount} entities by spec: {spec}", Name, relatedEntities.Count(), listRelatedSpec);
            return relatedEntities;
        }

        public virtual async Task DeleteSingleAsync(Specification<TEntity> spec)
        {
            if (spec == null) throw new ArgumentNullException(nameof(spec));
            var entity = await _сontext.ReadSingleBySpecAsync(_logger, spec);
            await DeleteSingleAsync(entity);
            _logger.Trace("{Name} deleted: {entity} by spec: {spec}", Name, entity, spec);
        }

        public virtual async Task<int> DeleteAsync(Specification<TEntity> spec)
        {
            if (spec == null) throw new ArgumentNullException(nameof(spec));
            var entities = await _сontext.EnumerateAsync(_logger, spec);
            foreach (var entity in entities)
                await DeleteSingleAsync(entity);
            _logger.Trace("{Name} deleted: {resultCount} by spec: {spec}", Name, entities.Count(), spec);
            return entities.Count();
        }

        protected virtual async Task DeleteSingleAsync(TEntity entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            if (_authoriationParameters.DeleteAuthorizationRequired)
                await AuthorizeWithException(_authoriationParameters.DeleteOperationRequirement, entity);
            await DeleteRelatedEntitiesAsync(entity);
            await _сontext.DeleteAsync(_logger, entity, false);
            _logger.Trace("{Name} deleted {entity}", Name, entity);
        }

        public virtual async Task DeleteRelatedEntitiesAsync(TEntity entity)
        {
            _logger.Trace("{Name} deleted entity related to {entity} (if such existed)", Name, entity);
            await Task.CompletedTask;
        }

        protected virtual async Task ValidateCustomUniquinessWithException(TEntity entity)
        {
            await Task.CompletedTask;
        }

        protected virtual Task ValidateWithExceptionAsync(EntityEntry<TEntity> entry)
        {
            if (entry == null)
                throw new ArgumentNullException(nameof(entry));
            return Task.CompletedTask;
        }

        public virtual async Task<int> PageCountAsync(Specification<TEntity> spec)
        {
            int totalCount = await _сontext.CountAsync(_logger, spec);
            int take = spec.Take == 0 ? totalCount : spec.Take;
            take = take == 0 ? 1 : take;
            int result = (int)Math.Ceiling(((decimal)totalCount / take));
            _logger.Trace("{Name} got page count {count} by spec: {spec}", Name, result, spec);
            return result;
        }

        public virtual async Task<int> CountTotalAsync(Specification<TEntity> spec)
        {
            int result = await _сontext.CountAsync(_logger, spec);
            _logger.Trace("{Name} got entities count {count} by spec: {spec}", Name, result, spec);
            return result;
        }

        public virtual async Task<bool> ExistsAsync(Specification<TEntity> spec)
        {
            bool result = await _сontext.ExistsBySpecAsync(_logger, spec);
            _logger.Trace("{Name} checked existance ({result}) by spec: {spec}", Name, result, spec);
            return result;
        }

        public virtual async Task<bool> ExistsAsync(TEntity entity)
        {
            bool result = await _сontext.ExistsAsync(_logger, entity);
            _logger.Trace("{Name} checked existance ({result})", Name, result, entity);
            return result;
        }

        protected virtual Task ModifyBeforeSaveAsync(EntityEntry<TEntity> entry)
        {
            if (entry == null)
                throw new ArgumentNullException(nameof(entry));
            return Task.CompletedTask;
        }

        protected async Task<IEnumerable<TCustom>> ReadAuthorizedOnlyFilter<TCustom>(IEnumerable<TCustom> entities) where TCustom: class
        {
            var authorizedEntities = new List<TCustom>();
            foreach (var entity in entities)
                if (await Authorize(_authoriationParameters.ReadOperationRequirement, entity))
                    authorizedEntities.Add(entity);
            return authorizedEntities;
        }

        protected async Task AuthorizeWithException<TCustom>(OperationAuthorizationRequirement requirement, TCustom finalValue) where TCustom : class
        {
            if (!await Authorize(requirement, finalValue))
            {
                string message = $"{finalValue} {requirement.Name} authorization failure";
                _logger.Trace(message);
                throw new AuthorizationException(message);
            }
        }

        //protected async Task AuthorizeWithException<TCustom>(object key, OperationAuthorizationRequirement requirement) where TCustom: class
        //{
        //    var entity = Context.ReadByKeyAsync<TCustom, Service<TEntity>>(Logger, key, true);
        //    await AuthorizeWithException(entity, requirement);
        //}

        protected async Task<bool> Authorize<T>(OperationAuthorizationRequirement requirement, T finalValue) where T : class
        {
            return await _identityService.AuthorizeAsync(_scopedParameters.ClaimsPrincipal, requirement, finalValue, false);
        }

        protected bool IsPropertyModified<TProperty>(EntityEntry<TEntity> entityEntry, Expression<Func<TEntity, TProperty>> propertyExpression, bool isUpdatable = true)
        {
            bool isModified = false;
            if (entityEntry.State == EntityState.Unchanged)
                isModified = false;
            else if (entityEntry.State == EntityState.Modified)
            {
                PropertyEntry<TEntity, TProperty> statusProperty = entityEntry.Property(propertyExpression);
                isModified = statusProperty.IsModified;
                if (isModified && !isUpdatable)
                    throw new EntityValidationException($"Property {statusProperty.Metadata.Name} is not updatable");
            }
            else if (entityEntry.State == EntityState.Added)
                isModified = true;
            return isModified;
        }
    }
}
