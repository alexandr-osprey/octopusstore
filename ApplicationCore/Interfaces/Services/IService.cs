using ApplicationCore.Entities;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ApplicationCore.Interfaces
{
    public interface IService<T> where T : Entity
    {
        string Name { get; set; }

        Task<T> AddAsync(T entity);
        Task<T> GetSingleAsync(ISpecification<T> spec);
        Task<IEnumerable<T>> ListAsync(ISpecification<T> spec);
        Task<IEnumerable<TRelated>> ListRelatedAsync<TRelated>(ISpecification<T> spec, Expression<Func<T, TRelated>> relatedSelect) where TRelated : Entity;
        Task<T> UpdateAsync(T entity);
        Task DeleteAsync(ISpecification<T> spec);
        int PageCount(ISpecification<T> spec);
        Task DeleteRelatedEntitiesAsync(T entity);
        int CountTotal(ISpecification<T> spec);
        bool Exist(ISpecification<T> spec);
    }
}
