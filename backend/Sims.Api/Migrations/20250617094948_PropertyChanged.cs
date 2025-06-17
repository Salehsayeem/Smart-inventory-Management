using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Sims.Api.Migrations
{
    public partial class PropertyChanged : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Create a sequence for generating new IDs
            migrationBuilder.Sql("CREATE SEQUENCE IF NOT EXISTS global_id_seq;");

            // Step 1: Add temporary NewId columns and update foreign keys for each table
            var tablesWithNewId = new[]
            {
                ("Categories", "Id"),
                ("Supplier", "Id"),
                ("Products", "Id"),
                ("PurchaseOrders", "Id"),
                ("PurchaseOrderItems", "Id"),
                ("StockMovements", "Id"),
                ("Locations", "Id"),
                ("Inventories", "Id"),
                ("ForecastData", "Id")
            };

            foreach (var (table, idColumn) in tablesWithNewId)
            {
                // Add NewId column
                migrationBuilder.AddColumn<long>(
                    name: "NewId",
                    table: table,
                    type: "bigint",
                    nullable: true);

                // Populate NewId with sequential values
                migrationBuilder.Sql($@"
                    UPDATE ""{table}""
                    SET ""NewId"" = nextval('global_id_seq');
                ");

                // Drop primary key constraint
                migrationBuilder.DropPrimaryKey(
                    name: $"PK_{table}",
                    table: table);
            }

            // Step 2: Update foreign key columns to use new IDs
            // Categories.CategoryId in Products
            migrationBuilder.AddColumn<long>(
                name: "NewCategoryId",
                table: "Products",
                type: "bigint",
                nullable: true);

            migrationBuilder.Sql(@"
                UPDATE ""Products""
                SET ""NewCategoryId"" = (SELECT ""NewId"" FROM ""Categories"" WHERE ""Categories"".""Id"" = ""Products"".""CategoryId"");
            ");

            migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "Products");

            migrationBuilder.RenameColumn(
                name: "NewCategoryId",
                table: "Products",
                newName: "CategoryId");

            migrationBuilder.AlterColumn<long>(
                name: "CategoryId",
                table: "Products",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldNullable: true);

            // Supplier.SupplierId in PurchaseOrders
            migrationBuilder.AddColumn<long>(
                name: "NewSupplierId",
                table: "PurchaseOrders",
                type: "bigint",
                nullable: true);

            migrationBuilder.Sql(@"
                UPDATE ""PurchaseOrders""
                SET ""NewSupplierId"" = (SELECT ""NewId"" FROM ""Supplier"" WHERE ""Supplier"".""Id"" = ""PurchaseOrders"".""SupplierId"");
            ");

            migrationBuilder.DropColumn(
                name: "SupplierId",
                table: "PurchaseOrders");

            migrationBuilder.RenameColumn(
                name: "NewSupplierId",
                table: "PurchaseOrders",
                newName: "SupplierId");

            // PurchaseOrders.PurchaseOrderId in PurchaseOrderItems
            migrationBuilder.AddColumn<long>(
                name: "NewPurchaseOrderId",
                table: "PurchaseOrderItems",
                type: "bigint",
                nullable: true);

            migrationBuilder.Sql(@"
                UPDATE ""PurchaseOrderItems""
                SET ""NewPurchaseOrderId"" = (SELECT ""NewId"" FROM ""PurchaseOrders"" WHERE ""PurchaseOrders"".""Id"" = ""PurchaseOrderItems"".""PurchaseOrderId"");
            ");

            migrationBuilder.DropColumn(
                name: "PurchaseOrderId",
                table: "PurchaseOrderItems");

            migrationBuilder.RenameColumn(
                name: "NewPurchaseOrderId",
                table: "PurchaseOrderItems",
                newName: "PurchaseOrderId");

            // Products.ProductId in PurchaseOrderItems, StockMovements, Inventories
            foreach (var table in new[] { "PurchaseOrderItems", "StockMovements", "Inventories" })
            {
                migrationBuilder.AddColumn<long>(
                    name: "NewProductId",
                    table: table,
                    type: "bigint",
                    nullable: true);

                migrationBuilder.Sql($@"
                    UPDATE ""{table}""
                    SET ""NewProductId"" = (SELECT ""NewId"" FROM ""Products"" WHERE ""Products"".""Id"" = ""{table}"".""ProductId"");
                ");

                migrationBuilder.DropColumn(
                    name: "ProductId",
                    table: table);

                migrationBuilder.RenameColumn(
                    name: "NewProductId",
                    table: table,
                    newName: "ProductId");
            }

            // Locations.LocationId in StockMovements, Inventories, ForecastData
            foreach (var table in new[] { "StockMovements", "Inventories", "ForecastData" })
            {
                migrationBuilder.AddColumn<long>(
                    name: "NewLocationId",
                    table: table,
                    type: "bigint",
                    nullable: true);

                migrationBuilder.Sql($@"
                    UPDATE ""{table}""
                    SET ""NewLocationId"" = (SELECT ""NewId"" FROM ""Locations"" WHERE ""Locations"".""Id"" = ""{table}"".""LocationId"");
                ");

                migrationBuilder.DropColumn(
                    name: "LocationId",
                    table: table);

                migrationBuilder.RenameColumn(
                    name: "NewLocationId",
                    table: table,
                    newName: "LocationId");
            }

            // Step 3: Finalize primary key columns
            foreach (var (table, idColumn) in tablesWithNewId)
            {
                // Drop old Id column
                migrationBuilder.DropColumn(
                    name: idColumn,
                    table: table);

                // Rename NewId to Id
                migrationBuilder.RenameColumn(
                    name: "NewId",
                    table: table,
                    newName: idColumn);

                // Make Id non-nullable and set identity
                migrationBuilder.AlterColumn<long>(
                    name: idColumn,
                    table: table,
                    type: "bigint",
                    nullable: false,
                    oldNullable: true)
                    .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                // Reapply primary key
                migrationBuilder.AddPrimaryKey(
                    name: $"PK_{table}",
                    table: table,
                    column: idColumn);
            }

            // Step 4: Reapply foreign key constraints
            migrationBuilder.AddForeignKey(
                name: "FK_Products_Categories_CategoryId",
                table: "Products",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PurchaseOrders_Supplier_SupplierId",
                table: "PurchaseOrders",
                column: "SupplierId",
                principalTable: "Supplier",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PurchaseOrderItems_PurchaseOrders_PurchaseOrderId",
                table: "PurchaseOrderItems",
                column: "PurchaseOrderId",
                principalTable: "PurchaseOrders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PurchaseOrderItems_Products_ProductId",
                table: "PurchaseOrderItems",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StockMovements_Products_ProductId",
                table: "StockMovements",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StockMovements_Locations_LocationId",
                table: "StockMovements",
                column: "LocationId",
                principalTable: "Locations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Inventories_Products_ProductId",
                table: "Inventories",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Inventories_Locations_LocationId",
                table: "Inventories",
                column: "LocationId",
                principalTable: "Locations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ForecastData_Locations_LocationId",
                table: "ForecastData",
                column: "LocationId",
                principalTable: "Locations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            // Drop the sequence
            migrationBuilder.Sql("DROP SEQUENCE IF EXISTS global_id_seq;");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Reverse the migration (restore text/Ulid IDs)
            var tablesWithId = new[]
            {
                ("Categories", "Id"),
                ("Supplier", "Id"),
                ("Products", "Id"),
                ("PurchaseOrders", "Id"),
                ("PurchaseOrderItems", "Id"),
                ("StockMovements", "Id"),
                ("Locations", "Id"),
                ("Inventories", "Id"),
                ("ForecastData", "Id")
            };

            // Drop foreign keys
            migrationBuilder.DropForeignKey(name: "FK_Products_Categories_CategoryId", table: "Products");
            migrationBuilder.DropForeignKey(name: "FK_PurchaseOrders_Supplier_SupplierId", table: "PurchaseOrders");
            migrationBuilder.DropForeignKey(name: "FK_PurchaseOrderItems_PurchaseOrders_PurchaseOrderId", table: "PurchaseOrderItems");
            migrationBuilder.DropForeignKey(name: "FK_PurchaseOrderItems_Products_ProductId", table: "PurchaseOrderItems");
            migrationBuilder.DropForeignKey(name: "FK_StockMovements_Products_ProductId", table: "StockMovements");
            migrationBuilder.DropForeignKey(name: "FK_StockMovements_Locations_LocationId", table: "StockMovements");
            migrationBuilder.DropForeignKey(name: "FK_Inventories_Products_ProductId", table: "Inventories");
            migrationBuilder.DropForeignKey(name: "FK_Inventories_Locations_LocationId", table: "Inventories");
            migrationBuilder.DropForeignKey(name: "FK_ForecastData_Locations_LocationId", table: "ForecastData");

            // Add temporary columns for old IDs
            foreach (var (table, idColumn) in tablesWithId)
            {
                migrationBuilder.AddColumn<string>(
                    name: "OldId",
                    table: table,
                    type: "text",
                    nullable: true);

                migrationBuilder.Sql($@"
                    UPDATE ""{table}""
                    SET ""OldId"" = gen_random_uuid()::text;
                ");

                migrationBuilder.DropPrimaryKey(
                    name: $"PK_{table}",
                    table: table);

                migrationBuilder.DropColumn(
                    name: idColumn,
                    table: table);

                migrationBuilder.RenameColumn(
                    name: "OldId",
                    table: table,
                    newName: idColumn);

                migrationBuilder.AlterColumn<string>(
                    name: idColumn,
                    table: table,
                    type: "text",
                    nullable: false,
                    oldNullable: true);

                migrationBuilder.AddPrimaryKey(
                    name: $"PK_{table}",
                    table: table,
                    column: idColumn);
            }

            // Restore foreign keys
            migrationBuilder.AddColumn<string>(
                name: "OldCategoryId",
                table: "Products",
                type: "text",
                nullable: true);

            migrationBuilder.Sql(@"
                UPDATE ""Products""
                SET ""OldCategoryId"" = gen_random_uuid()::text;
            ");

            migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "Products");

            migrationBuilder.RenameColumn(
                name: "OldCategoryId",
                table: "Products",
                newName: "CategoryId");

            foreach (var table in new[] { "PurchaseOrders", "PurchaseOrderItems", "StockMovements", "Inventories", "ForecastData" })
            {
                var fkColumns = table switch
                {
                    "PurchaseOrders" => new[] { ("SupplierId", "Supplier") },
                    "PurchaseOrderItems" => new[] { ("PurchaseOrderId", "PurchaseOrders"), ("ProductId", "Products") },
                    "StockMovements" => new[] { ("ProductId", "Products"), ("LocationId", "Locations") },
                    "Inventories" => new[] { ("ProductId", "Products"), ("LocationId", "Locations") },
                    "ForecastData" => new[] { ("LocationId", "Locations") },
                    _ => new (string, string)[0]
                };

                foreach (var (fkColumn, refTable) in fkColumns)
                {
                    migrationBuilder.AddColumn<string>(
                        name: $"Old{fkColumn}",
                        table: table,
                        type: "text",
                        nullable: true);

                    migrationBuilder.Sql($@"
                        UPDATE ""{table}""
                        SET ""Old{fkColumn}"" = gen_random_uuid()::text;
                    ");

                    migrationBuilder.DropColumn(
                        name: fkColumn,
                        table: table);

                    migrationBuilder.RenameColumn(
                        name: $"Old{fkColumn}",
                        table: table,
                        newName: fkColumn);
                }
            }

            // Reapply foreign keys
            migrationBuilder.AddForeignKey(
                name: "FK_Products_Categories_CategoryId",
                table: "Products",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_PurchaseOrders_Supplier_SupplierId",
                table: "PurchaseOrders",
                column: "SupplierId",
                principalTable: "Supplier",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PurchaseOrderItems_PurchaseOrders_PurchaseOrderId",
                table: "PurchaseOrderItems",
                column: "PurchaseOrderId",
                principalTable: "PurchaseOrders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PurchaseOrderItems_Products_ProductId",
                table: "PurchaseOrderItems",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StockMovements_Products_ProductId",
                table: "StockMovements",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StockMovements_Locations_LocationId",
                table: "StockMovements",
                column: "LocationId",
                principalTable: "Locations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Inventories_Products_ProductId",
                table: "Inventories",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Inventories_Locations_LocationId",
                table: "Inventories",
                column: "LocationId",
                principalTable: "Locations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ForecastData_Locations_LocationId",
                table: "ForecastData",
                column: "LocationId",
                principalTable: "Locations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}