using ApplicationCore.Entities;
using ApplicationCore.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public abstract class Service<T> 
        : IService<T> 
        where T : Entity
    {
        protected readonly IAsyncRepository<T> _repository;
        protected readonly IAppLogger<Service<T>> _logger;
        protected string _name;
        protected int _maxTake = 200;

        public string Name
        {
            get { return _name; }
            set
            {
                if (_name != value)
                {
                    _name = value;
                    _logger.Name = value + "Logger";
                    _repository.Name = value + "Repository";
                }
            }
        }

        public Service(
            IAsyncRepository<T> repository,
            IAppLogger<Service<T>> logger)
        {
            _repository = repository;
            _logger = logger;
            Name = typeof(T).Name + "Service";
        }
        virtual public async Task<T> AddAsync(T entity)
        {
            var result = await _repository.AddAsync(entity);
            _logger.Trace("{Name} added entity {entity}", Name, result);
            return result;
        }
        virtual public async Task<T> GetSingleAsync(ISpecification<T> spec)
        {
            spec.Take = 1;
            var result = await _repository.GetSingleBySpecAsync(spec);
            _logger.Trace("{Name} retreived single entity {result} by spec: {spec}", Name, result, spec);
            return result;
        }
        virtual public async Task<T> UpdateAsync(T entity)
        {
            var result = await _repository.UpdateAsync(entity);
            _logger.Trace("{Name} updated entity {entity}", Name, result);
            return result;
        }
        virtual public async Task<IEnumerable<T>> ListAsync(ISpecification<T> spec)
        {
            var result = await _repository.ListAsync(spec);
            _logger.Trace("{Name} listed: {resultCount} entities: by spec: {spec}", Name, result.Count(), spec);
            return result;
        }
        virtual public async Task<IEnumerable<TRelated>> ListRelatedAsync<TRelated>(ISpecification<T> spec, Expression<Func<T, TRelated>> relatedSelect) where TRelated : Entity
        {
            var result = await _repository.ListRelatedAsync(spec, relatedSelect);
            _logger.Trace("{Name} listed related: {resultCount} entities: by spec: {spec}", Name, result.Count(), spec);
            return result;
        }
        virtual public async Task DeleteAsync(ISpecification<T> spec)
        {
            var entities = await _repository.ListAsync(spec);
            foreach(var entity in entities)
            {
                await DeleteRelatedEntitiesAsync(entity);
                await _repository.DeleteAsync(entity);
            }
            _logger.Trace("{Name} deleted: {resultCount}, by spec: {spec}", Name, entities.Count(), spec);
        }
        virtual public async Task DeleteRelatedEntitiesAsync(T entity)
        {
            _logger.Trace("{Name} deleted entity related to {entity} (if such existed)", Name, entity);
            await Task.CompletedTask;
        }
        public int PageCount(ISpecification<T> spec)
        {
            int totalCount = _repository.Count(spec);
            int result = (int)Math.Ceiling(((decimal)totalCount / spec.Take));
            _logger.Trace("{Name} got page count {count} by spec: {spec}", Name, result, spec);
            return result;
        }
        public int CountTotal(ISpecification<T> spec)
        {
            int result = _repository.Count(spec);
            _logger.Trace("{Name} got entities count {count} by spec: {spec}", Name, result, spec);
            return result;
        }
        public bool Exist(ISpecification<T> spec)
        {
            bool result = CountTotal(spec) != 0;
            _logger.Trace("{Name} checked existance ({result}) by spec: {spec}", Name, result, spec);
            return result;
        }
    }
}
