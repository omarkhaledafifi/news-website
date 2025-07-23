using AutoMapper;
using NewsWebsite.DAL.Contracts;
using NewsWebsite.DAL.Etities;
using NewsWebsite.Services.Abstraction;
using NewsWebsite.BBL.Specifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NewsWebsite.BBL.DTOs;
using NewsWebsite.BBL.DTOs.UserRequests;

namespace NewsWebsite.Services.Services
{
    public class ArticleService(IUnitOfWork unitOfWork, IMapper mapper) : IArticleService
    {
        public async Task<ArticleResponse> CreateArticleAsync(ArticleRequest request)
        {
            var article = mapper.Map<Article>(request);
            var repo = unitOfWork.GetRepository<Article>();
            await repo.AddAsync(article);
            await unitOfWork.SaveChangesAsync();
            return mapper.Map<ArticleResponse>(article);
        }

        public async Task<bool> DeleteArticleAsync(int id)
        {
            var repo = unitOfWork.GetRepository<Article>();
            var article = await repo.GetAsync(id);
            if (article == null) return false;

            repo.DeleteAsync(article);
            await unitOfWork.SaveChangesAsync();
            return true;

        }

        public async Task<PaginatedResponse<ArticleResponse>> GetAllArticlesAsync(ArticleQueryParameters parameters)
        {
            var speccificatios = new ArticleWithIncludesSpecifications(parameters);
            var repo = unitOfWork.GetRepository<Article>();
            var data = await repo.GetAllAsync(speccificatios);
            var mappedData = mapper.Map<IEnumerable<Article>, IEnumerable<ArticleResponse>>(data);
            var pageCount = data.Count();
            var totalCount = await repo.CountAsync(new ArticleCountSpecifications(parameters));
            return new(parameters.PageIndex, pageCount, totalCount, mappedData);
        }

        public async Task<ArticleResponse> GetArticleAsync(int id)
        {
            var speccificatios = new ArticleWithIncludesSpecifications(id);
            var data = await unitOfWork.GetRepository<Article>().GetAsync(speccificatios);
            //var data = await repo.GetAsync(id);
            return mapper.Map<Article, ArticleResponse>(data);
        }

        public async Task<bool> UpdateArticleAsync(int id, ArticleRequest request)
        {
            var repo = unitOfWork.GetRepository<Article>();
            var article = await repo.GetAsync(id);
            if (article == null) return false;

            request.AuthorId = article.AuthorId;
            request.AuthorName = article.AuthorName;
            mapper.Map(request, article); // Updates values from request
            repo.UpdateAsync(article);
            await unitOfWork.SaveChangesAsync();
            return true;
        }
    }
}
