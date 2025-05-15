using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sims.Api.Migrations
{
    /// <inheritdoc />
    public partial class NewDataAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ModuleIcon", "Name", "Path" },
                values: new object[] { "bx bx-dashboard", "Dashboard", "dashboard" });

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "ModuleIcon", "Name", "Path" },
                values: new object[] { "bx bx-user", "Profile", "profile" });

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "ModuleIcon", "Name", "Path" },
                values: new object[] { "bx bx-group", "User Management", "user-management" });

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "ModuleIcon", "Name", "Path" },
                values: new object[] { "bx bx-box", "Product", "product" });

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "ModuleIcon", "Name", "Path" },
                values: new object[] { "bx bx-category", "Category", "category" });

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "ModuleIcon", "Name", "Path" },
                values: new object[] { "bx bx-archive", "Inventory Tracking", "inventory-tracking" });

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "ModuleIcon", "Name", "Path" },
                values: new object[] { "bx bx-purchase-tag", "Purchase Orders", "purchase-orders" });

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "ModuleIcon", "Name", "Path" },
                values: new object[] { "bx bx-store", "Suppliers", "suppliers" });

            migrationBuilder.InsertData(
                table: "Modules",
                columns: new[] { "Id", "ModuleIcon", "Name", "Path" },
                values: new object[] { 9, "bx bx-bar-chart", "Reports & Logs", "reports-logs" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Modules",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ModuleIcon", "Name", "Path" },
                values: new object[] { "bx bx-user", "Profile", "/profile" });

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "ModuleIcon", "Name", "Path" },
                values: new object[] { "bx bx-group", "User Management", "/user-management" });

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "ModuleIcon", "Name", "Path" },
                values: new object[] { "bx bx-box", "Product", "/product" });

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "ModuleIcon", "Name", "Path" },
                values: new object[] { "bx bx-category", "Category", "/category" });

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "ModuleIcon", "Name", "Path" },
                values: new object[] { "bx bx-archive", "Inventory Tracking", "/inventory-tracking" });

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "ModuleIcon", "Name", "Path" },
                values: new object[] { "bx bx-purchase-tag", "Purchase Orders", "/purchase-orders" });

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "ModuleIcon", "Name", "Path" },
                values: new object[] { "bx bx-store", "Suppliers", "/suppliers" });

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "ModuleIcon", "Name", "Path" },
                values: new object[] { "bx bx-bar-chart", "Reports & Logs", "/reports-logs" });
        }
    }
}
