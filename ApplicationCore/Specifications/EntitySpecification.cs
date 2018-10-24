using ApplicationCore.Entities;
using System;
using System.Linq.Expressions;

namespace ApplicationCore.Specifications
{
    /// <summary>
    /// Specification for Entity instances
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class EntitySpecification<T> : Specification<T> where T : Entity
    {
        public EntitySpecification()
            : base()
        {
        }

        public EntitySpecification(EntitySpecification<T> entitySpecification)
            : base(entitySpecification)
        {
        }

        public EntitySpecification(int id)
            : base(e => e.Id == id)
        {
        }

        public EntitySpecification(Expression<Func<T, bool>> criteria)
            : base(criteria)
        {
        }
        public EntitySpecification(
            Expression<Func<T, bool>> criteria,
            params Expression<Func<T, object>>[] includeExpressions)
            : base(criteria, includeExpressions)
        {
        }
        public EntitySpecification(Expression<Func<T, bool>> criteria, int take, int skip)
            : base(criteria, take, skip)
        {
        }
    }
}
