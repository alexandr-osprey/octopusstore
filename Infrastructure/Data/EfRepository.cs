using ApplicationCore.Interfaces;
using Microsoft.EntityFrameworkCore;
using ApplicationCore.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;
using Infrastructure.Exceptions;
using System.Linq.Expressions;

namespace Infrastructure.Data
{
    public class EfRepository<T> : IAsyncRepository<T> where T : Entity
    {
        protected readonly StoreContext _dbContext;
        private readonly IAppLogger<EfRepository<T>> _logger;
        private string _name;

        public EfRepository(StoreContext dbContext, IAppLogger<EfRepository<T>> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
            Name = typeof(T).Name + "Repository";
        }

        public string Name
        {
            get { return _name; }
            set
            {
                if (value != _name)
                {
                    _name = value;
                    _logger.Name = _name + "Logger";
                }
            }
        }

        public async Task<T> AddAsync(T entity)
        {
            if (entity == null)  {  throw new ArgumentNullException();  }
            try
            {
                _dbContext.Set<T>().Add(entity);
                await _dbContext.SaveChangesAsync();
            }
            catch (DbUpdateException updateException)
            {
                throw updateException.LogAndGetDbException(entity, _logger);
            }
            return entity;
        }
        public async Task<T> GetByIdAsync(int id)
        {
            return await _dbContext.Set<T>().FindAsync(id);
        }
        public async Task<T> GetSingleBySpecAsync(ISpecification<T> spec)
        {
            if (spec == null)  {  throw new ArgumentNullException();  }
            return (await ListAsync(spec)).FirstOrDefault();
        }
        public async Task<T> UpdateAsync(T entity)
        {
            if (entity == null)  {   throw new ArgumentNullException();  }
            try
            {
                _dbContext.Entry(entity).State = EntityState.Modified;
                await _dbContext.SaveChangesAsync();
                return entity;
            }
            catch (DbUpdateConcurrencyException concurrencyUpdateException)
            {
                throw concurrencyUpdateException.LogAndGetDbException(entity, _logger);
            }
            catch (DbUpdateException updateException)
            {
                throw updateException.LogAndGetDbException(entity, _logger);
            }
        }
        public async Task DeleteAsync(T entity)
        {
            try
            {
                _dbContext.Set<T>().Remove(entity);
                await _dbContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException concurrencyUpdateException)
            {
                throw concurrencyUpdateException.LogAndGetDbException(entity, _logger);
            }
            catch (DbUpdateException updateException)
            {
                throw updateException.LogAndGetDbException(entity, _logger);
            }
        }
        public async Task<IEnumerable<T>> ListAllAsync()
        {
            try
            {
                return await _dbContext.Set<T>().ToListAsync();
            }
            catch (Exception readException)
            {
                throw readException.LogAndGetDbException(_logger, "ListAllAsync");
            }
        }
        public async Task<IEnumerable<T>> ListAsync(ISpecification<T> spec)
        {
            if (spec == null)  { throw new ArgumentNullException(); }
            try
            {
                return await GetQueryBySpecSkipAndTake(spec).ToListAsync();
            }
            catch (Exception readException)
            {
                throw readException.LogAndGetDbException(_logger, spec.ToString());
            }
        }
        public async Task<IEnumerable<TRelated>> ListRelatedAsync<TRelated>(ISpecification<T> spec, Expression<Func<T, TRelated>> relatedSelect) where TRelated : Entity
        {
            try
            {
                return await GetQueryBySpecWithIncludes(spec)
                    .Select(relatedSelect)
                    .Distinct()
                    .ToListAsync();
            }
            catch (Exception readException)
            {
                throw readException.LogAndGetDbException(_logger, "ListRelatedAsync");
            }
        }
        public async Task<IEnumerable<TRelated>> ListRelatedEnumAsync<TRelated>(ISpecification<T> spec, Expression<Func<T, IEnumerable<TRelated>>> relatedEnumSelect) where TRelated : Entity
        {
            try
            {
                return await GetQueryBySpecWithIncludes(spec)
                    .SelectMany(relatedEnumSelect)
                    .Distinct()
                    .ToListAsync();
            }
            catch (Exception readException)
            {
                throw readException.LogAndGetDbException(_logger, "ListRelatedEnumAsync");
            }
        }
        public int Count(ISpecification<T> spec)
        {
            if (spec == null)  { throw new ArgumentNullException(); }
            try
            {
                var queryable = GetQueryBySpecWithIncludes(spec);
                return queryable.Count();
            }
            catch (Exception readException)
            {
                throw readException.LogAndGetDbException(_logger, spec.ToString());
            }
        }

        private IQueryable<T> GetQueryBySpecSkipAndTake(ISpecification<T> spec)
        {
            var result = GetQueryBySpecWithIncludes(spec);
            result = result.Skip(spec.Skip);
            return spec.Take > 0 ? result.Take(spec.Take) : result;
        }
        private IQueryable<T> GetQueryBySpecWithIncludes(ISpecification<T> spec)
        {
            // fetch a Queryable that includes all expression-based includes
            var queryableResultWithIncludes = spec.Includes
                .Aggregate(_dbContext.Set<T>().AsNoTracking().AsQueryable(),
                    (current, include) => current.Include(include));
            // modify the IQueryable to include any string-based include statements
            var secondaryResult = spec.IncludeStrings
                .Aggregate(queryableResultWithIncludes,
                    (current, include) => current.Include(include));

            return secondaryResult.Where(spec.Criteria);
        }
    }
}
