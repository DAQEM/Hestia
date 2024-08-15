using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hestia.API.Migrations
{
    /// <inheritdoc />
    public partial class AddedProjectSlug : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Slug",
                table: "Projects",
                type: "longtext",
                nullable: false);

            migrationBuilder.AlterColumn<int>(
                name: "ParentId",
                table: "ProjectCategories",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Slug",
                table: "Projects");

            migrationBuilder.AlterColumn<int>(
                name: "ParentId",
                table: "ProjectCategories",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);
        }
    }
}
