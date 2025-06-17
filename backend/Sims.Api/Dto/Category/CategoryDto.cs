using System.ComponentModel.DataAnnotations;

namespace Sims.Api.Dto.Category
{

    public class CreateOrUpdateCategoryDto
    {
        public string Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public long ShopId { get; set; }
    }
    public class GetCategoryByIdDto
    {
        public Ulid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public long ShopId { get; set; }
    }
    public class CategoryLandingPaginationDto
    {
        public List<CategoryLandingDataDto> Response { get; set; } = default!;
        public long CurrentPage { get; set; } = default!;
        public long TotalCount { get; set; } = default!;
        public long PageSize { get; set; } = default!;
    }

    public class CategoryLandingDataDto
    {
        public int Sl { get; set; }
        public string Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = String.Empty;
    }
}

