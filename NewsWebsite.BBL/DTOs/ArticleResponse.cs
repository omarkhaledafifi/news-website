using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsWebsite.BBL.DTOs
{
    public record ArticleResponse
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string? Summary { get; set; }
        public string Content { get; set; }
        public string Slug { get; set; }
        public string AuthorId { get; set; }
        public string AuthorName { get; set; }
        public DateTime PublishedAt { get; set; } = DateTime.UtcNow;
    }
}
