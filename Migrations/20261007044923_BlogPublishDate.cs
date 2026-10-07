using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Portafolio.Migrations
{
    /// <inheritdoc />
    public partial class BlogPublishDate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Published",
                table: "BlogPosts");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Published",
                table: "BlogPosts",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }
    }
}
