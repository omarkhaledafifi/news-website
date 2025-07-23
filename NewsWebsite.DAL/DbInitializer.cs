using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NewsWebsite.DAL.Context;
using NewsWebsite.DAL.Contracts;
using NewsWebsite.DAL.Etities;
using NewsWebsite.DAL.Etities.Identity;
using NewsWebsite.DAL.IdentityData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace NewsWebsite.DAL
{
    public class DbInitializer(NewsDbContext newsContext, NewsIdentityDbContext identityDbContext,
        UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager) : IDbInitializer
    {

        public async Task InitailizeAsync()
        {
            if (newsContext.Database.GetPendingMigrations().Any())
                await newsContext.Database.MigrateAsync();
            // Add Articles
            await AddArticlesAsync();
        }

        private async Task AddArticlesAsync()
        {
            if (!newsContext.Articles.Any())
            {
                // Find an existing user to assign as author
                var author = await userManager.FindByEmailAsync("superadmin@super.com");
                if (author != null)
                {
                    var articles = new List<Article>
                    {
                        new Article
                        {
                            Title = "Breaking News: ASP.NET 8 Released!",
                            Summary = "Explore what's new in ASP.NET 8.",
                            Content = "ASP.NET 8 includes many performance improvements...",
                            Slug = "aspnet-8-released",
                            AuthorId = author.Id,
                            AuthorName = author.UserName ?? author.Email!,
                            PublishedAt = DateTime.UtcNow
                        },
                        new Article
                        {
                            Title = "Entity Framework Tips",
                            Summary = "A quick guide for EF Core beginners.",
                            Content = "Here are some practical tips for working with EF Core...",
                            Slug = "ef-core-tips",
                            AuthorId = author.Id,
                            AuthorName = author.UserName ?? author.Email!,
                            PublishedAt = DateTime.UtcNow.AddDays(-1)
                        },
                        new Article
                        {
                            Title = "Christiano Ronaldo palying with al nasr",
                            Summary = "Al-Nasr Club paid for Christiano",
                            Content = "Al-Nasr Club paid for Christiano Ronaldo a lot of money to play with them...",
                            Slug = "ef-core-tips",
                            AuthorId = author.Id,
                            AuthorName = author.UserName ?? author.Email!,
                            PublishedAt = DateTime.UtcNow.AddDays(-8)
                        },
                        new Article
                        {
                            Title = "Football Tips",
                            Summary = "A quick guide for football beginners.",
                            Content = "Here are some practical tips for playing football...",
                            Slug = "foot-ball-tips",
                            AuthorId = author.Id,
                            AuthorName = author.UserName ?? author.Email!,
                            PublishedAt = DateTime.UtcNow.AddDays(-2)
                        },
                        new Article
                        {
                            Title = "Cristians and what they believe",
                            Summary = "A quick guide for EF Core beginners.",
                            Content = "Here are some who practice there religion...",
                            Slug = "Cristians-core-thrinity",
                            AuthorId = author.Id,
                            AuthorName = author.UserName ?? author.Email!,
                            PublishedAt = DateTime.UtcNow.AddDays(-1)
                        }
                    };

                    await newsContext.Articles.AddRangeAsync(articles);
                    await newsContext.SaveChangesAsync();
                }
            }
        }

        public async Task InitailizeIdentityAsync()
        {
            // Remove UserRoles (link table) first due to FK constraints
            //identityDbContext.UserRoles.RemoveRange(identityDbContext.UserRoles);

            //// Remove Users and Roles (order matters due to FK constraints)
            //identityDbContext.Users.RemoveRange(identityDbContext.Users);
            //identityDbContext.Roles.RemoveRange(identityDbContext.Roles);
            //await identityDbContext.SaveChangesAsync();
            if (!roleManager.Roles.Any())
            {
                await roleManager.CreateAsync(new IdentityRole("Admin"));
                await roleManager.CreateAsync(new IdentityRole("User"));
                await roleManager.CreateAsync(new IdentityRole("SuperAdmin"));
            }
            if (!userManager.Users.Any())
            {
                var superAdminUser = new ApplicationUser() { DisplayName = "Super Admin User", Email = "superadmin@super.com", PhoneNumber = "0123456789", UserName = "SA12", NationalId = "123" };
                var adminUser = new ApplicationUser() { DisplayName = "Super Admin User", Email = "admin@admin.com", PhoneNumber = "0123456789", UserName = "A-12", NationalId = "321" };
                await userManager.CreateAsync(adminUser, "password");
                await userManager.CreateAsync(superAdminUser, "password");
                await userManager.AddToRoleAsync(adminUser, "Admin");
                await userManager.AddToRoleAsync(superAdminUser, "SuperAdmin");
            }
        }
    }
}
