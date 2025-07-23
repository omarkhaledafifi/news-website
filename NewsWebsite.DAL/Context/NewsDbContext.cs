using Microsoft.EntityFrameworkCore;
using NewsWebsite.DAL.Configurations;
using NewsWebsite.DAL.Etities;
using System;

namespace NewsWebsite.DAL.Context
{
    public class NewsDbContext : DbContext
    {
        public NewsDbContext(DbContextOptions<NewsDbContext> options)
            : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ArticleConfiguration).Assembly);
        }

        public DbSet<Article> Articles { get; set; }
    }
}
