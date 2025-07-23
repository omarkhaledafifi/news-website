using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace NewsWebsite.DAL.Contracts
{
    public interface ISpecification<T> where T : class
    {
        Expression<Func<T, bool>> Crateria { get; }
        List<Expression<Func<T, object>>> IncludeExpressions { get; }
        Expression<Func<T, object>> OrderBy { get; }
        Expression<Func<T, object>> OrderByDesc { get; }
        public bool IsPaginated { get; }
        public int Skip { get; }
        public int Take { get; }
    }
}
