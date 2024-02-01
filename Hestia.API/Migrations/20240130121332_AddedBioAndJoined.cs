using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hestia.API.Migrations
{
    /// <inheritdoc />
    public partial class AddedBioAndJoined : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Bio",
                table: "Users",
                type: "longtext",
                nullable: false);

            migrationBuilder.AddColumn<long>(
                name: "Joined",
                table: "Users",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "LastActive",
                table: "Users",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Bio",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Joined",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "LastActive",
                table: "Users");
        }
    }
}
