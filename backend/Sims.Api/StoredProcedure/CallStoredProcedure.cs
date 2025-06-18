using Npgsql;
using System.Data;
using Newtonsoft.Json;
using Sims.Api.Dto;
using Sims.Api.Dto.Category;
using Sims.Api.Dto.Product;
using Sims.Api.Helper;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Sims.Api.StoredProcedure
{
    public class CallStoredProcedure
    {
        public CategoryLandingPaginationDto CategoryLandingPagination(string search, long shopId, int pageNo, int pageSize, string connectionString)
        {
            if (connectionString == null)
            {
                throw new ArgumentNullException(nameof(connectionString), "Connection string cannot be null");
            }

            if (shopId == 0)
            {
                throw new ArgumentException("Shop ID cannot be zero", nameof(shopId));
            }

            using (var connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();
                using (var cmd = new NpgsqlCommand(
                    $"SELECT * FROM {CommonHelper.StoredProcedureNames.GetCategoryPagination}(@search, @shop_id, @page_number, @page_size)",
                    connection))
                {
                    cmd.CommandType = CommandType.Text;

                    cmd.Parameters.AddWithValue("@search", string.IsNullOrEmpty(search) ? DBNull.Value : (object)search);
                    cmd.Parameters.AddWithValue("@shop_id", shopId);
                    cmd.Parameters.AddWithValue("@page_number", pageNo);
                    cmd.Parameters.AddWithValue("@page_size", pageSize);

                    using (var reader = cmd.ExecuteReader())
                    {
                        if (!reader.Read()) return new CategoryLandingPaginationDto();
                        var jsonData = reader.IsDBNull(0) ? "[]" : reader.GetString(0);
                        var totalCount = reader.GetInt32(1);

                        var response = JsonConvert.DeserializeObject<List<CategoryLandingDataDto>>(jsonData)
                                       ?? [];

                        for (int i = 0; i < response.Count; i++)
                        {
                            response[i].Sl = i + 1;
                        }

                        return new CategoryLandingPaginationDto()
                        {
                            Response = response,
                            TotalCount = totalCount,
                            PageSize = pageSize,
                            CurrentPage = pageNo
                        };
                    }
                }
            }
        }

        public ProductLandingPaginationDto AllProductsPagination(string search, long shopId, int pageNo, int pageSize, string connectionString)
        {
            if (connectionString == null)
            {
                throw new ArgumentNullException(nameof(connectionString), "Connection string cannot be null");
            }

            if (shopId == 0)
            {
                throw new ArgumentException("Shop ID cannot be zero", nameof(shopId));
            }

            using (var connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();
                using (var cmd = new NpgsqlCommand(
                           $"SELECT * FROM {CommonHelper.StoredProcedureNames.GetAllProductsPagination}(@search, @shop_id, @page_number, @page_size)",
                           connection))
                {
                    cmd.CommandType = CommandType.Text;

                    cmd.Parameters.AddWithValue("@search", string.IsNullOrEmpty(search) ? DBNull.Value : (object)search);
                    cmd.Parameters.AddWithValue("@shop_id", shopId);
                    cmd.Parameters.AddWithValue("@page_number", pageNo);
                    cmd.Parameters.AddWithValue("@page_size", pageSize);

                    using (var reader = cmd.ExecuteReader())
                    {
                        if (!reader.Read()) return new ProductLandingPaginationDto();
                        var jsonData = reader.IsDBNull(0) ? "[]" : reader.GetString(0);
                        var totalCount = reader.GetInt32(1);

                        var response = JsonConvert.DeserializeObject<List<ProductLandingDataDto>>(jsonData)
                                       ?? [];

                        for (int i = 0; i < response.Count; i++)
                        {
                            response[i].Sl = i + 1;
                        }

                        return new ProductLandingPaginationDto()
                        {
                            Response = response,
                            TotalCount = totalCount,
                            PageSize = pageSize,
                            CurrentPage = pageNo
                        };
                    }
                }
            }
        }

        public ProductLandingPaginationDto AllProductsByCategoryPagination(string search, long shopId,long categoryId, int pageNo, int pageSize, string connectionString)
        {
            if (connectionString == null)
            {
                throw new ArgumentNullException(nameof(connectionString), "Connection string cannot be null");
            }

            if (shopId == 0)
            {
                throw new ArgumentException("Shop ID cannot be zero", nameof(shopId));
            }

            using (var connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();
                using (var cmd = new NpgsqlCommand(
                           $"SELECT * FROM {CommonHelper.StoredProcedureNames.GetAllProductsByCategoryPagination}(@search, @shop_id,@category_id, @page_number, @page_size)",
                           connection))
                {
                    cmd.CommandType = CommandType.Text;

                    cmd.Parameters.AddWithValue("@search", string.IsNullOrEmpty(search) ? DBNull.Value : (object)search);
                    cmd.Parameters.AddWithValue("@shop_id", shopId);
                    cmd.Parameters.AddWithValue("@category_id", categoryId);
                    cmd.Parameters.AddWithValue("@page_number", pageNo);
                    cmd.Parameters.AddWithValue("@page_size", pageSize);

                    using (var reader = cmd.ExecuteReader())
                    {
                        if (!reader.Read()) return new ProductLandingPaginationDto();
                        var jsonData = reader.IsDBNull(0) ? "[]" : reader.GetString(0);
                        var totalCount = reader.GetInt32(1);

                        var response = JsonConvert.DeserializeObject<List<ProductLandingDataDto>>(jsonData)
                                       ?? [];

                        for (int i = 0; i < response.Count; i++)
                        {
                            response[i].Sl = i + 1;
                        }

                        return new ProductLandingPaginationDto()
                        {
                            Response = response,
                            TotalCount = totalCount,
                            PageSize = pageSize,
                            CurrentPage = pageNo
                        };
                    }
                }
            }
        }

        public List<CommonDdlDto> CategoryListOfShopDdl(long shopId, string connectionString)
        {
            if (connectionString == null)
            {
                throw new ArgumentNullException(nameof(connectionString), "Connection string cannot be null");
            }

            if (shopId <= 0)
            {
                throw new ArgumentException("shop ID must be a positive number", nameof(shopId));
            }

            using (var connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();
                using (var cmd = new NpgsqlCommand(
                           $"SELECT * FROM {CommonHelper.StoredProcedureNames.GetCategoryListOfShopDdl}(@shop_id)",
                           connection))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.AddWithValue("@shop_id", shopId);

                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            var jsonData = reader.IsDBNull(0) ? "[]" : reader.GetString(0);
                            return JsonConvert.DeserializeObject<List<CommonDdlDto>>(jsonData)
                                   ?? [];
                        }
                    }
                }
            }

            return new List<CommonDdlDto>();
        }
        public List<CommonDdlDto> ProductListOfShopDdl(long shopId, string connectionString)
        {
            if (connectionString == null)
            {
                throw new ArgumentNullException(nameof(connectionString), "Connection string cannot be null");
            }

            if (shopId <= 0)
            {
                throw new ArgumentException("shop ID must be a positive number", nameof(shopId));
            }

            using (var connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();
                using (var cmd = new NpgsqlCommand(
                           $"SELECT * FROM {CommonHelper.StoredProcedureNames.GetProductListOfShopDdl}(@shop_id)",
                           connection))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.AddWithValue("@shop_id", shopId);

                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            var jsonData = reader.IsDBNull(0) ? "[]" : reader.GetString(0);
                            return JsonConvert.DeserializeObject<List<CommonDdlDto>>(jsonData)
                                   ?? [];
                        }
                    }
                }
            }

            return new List<CommonDdlDto>();
        }
    }
}
