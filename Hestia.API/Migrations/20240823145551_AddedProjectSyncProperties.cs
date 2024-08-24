using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hestia.API.Migrations
{
    /// <inheritdoc />
    public partial class AddedProjectSyncProperties : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Downloads",
                table: "Projects");

            migrationBuilder.AddColumn<long>(
                name: "CurseForgeDownloads",
                table: "Projects",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "ModrinthDownloads",
                table: "Projects",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<bool>(
                name: "ShouldSync",
                table: "Projects",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "SyncedAt",
                table: "Projects",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CurseForgeDownloads",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "ModrinthDownloads",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "ShouldSync",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "SyncedAt",
                table: "Projects");

            migrationBuilder.AddColumn<long>(
                name: "Downloads",
                table: "Projects",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }
    }
}
