using NewsWebsite.BBL.DTOs.UserRequests;
using NewsWebsite.DAL.Etities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsWebsite.BBL.Specifications
{
    public class ArticleCountSpecifications(ArticleQueryParameters parameters) : BaseSpecification<Article>(p =>
            (string.IsNullOrWhiteSpace(parameters.AuthorId) || p.AuthorId == parameters.AuthorId) &&
            (string.IsNullOrWhiteSpace(parameters.SearchInTitles) ||
                    (p.Title.ToLower().Contains(parameters.SearchInTitles.ToLower()))))
    {

    }
}