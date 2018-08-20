using ApplicationCore.Interfaces;
using System;
using System.Linq.Expressions;
using System.Collections.Generic;
using ApplicationCore.Entities;

namespace ApplicationCore.Specifications
{
    public abstract class BaseSpecification<T> : ISpecification<T> where T : Entity
    {
        public Expression<Func<T, bool>> Criteria { get; }
        public List<Expression<Func<T, object>>> Includes { get; } = new List<Expression<Func<T, object>>>();
        public List<string> IncludeStrings { get; } = new List<string>();

        public int Take { get; set; }
        public int Skip { get; set; }
        public int Page { get { return Skip / Take + 1; } }
        public virtual string Description { get; set; }

        public BaseSpecification(Expression<Func<T, bool>> criteria)
        {
            Criteria = criteria;
        }

        public override string ToString()
        {
            return $"Description: {Description}, " +
                $"Take: {Take}, " +
                $"Skip: {Skip}" ;
        }
        public virtual void AddInclude(Expression<Func<T, object>> includeExpression)
        {
            Includes.Add(includeExpression);
        }
        public virtual void AddInclude(string includeString)
        {
            IncludeStrings.Add(includeString);
        }
        protected static bool HasValue(string s)
        {
            return (s != null && s != "");
        }
    }
}
