using Microsoft.EntityFrameworkCore;
using NewsWebsite.DAL.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsWebsite.BBL.Specifications
{
    public class SpecificationEvaluator
    {
        public static IQueryable<T> CreateQuery<T>(IQueryable<T> inputQuery, ISpecification<T> specifications) where T : class
        {
            //var inputQuery = inputQuery;
            if (specifications.Crateria is not null)
                inputQuery = inputQuery.Where(specifications.Crateria);

            //foreach (var item in specifications.IncludeExpressions)
            //    inputQuery.Include(item);
            inputQuery = specifications.IncludeExpressions.Aggregate(inputQuery, (currentQuery, include) => currentQuery.Include(include));

            if (specifications.OrderBy is not null)
                inputQuery = inputQuery.OrderBy(specifications.OrderBy);
            else if (specifications.OrderByDesc is not null)
                inputQuery = inputQuery.OrderByDescending(specifications.OrderByDesc);

            if (specifications.IsPaginated)
                inputQuery = inputQuery.Skip(specifications.Skip).Take(specifications.Take);

            return inputQuery;

        }
    }
}
