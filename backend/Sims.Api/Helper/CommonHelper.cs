using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Sims.Api.Models;
using Dapper;
using System.Data;

namespace Sims.Api.Helper
{
    public static class CommonHelper
    {
        public static void ApplyCommonConfigurations(ModelBuilder modelBuilder)
        {
            var ulidConverter = new UlidToStringConverter();

            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                var properties = entityType.ClrType.GetProperties();

                // Apply default value configuration for CreatedAt
                if (properties.Any(p => p.Name == "CreatedAt"))
                {
                    modelBuilder.Entity(entityType.ClrType)
                        .Property("CreatedAt")
                        .HasDefaultValueSql("now()");
                }

                foreach (var property in properties.Where(p =>
                             p.PropertyType == typeof(Ulid) || p.PropertyType == typeof(Ulid?)))
                {
                    modelBuilder.Entity(entityType.ClrType)
                        .Property(property.Name)
                        .HasConversion(ulidConverter);
                }
            }
        }

        public static Ulid StringToUlidConverter(string userId)
        {
            try
            {
                if (Ulid.TryParse(userId, out Ulid user))
                {
                    return user;
                }
                else
                {
                    throw new Exception("Invalid User ID format.");
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }

        }

        public class UlidToStringConverter : ValueConverter<Ulid, string>
        {
            public UlidToStringConverter() : base(
                ulid => ulid.ToString(), // Convert Ulid to string
                value => Ulid.Parse(value) // Convert string to Ulid
            )
            {
            }
        }
        public class SqlDateOnlyTypeHandler : SqlMapper.TypeHandler<DateOnly>
        {
            public override void SetValue(IDbDataParameter parameter, DateOnly date)
            {
                parameter.Value = date.ToDateTime(TimeOnly.MinValue);
                parameter.DbType = DbType.Date;
            }

            public override DateOnly Parse(object value)
                => DateOnly.FromDateTime((DateTime)value);
        }
        public static class StoredProcedureNames
        {
            public static readonly string GetCategoryPagination = "fn_get_categories_pagination";
            public static readonly string GetAllProductsPagination = "fn_get_all_products_pagination";
            public static readonly string GetAllProductsByCategoryPagination = "fn_get_all_products_by_category_pagination";
            public static readonly string GetAllLocationsPagination = "fn_get_all_locations_pagination";
            public static readonly string GetCategoryListOfShopDdl = "ddl_category_list_of_shop";
            public static readonly string GetProductListOfShopDdl = "ddl_product_list_of_shop";
            public static readonly string GetWarehouseListDdl = "ddl_warehouse_list_of_shop";
            public static readonly string GetAllInventoryDetailsPagination = "fn_get_all_inventory_details_pagination";
            public static readonly string GetAllSuppliersPagination = "fn_get_all_suppliers_pagination";
            public static readonly string GetAllPurchaseOrdersPagination = "fn_get_all_purchase_orders_pagination";
            public static readonly string GetAllSaleOrdersPagination = "fn_get_all_sales_orders_pagination";
            public static readonly string GetAllSalesSummaryPagination = "fn_get_sales_summary_pagination";
            public static readonly string GetAllTopProductsPagination = "fn_get_top_products_pagination";
            public static readonly string GetInventoryStatusPagination = "fn_get_inventory_status_pagination";
            public static readonly string GetLowStockProductsPagination = "fn_get_low_stock_products_pagination";
            public static readonly string GetSupplierPerformancePagination = "fn_get_supplier_performance_pagination";
            public static readonly string GetStockMovementHistoryPagination = "fn_get_stock_movement_history_pagination";

        }
    }
}
