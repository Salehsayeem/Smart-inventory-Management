using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sims.Api.Migrations
{
    /// <inheritdoc />
    public partial class ModuleIconAdeed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ModuleIcon",
                table: "Modules",
                type: "character varying(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "Id",
                keyValue: 1,
                column: "ModuleIcon",
                value: "bx bx-user");

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "Id",
                keyValue: 2,
                column: "ModuleIcon",
                value: "bx bx-group");

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "Id",
                keyValue: 3,
                column: "ModuleIcon",
                value: "bx bx-box");

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "Id",
                keyValue: 4,
                column: "ModuleIcon",
                value: "bx bx-category");

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "Id",
                keyValue: 5,
                column: "ModuleIcon",
                value: "bx bx-archive");

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "Id",
                keyValue: 6,
                column: "ModuleIcon",
                value: "bx bx-purchase-tag");

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "Id",
                keyValue: 7,
                column: "ModuleIcon",
                value: "bx bx-store");

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "Id",
                keyValue: 8,
                column: "ModuleIcon",
                value: "bx bx-bar-chart");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ModuleIcon",
                table: "Modules");
        }
    }
}
