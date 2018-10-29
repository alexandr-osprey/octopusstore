using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace ApplicationCore.Specifications
{
    /// <summary>
    /// Contains full information for executing a request on database
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Specification<T>  where T: class
    {
        public Expression<Func<T, bool>> Criteria { get; }
        public List<Expression<Func<T, object>>> Includes { get; } = new List<Expression<Func<T, object>>>();
        public List<string> IncludeStrings { get; } = new List<string>();
        public Func<T, int> OrderBy { get; set; } = o => o.GetHashCode();

        public int Take { get; set; }
        public int Skip { get; set; }
        public int Page { get { return Skip / Take + 1; } }
        public virtual string Description { get; set; }
        /// <summary>
        /// Select all by default
        /// </summary>
        public Specification(): this(e => true)
        {
        }
        /// <summary>
        /// Select based on criteria
        /// </summary>
        /// <param name="criteria"></param>
        public Specification(Expression<Func<T, bool>> criteria)
        {
            Criteria = criteria;
        }
        /// <summary>
        /// Select based on existing specificaiton. Does not clone properties.
        /// </summary>
        /// <param name="specification"></param>
        public Specification(Specification<T> specification)
        {
            Criteria = specification.Criteria;
            Includes = specification.Includes;
            IncludeStrings = specification.IncludeStrings;
            Take = specification.Take;
            Skip = specification.Skip;
            Description = specification.Description;
        }
        /// <summary>
        /// Select based on criteria with include expressions
        /// </summary>
        /// <param name="criteria"></param>
        /// <param name="includeExpressions"></param>
        public Specification(Expression<Func<T, bool>> criteria, params Expression<Func<T, object>>[] includeExpressions): this(criteria)
        {
            foreach (var i in includeExpressions)
                AddInclude(i);
        }
        /// <summary>
        /// Select based on criteria with take and skip specified
        /// </summary>
        /// <param name="criteria"></param>
        /// <param name="take"></param>
        /// <param name="skip"></param>
        public Specification(Expression<Func<T, bool>> criteria, int take, int skip): this(criteria)
        {
            Take = take;
            Skip = skip;
        }

        public override string ToString()
        {
            return $"Description: {Description}, " +
                $"Take: {Take}, " +
                $"Skip: {Skip}";
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
            return (!string.IsNullOrWhiteSpace(s));
        }
    }
}
