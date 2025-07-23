using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsWebsite.BBL.DTOs.UserRequests
{
    public class ArticleRequest
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public string? AuthorId { get; set; }
        public string? AuthorName { get; set; }
        public string Summary { get; set; }
        public string Slug { get; set; }

    }
}
