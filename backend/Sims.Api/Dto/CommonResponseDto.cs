using System.Text.Json;

namespace Sims.Api.Dto
{
    public class CommonResponseDto
    {
        public int StatusCode { get; set; }
        public string Message { get; set; } = string.Empty;
        public dynamic? Data { get; set; } = null;
        public override string ToString()
        {
            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };

            return JsonSerializer.Serialize(this, options);
        }
    }
    public class PaginationDto<T>
    {
        public List<T> Response { get; set; } = new List<T>();
        public long CurrentPage { get; set; }
        public long PageSize { get; set; }
        public long TotalCount { get; set; }
    }
}
