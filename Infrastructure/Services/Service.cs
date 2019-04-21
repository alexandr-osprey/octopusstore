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
        public DbContext Context { get; }
        protected IAppLogger<Service<TEntity>> Logger { get; }
        public IScopedParameters ScopedParameters { get; }
        protected IAuthorizationParameters<TEntity> AuthoriationParameters { get; }

        public IIdentityService IdentityService { get; }
        public string Name { get; set; }

        public Service(
            DbContext context,
            IIdentityService identityService,
            IScopedParameters scopedParameters,
            IAuthorizationParameters<TEntity> authoriationParameters,
            IAppLogger<Service<TEntity>> logger)
        {
            Context = context;
            ScopedParameters = scopedParameters;
            AuthoriationParameters = authoriationParameters;
            IdentityService = identityService;
            Logger = logger;
            Name = typeof(TEntity).Name + "Service";
        }

        public virtual async Task<TEntity> CreateAsync(TEntity entity)
        {
            var entry = Context.Entry(entity);
            var result = await Context.Add(Logger, entity, true, false);
            await ValidateWithExceptionAsync(entry);
            entity.OwnerId = ScopedParameters.CurrentUserId ?? throw new Exception("User identity not provided for entity creation");
            await ValidateCustomUniquinessWithException(entity);
            if (AuthoriationParameters.CreateAuthorizationRequired)
                await AuthorizeWithException(AuthoriationParameters.CreateOperationRequirement, entity);
            await ModifyBeforeSaveAsync(entry);
            await Context.SaveChangesAsync();
            Logger.Trace("{Name} added entity {entity}", Name, result);
            return result;
        }

        public virtual async Task<TEntity> ReadSingleAsync(Specification<TEntity> spec)
        {
            var entity = await Context.ReadSingleBySpecAsync(Logger, spec, true);
            if (AuthoriationParameters.ReadAuthorizationRequired)
                await AuthorizeWithException(AuthoriationParameters.ReadOperationRequirement, entity);
            Logger.Trace("{Name} retreived single entity {entity} by spec: {spec}", Name, entity, spec);
            return entity;
        }

        public virtual async Task<TEntity> ReadSingleAsync(TEntity entity)
        {
            entity = await Context.ReadSingleAsync(Logger, entity, true);
            if (AuthoriationParameters.ReadAuthorizationRequired)
                await AuthorizeWithException(AuthoriationParameters.ReadOperationRequirement, entity);
            Logger.Trace("{Name} retreived single entity {entity}", Name, entity);
            return entity;
        }

        public virtual async Task<TEntity> UpdateAsync(TEntity entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));
            var entityEntry = Context.Entry(entity);
            
            if (entityEntry.State == EntityState.Detached)
                throw new EntityValidationException($"Entity {entity} not being tracked");
            await ValidateWithExceptionAsync(entityEntry);
            if (AuthoriationParameters.UpdateAuthorizationRequired)
            {
                await AuthorizeWithException(AuthoriationParameters.UpdateOperationRequirement, entity);
            }
            await ModifyBeforeSaveAsync(entityEntry);
            var result = await Context.UpdateSingleAsync(Logger, entity);
            Logger.Trace("{Name} updated entity {entity}", Name, result);
            return result;
        }

        public virtual async Task<IEnumerable<TEntity>> EnumerateAsync(Specification<TEntity> spec)
        {
            var entities = await Context.EnumerateAsync(Logger, spec);
            if (AuthoriationParameters.ReadAuthorizationRequired)
                entities = await ReadAuthorizedOnlyFilter(entities);
            Logger.Trace("{Name} listed: {resultCount} entities by spec: {spec}", Name, entities.Count(), spec);
            return entities;
        }

        public virtual async Task<IEnumerable<TRelated>> EnumerateRelatedAsync<TRelated>(Specification<TEntity> spec, Expression<Func<TEntity, TRelated>> relatedSelect) where TRelated: class
        {
            var relatedEntities = await Context.EnumerateRelatedAsync(Logger, spec, relatedSelect);
            if (AuthoriationParameters.ReadAuthorizationRequired)
                relatedEntities = await ReadAuthorizedOnlyFilter(relatedEntities);
            Logger.Trace("{Name} listed related: {resultCount} entities by spec: {spec}", Name, relatedEntities.Count(), spec);
            return relatedEntities;
        }

        public async Task<IEnumerable<TRelated>> EnumerateRelatedEnumAsync<TRelated>(
           Specification<TEntity> listRelatedSpec,
           Expression<Func<TEntity, IEnumerable<TRelated>>> relatedEnumSelect) where TRelated: class
        {
            var relatedEntities = await Context.EnumerateRelatedEnumAsync(Logger, listRelatedSpec, relatedEnumSelect);
            if (AuthoriationParameters.ReadAuthorizationRequired)
                relatedEntities = await ReadAuthorizedOnlyFilter(relatedEntities);
            Logger.Trace("{Name} listed related enum: {resultCount} entities by spec: {spec}", Name, relatedEntities.Count(), listRelatedSpec);
            return relatedEntities;
        }

        public virtual async Task DeleteSingleAsync(Specification<TEntity> spec)
        {
            if (spec == null) throw new ArgumentNullException(nameof(spec));
            var entity = await Context.ReadSingleBySpecAsync(Logger, spec);
            await DeleteSingleAsync(entity);
            Logger.Trace("{Name} deleted: {entity} by spec: {spec}", Name, entity, spec);
        }

        public virtual async Task<int> DeleteAsync(Specification<TEntity> spec)
        {
            if (spec == null) throw new ArgumentNullException(nameof(spec));
            var entities = await Context.EnumerateAsync(Logger, spec);
            foreach (var entity in entities)
                await DeleteSingleAsync(entity);
            Logger.Trace("{Name} deleted: {resultCount} by spec: {spec}", Name, entities.Count(), spec);
            return entities.Count();
        }

        protected virtual async Task DeleteSingleAsync(TEntity entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            if (AuthoriationParameters.DeleteAuthorizationRequired)
                await AuthorizeWithException(AuthoriationParameters.DeleteOperationRequirement, entity);
            await DeleteRelatedEntitiesAsync(entity);
            await Context.DeleteAsync(Logger, entity, false);
            Logger.Trace("{Name} deleted {entity}", Name, entity);
        }

        public virtual async Task DeleteRelatedEntitiesAsync(TEntity entity)
        {
            Logger.Trace("{Name} deleted entity related to {entity} (if such existed)", Name, entity);
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
            int totalCount = await Context.CountAsync(Logger, spec);
            int take = spec.Take == 0 ? totalCount : spec.Take;
            take = take == 0 ? 1 : take;
            int result = (int)Math.Ceiling(((decimal)totalCount / take));
            Logger.Trace("{Name} got page count {count} by spec: {spec}", Name, result, spec);
            return result;
        }

        public virtual async Task<int> CountTotalAsync(Specification<TEntity> spec)
        {
            int result = await Context.CountAsync(Logger, spec);
            Logger.Trace("{Name} got entities count {count} by spec: {spec}", Name, result, spec);
            return result;
        }

        public virtual async Task<bool> ExistsAsync(Specification<TEntity> spec)
        {
            bool result = await Context.ExistsBySpecAsync(Logger, spec);
            Logger.Trace("{Name} checked existance ({result}) by spec: {spec}", Name, result, spec);
            return result;
        }

        public virtual async Task<bool> ExistsAsync(TEntity entity)
        {
            bool result = await Context.ExistsAsync(Logger, entity);
            Logger.Trace("{Name} checked existance ({result})", Name, result, entity);
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
                if (await Authorize(AuthoriationParameters.ReadOperationRequirement, entity))
                    authorizedEntities.Add(entity);
            return authorizedEntities;
        }

        protected async Task AuthorizeWithException<TCustom>(OperationAuthorizationRequirement requirement, TCustom finalValue) where TCustom : class
        {
            if (!await Authorize(requirement, finalValue))
            {
                string message = $"{finalValue} {requirement.Name} authorization failure";
                Logger.Trace(message);
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
            return await IdentityService.AuthorizeAsync(ScopedParameters.ClaimsPrincipal, requirement, finalValue, false);
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
