using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sims.Api.Migrations
{
    /// <inheritdoc />
    public partial class LocationAddedToPo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "LocationId",
                table: "PurchaseOrders",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LocationId",
                table: "PurchaseOrders");
        }
    }
}
