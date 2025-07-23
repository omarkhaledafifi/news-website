using NewsWebsite.DAL.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace NewsWebsite.BBL.Specifications
{
    public abstract class BaseSpecification<T> : ISpecification<T> where T : class
    {
        public BaseSpecification(Expression<Func<T, bool>>? crateria)
        {
            Crateria = crateria;
        }
        public Expression<Func<T, bool>> Crateria { get; private set; }

        public List<Expression<Func<T, object>>> IncludeExpressions { get; } = [];

        public Expression<Func<T, object>> OrderBy { get; private set; }

        public Expression<Func<T, object>> OrderByDesc { get; private set; }

        public bool IsPaginated { get; private set; }
        public int Skip { get; private set; }
        public int Take { get; private set; }
        protected void AddPagination(int pageSize, int pageIndex)
        {
            IsPaginated = true;
            Take = pageSize;
            Skip = (pageIndex - 1) * pageSize;
        }

        protected void AddInclude(Expression<Func<T, object>> include)
            => IncludeExpressions.Add(include);
        protected void AddOrderBy(Expression<Func<T, object>> expression)
            => OrderBy = expression;
        protected void AddOrderByDescending(Expression<Func<T, object>> expression)
            => OrderByDesc = expression;

    }
}
