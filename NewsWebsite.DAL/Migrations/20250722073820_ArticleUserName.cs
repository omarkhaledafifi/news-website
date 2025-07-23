using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NewsWebsite.DAL.Migrations
{
    /// <inheritdoc />
    public partial class ArticleUserName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Author",
                table: "Articles",
                newName: "AuthorName");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "AuthorName",
                table: "Articles",
                newName: "Author");
        }
    }
}
