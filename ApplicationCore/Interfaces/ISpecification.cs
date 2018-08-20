using ApplicationCore.Entities;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace ApplicationCore.Interfaces
{
    public interface ISpecification<T> where T : Entity
    {
        Expression<Func<T, bool>> Criteria { get; }
        List<Expression<Func<T, object>>> Includes { get; }
        List<string> IncludeStrings { get; }
        void AddInclude(Expression<Func<T, object>> includeExpression);
        void AddInclude(string includeExpression);
        int Take { get; set; }
        int Skip { get; set; }
        int Page { get; }
        string Description { get; set; }
    }
}
