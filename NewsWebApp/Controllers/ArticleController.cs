using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NewsWebsite.BBL.DTOs;
using NewsWebsite.BBL.DTOs.UserRequests;
using NewsWebsite.Services.Abstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsWebApp.Controllers
{
    public class ArticleController(IServiceManager serviceManager) : BaseController
    {
        [HttpGet]
        public async Task<ActionResult<PaginatedResponse<ArticleResponse>>> GetAllArticles([FromQuery] ArticleQueryParameters parameters) //GET   BaseUrl/api/Articles
        {
            var products = await serviceManager.ArticleService.GetAllArticlesAsync(parameters);
            return Ok(products);
        }


        [Authorize(Roles = "User")]
        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<ArticleResponse>>> GetArticle(int id) //GET   BaseUrl/api/Articles/18
        {
            var product = await serviceManager.ArticleService.GetArticleAsync(id);
            if(product == null) return NotFound();
            return Ok(product);
        }


        [Authorize]
        [HttpPost]
        public async Task<ActionResult<ArticleResponse>> CreateArticle([FromBody] ArticleRequest request)
        {
            request.AuthorId = GetCurrentUserId()!;
            request.AuthorName = GetCurrentUserDisplayName()!;
            var createdArticle = await serviceManager.ArticleService.CreateArticleAsync(request);
            return CreatedAtAction(nameof(GetArticle), new { id = createdArticle.Id }, createdArticle);
        }

        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateArticle(int id, [FromBody] ArticleRequest request)
        {
            var success = await serviceManager.ArticleService.UpdateArticleAsync(id, request);
            if (!success) return NotFound();
            return NoContent();
        }

        [Authorize(Roles = "SuperAdmin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteArticle(int id)
        {
            var success = await serviceManager.ArticleService.DeleteArticleAsync(id);
            if (!success) return NotFound();
            return NoContent();
        }

    }
}
