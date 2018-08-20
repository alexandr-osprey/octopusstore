using ApplicationCore.Entities;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ApplicationCore.Interfaces
{
    public interface IAsyncRepository<T> where T : Entity
    {
        string Name { get; set; }
        Task<T> AddAsync(T entity);
        Task<T> GetByIdAsync(int id);
        Task<T> GetSingleBySpecAsync(ISpecification<T> spec);
        Task<T> UpdateAsync(T entity);
        Task DeleteAsync(T entity);
        Task<IEnumerable<T>> ListAllAsync();
        Task<IEnumerable<T>> ListAsync(ISpecification<T> spec);
        Task<IEnumerable<TRelated>> ListRelatedAsync<TRelated>(ISpecification<T> spec, Expression<Func<T, TRelated>> relatedSelect) where TRelated : Entity;
        Task<IEnumerable<TRelated>> ListRelatedEnumAsync<TRelated>(ISpecification<T> spec, Expression<Func<T, IEnumerable<TRelated>>> relatedEnumSelect) where TRelated : Entity;
        int Count(ISpecification<T> spec);
    }
}
