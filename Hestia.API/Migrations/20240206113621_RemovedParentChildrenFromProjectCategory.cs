using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hestia.API.Migrations
{
    /// <inheritdoc />
    public partial class RemovedParentChildrenFromProjectCategory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProjectCategories_ProjectCategories_ParentId",
                table: "ProjectCategories");

            migrationBuilder.DropIndex(
                name: "IX_ProjectCategories_ParentId",
                table: "ProjectCategories");

            migrationBuilder.DropColumn(
                name: "ParentId",
                table: "ProjectCategories");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ParentId",
                table: "ProjectCategories",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProjectCategories_ParentId",
                table: "ProjectCategories",
                column: "ParentId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectCategories_ProjectCategories_ParentId",
                table: "ProjectCategories",
                column: "ParentId",
                principalTable: "ProjectCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
