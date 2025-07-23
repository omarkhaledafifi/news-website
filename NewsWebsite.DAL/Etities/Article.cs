using NewsWebsite.DAL.Etities.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NewsWebsite.DAL.Etities
{
    public class Article : BaseEntity
    {

        [Required]
        public string Title { get; set; }

        public string? Summary { get; set; }

        [Required]
        public string Content { get; set; }

        public string Slug { get; set; }

        public string AuthorName { get; set; }
        public string AuthorId { get; set; }

        public DateTime PublishedAt { get; set; } = DateTime.UtcNow;
    }
}
