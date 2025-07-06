using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sims.Api.Migrations
{
    /// <inheritdoc />
    public partial class CodeAddedPS : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Code",
                table: "Sales",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Code",
                table: "PurchaseOrders",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Code",
                table: "Sales");

            migrationBuilder.DropColumn(
                name: "Code",
                table: "PurchaseOrders");
        }
    }
}
