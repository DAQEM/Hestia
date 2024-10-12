using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hestia.API.Migrations
{
    /// <inheritdoc />
    public partial class RemovedBlogIsFeatured : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsFeatured",
                table: "Blogs");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsFeatured",
                table: "Blogs",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }
    }
}
