using NewsWebsite.BBL.DTOs.UserRequests;
using NewsWebsite.DAL.Etities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsWebsite.BBL.Specifications
{
    public class ArticleWithIncludesSpecifications : BaseSpecification<Article>
    {
        public ArticleWithIncludesSpecifications(int id) : base(product => product.Id == id)
        {
            AddArticleIncludes();
        }
        public ArticleWithIncludesSpecifications(ArticleQueryParameters parameters) : base(p =>
            (string.IsNullOrWhiteSpace(parameters.AuthorId) || p.AuthorId == parameters.AuthorId) &&
            (string.IsNullOrWhiteSpace(parameters.SearchInTitles) ||
                    (p.Title.ToLower().Contains(parameters.SearchInTitles.ToLower()))))
        {
            AddArticleIncludes();
            AddSorting(parameters.Options);
            AddPagination(parameters.PageSize, parameters.PageIndex);

        }


        private void AddSorting(ArticleSortingOptions options)
        {
            switch (options)
            {
                case ArticleSortingOptions.DateAscending:
                    AddOrderBy(d => d.PublishedAt);
                    break;
                case ArticleSortingOptions.DateDescending:
                    AddOrderByDescending(d => d.PublishedAt);
                    break;
                default:
                    break;
            }
        }
        private void AddArticleIncludes()
        {
            //AddInclude(p => p.Author);
        }


    }
}
