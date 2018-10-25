using ApplicationCore.Entities;
using ApplicationCore.Exceptions;
using ApplicationCore.Identity;
using ApplicationCore.Interfaces;
using ApplicationCore.Specifications;
using Infrastructure.Data;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public abstract class Service<TEntity>: IService<TEntity> where TEntity: Entity
    {
        protected DbContext _context;
        protected IAppLogger<Service<TEntity>> _logger;
        protected IScopedParameters _scopedParameters;
        protected IAuthorizationParameters<TEntity> _authoriationParameters;

        public IIdentityService IdentityService { get; }
        public string Name { get; set; }

        public Service(
            DbContext context,
            IIdentityService identityService,
            IScopedParameters scopedParameters,
            IAuthorizationParameters<TEntity> authoriationParameters,
            IAppLogger<Service<TEntity>> logger)
        {
            _context = context;
            _scopedParameters = scopedParameters;
            _authoriationParameters = authoriationParameters;
            IdentityService = identityService;
            _logger = logger;
            Name = typeof(TEntity).Name + "Service";
        }

        public virtual async Task<TEntity> CreateAsync(TEntity entity)
        {
            await ValidateCreateWithExceptionAsync(entity);
            if (_authoriationParameters.CreateAuthorizationRequired)
            {
                entity.OwnerId = _scopedParameters.ClaimsPrincipal.Identity.Name;
                await AuthorizeWithException(entity, _authoriationParameters.CreateOperationRequirement);
            }
            var result = await _context.CreateAsync(_logger, entity);
            _logger.Trace("{Name} added entity {entity}", Name, result);
            return result;
        }
        public virtual async Task<TEntity> ReadSingleAsync(Specification<TEntity> spec)
        {
            var entity = await _context.ReadSingleBySpecAsync(_logger, spec);
            if (_authoriationParameters.ReadAuthorizationRequired)
                await AuthorizeWithException(entity, _authoriationParameters.ReadOperationRequirement);
            _logger.Trace("{Name} retreived single entity {entity} by spec: {spec}", Name, entity, spec);
            return entity;
        }
        public virtual async Task<TEntity> ReadSingleAsync(TEntity entity)
        {
            entity = await _context.ReadSingleAsync(_logger, entity);
            if (_authoriationParameters.ReadAuthorizationRequired)
                await AuthorizeWithException(entity, _authoriationParameters.ReadOperationRequirement);
            _logger.Trace("{Name} retreived single entity {entity}", Name, entity);
            return entity;
        }
        public virtual async Task<TEntity> UpdateAsync(TEntity entity)
        {
            await ValidateUpdateWithExceptionAsync(entity);
            if (_authoriationParameters.UpdateAuthorizationRequired)
                await AuthorizeWithException(entity, _authoriationParameters.UpdateOperationRequirement);
            var result = await _context.UpdateAsync(_logger, entity);
            _logger.Trace("{Name} updated entity {entity}", Name, result);
            return result;
        }
        public virtual async Task<IEnumerable<TEntity>> EnumerateAsync(Specification<TEntity> spec)
        {
            var entities = await _context.EnumerateAsync(_logger, spec);
            if (_authoriationParameters.ReadAuthorizationRequired)
                entities = await ReadAuthorizedOnlyFilter(entities);
            _logger.Trace("{Name} listed: {resultCount} entities by spec: {spec}", Name, entities.Count(), spec);
            return entities;
        }
        public virtual async Task<IEnumerable<TRelated>> EnumerateRelatedAsync<TRelated>(Specification<TEntity> spec, Expression<Func<TEntity, TRelated>> relatedSelect) where TRelated: class
        {
            var relatedEntities = await _context.EnumerateRelatedAsync(_logger, spec, relatedSelect);
            if (_authoriationParameters.ReadAuthorizationRequired)
                relatedEntities = await ReadAuthorizedOnlyFilter(relatedEntities);
            _logger.Trace("{Name} listed related: {resultCount} entities by spec: {spec}", Name, relatedEntities.Count(), spec);
            return relatedEntities;
        }
        public async Task<IEnumerable<TRelated>> EnumerateRelatedEnumAsync<TRelated>(
           Specification<TEntity> listRelatedSpec,
           Expression<Func<TEntity, IEnumerable<TRelated>>> relatedEnumSelect) where TRelated: class
        {
            var relatedEntities = await _context.EnumerateRelatedEnumAsync(_logger, listRelatedSpec, relatedEnumSelect);
            if (_authoriationParameters.ReadAuthorizationRequired)
                relatedEntities = await ReadAuthorizedOnlyFilter(relatedEntities);
            _logger.Trace("{Name} listed related enum: {resultCount} entities by spec: {spec}", Name, relatedEntities.Count(), listRelatedSpec);
            return relatedEntities;
        }
        public virtual async Task DeleteSingleAsync(Specification<TEntity> spec)
        {
            var entity = await _context.ReadSingleBySpecAsync(_logger, spec);
            await DeleteSingleAsync(entity);
            _logger.Trace("{Name} deleted: {entity} by spec: {spec}", Name, entity, spec);
        }
        public virtual async Task<int> DeleteAsync(Specification<TEntity> spec)
        {
            var entities = await _context.EnumerateAsync(_logger, spec);
            foreach (var entity in entities)
                await DeleteSingleAsync(entity);
            _logger.Trace("{Name} deleted: {resultCount} by spec: {spec}", Name, entities.Count(), spec);
            return entities.Count();
        }
        protected virtual async Task DeleteSingleAsync(TEntity entity)
        {
            if (_authoriationParameters.DeleteAuthorizationRequired)
                await AuthorizeWithException(entity, _authoriationParameters.DeleteOperationRequirement);
            await DeleteRelatedEntitiesAsync(entity);
            await _context.DeleteAsync(_logger, entity, false);
            _logger.Trace("{Name} deleted {entity}", Name, entity);
        }
        public virtual async Task DeleteRelatedEntitiesAsync(TEntity entity)
        {
            _logger.Trace("{Name} deleted entity related to {entity} (if such existed)", Name, entity);
            await Task.CompletedTask;
        }
        public virtual async Task ValidateCreateWithExceptionAsync(TEntity entity)
        {
            //_logger.Trace("{Name} validated {entity}", Name, entity);
            await Task.CompletedTask;
        }
        public virtual async Task ValidateUpdateWithExceptionAsync(TEntity entity)
        {
            //_logger.Trace("{Name} validated {entity}", Name, entity);
            await Task.CompletedTask;
        }
        public virtual async Task<int> PageCountAsync(Specification<TEntity> spec)
        {
            int totalCount = await _context.CountAsync(_logger, spec);
            int result = (int)Math.Ceiling(((decimal)totalCount / spec.Take));
            _logger.Trace("{Name} got page count {count} by spec: {spec}", Name, result, spec);
            return result;
        }
        public virtual async Task<int> CountTotalAsync(Specification<TEntity> spec)
        {
            int result = await _context.CountAsync(_logger, spec);
            _logger.Trace("{Name} got entities count {count} by spec: {spec}", Name, result, spec);
            return result;
        }
        public virtual async Task<bool> ExistsAsync(Specification<TEntity> spec)
        {
            bool result = await _context.ExistsBySpecAsync(_logger, spec);
            _logger.Trace("{Name} checked existance ({result}) by spec: {spec}", Name, result, spec);
            return result;
        }
        public virtual async Task<bool> ExistsAsync(TEntity entity)
        {
            bool result = await _context.ExistsAsync(_logger, entity);
            _logger.Trace("{Name} checked existance ({result})", Name, result, entity);
            return result;
        }

        protected async Task<IEnumerable<TCustom>> ReadAuthorizedOnlyFilter<TCustom>(IEnumerable<TCustom> entities) where TCustom: class
        {
            var authorizedEntities = new List<TCustom>();
            foreach (var entity in entities)
                if (await Authorize(entity, _authoriationParameters.ReadOperationRequirement))
                    authorizedEntities.Add(entity);
            return authorizedEntities;
        }

        protected async Task AuthorizeWithException<TCustom>(TCustom entity, OperationAuthorizationRequirement requirement) where TCustom: class
        {
            if (!await Authorize(entity, requirement))
            {
                string message = $"{entity} {requirement.Name} authorization failure";
                _logger.Trace(message);
                throw new AuthorizationException(message);
            }
        }
        protected async Task AuthorizeWithException<TCustom>(object key, OperationAuthorizationRequirement requirement) where TCustom: class
        {
            var entity = _context.ReadByKeyAsync<TCustom, Service<TEntity>>(_logger, key, true);
            await AuthorizeWithException(entity, requirement);
        }
        protected async Task<bool> Authorize(object obj, OperationAuthorizationRequirement requirement)
        {
            return await IdentityService.AuthorizeAsync(_scopedParameters.ClaimsPrincipal, obj, requirement);
        }
    }
}
