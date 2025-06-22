using Newtonsoft.Json;
using Npgsql;
using Sims.Api.Dto;
using Sims.Api.Helper;
using System.Data;
using Dapper;

namespace Sims.Api.StoredProcedure
{
    public class CallStoredProcedure(AppSettings appSettings)
    {
        private readonly AppSettings _appSettings = appSettings ?? throw new ArgumentNullException(nameof(appSettings));

        public async Task<TResult> CallFunctionAsync<TResult>(string functionName, object parameters = null)
        {
            if (string.IsNullOrWhiteSpace(functionName))
                throw new ArgumentException("Function name cannot be empty", nameof(functionName));

            if (string.IsNullOrWhiteSpace(_appSettings.ConnectionString))
                throw new InvalidOperationException("Connection string is not configured");

            try
            {
                using (var connection = new NpgsqlConnection(_appSettings.ConnectionString))
                {
                    await connection.OpenAsync();

                    // Construct the SQL query with schema
                    var paramNames = parameters != null
                        ? string.Join(", ", parameters.GetType().GetProperties().Select((p, i) => $"@p{i}"))
                        : string.Empty;

                    var sql = $"SELECT * FROM public.{functionName}({paramNames})";

                    // Execute query and get JSON result
                    var jsonResult = await connection.QuerySingleAsync<string>(
                        sql,
                        parameters,
                        commandType: System.Data.CommandType.Text
                    );

                    // Deserialize JSON to the requested type
                    return JsonConvert.DeserializeObject<TResult>(jsonResult ?? "[]")
                        ?? (TResult)Activator.CreateInstance(typeof(TResult));
                }
            }
            catch (NpgsqlException ex)
            {
                // In a real app, use a proper logging framework
                Console.Error.WriteLine($"Database error calling {functionName}: {ex.Message}");
                throw new ApplicationException($"Failed to execute function {functionName}", ex);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Unexpected error calling {functionName}: {ex.Message}");
                throw;
            }
        }

        public async Task<PaginationDto<TData>> CallPagedFunctionAsync<TData>(
            string functionName,
            object parameters = null,
            int pageNumber = 1,
            int pageSize = 10)
        {
            if (string.IsNullOrWhiteSpace(functionName))
                throw new ArgumentException("Function name cannot be empty", nameof(functionName));
            if (pageNumber < 1)
                throw new ArgumentException("Page number must be at least 1", nameof(pageNumber));
            if (pageSize < 1)
                throw new ArgumentException("Page size must be at least 1", nameof(pageSize));
            if (string.IsNullOrWhiteSpace(_appSettings.ConnectionString))
                throw new InvalidOperationException("Connection string is not configured");

            try
            {
                using (var connection = new NpgsqlConnection(_appSettings.ConnectionString))
                {
                    await connection.OpenAsync();

                    var paramNames = parameters != null
                        ? string.Join(", ", parameters.GetType().GetProperties().Select((p, i) => $"@p{i}"))
                        : string.Empty;

                    var sql = $"SELECT * FROM public.{functionName}({paramNames})";

                    var result = await connection.QuerySingleAsync(
                        sql,
                        parameters,
                        commandType: System.Data.CommandType.Text
                    );

                    var jsonData = result.result_data as string;
                    var totalCount = Convert.ToInt32(result.total_count);

                    var data = JsonConvert.DeserializeObject<List<TData>>(jsonData ?? "[]")
                        ?? new List<TData>();

                    // Add Sl to each item if TData has an Sl property
                    if (typeof(TData).GetProperty("Sl") != null)
                    {
                        for (int i = 0; i < data.Count; i++)
                        {
                            var item = data[i];
                            var sl = (pageNumber - 1) * pageSize + i + 1;
                            typeof(TData).GetProperty("Sl")?.SetValue(item, sl);
                        }
                    }

                    return new PaginationDto<TData>
                    {
                        Response = data,
                        CurrentPage = pageNumber,
                        PageSize = pageSize,
                        TotalCount = totalCount
                    };
                }
            }
            catch (NpgsqlException ex)
            {
                Console.Error.WriteLine($"Database error calling {functionName}: {ex.Message}");
                throw new ApplicationException($"Failed to execute function {functionName}", ex);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Unexpected error calling {functionName}: {ex.Message}");
                throw;
            }
        }
    }
}
