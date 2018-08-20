using ApplicationCore.Entities;
using System;
using System.Linq.Expressions;

namespace ApplicationCore.Specifications
{
    public class Specification<T> : BaseSpecification<T> where T : Entity
    {
        public Specification(
            Expression<Func<T, bool>> criteria, 
            params Expression<Func<T, object>>[] includeExpressions)
            : base(criteria)
        {
            foreach (var i in includeExpressions)
                AddInclude(i);
        }
        public Specification(Expression<Func<T, bool>> criteria)
            : base(criteria){ }
        public Specification(int? id = null)
            : base(i => (!id.HasValue || i.Id == id))
        {
            Description = $"{typeof(T).Name} id=" + id ?? "null";
            Take = 1;
        }
        public Specification(Expression<Func<T, bool>> criteria, int take, int skip)
            : base(criteria)
        {
            Take = take;
            Skip = skip;
        }
    }
}
