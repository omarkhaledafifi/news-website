using NewsWebsite.BBL.DTOs;
using NewsWebsite.BBL.DTOs.UserRequests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsWebsite.Services.Abstraction
{
    public interface IArticleService
    {
        
        //GetAllProducts => IEnumrable<ProductResponse>
        Task<PaginatedResponse<ArticleResponse>> GetAllArticlesAsync(ArticleQueryParameters parameters);
        //GetProduct
        Task<ArticleResponse> GetArticleAsync(int id);
        Task<ArticleResponse> CreateArticleAsync(ArticleRequest request);
        Task<bool> UpdateArticleAsync(int id, ArticleRequest request);
        Task<bool> DeleteArticleAsync(int id);


    }
}
