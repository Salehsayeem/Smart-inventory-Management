using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Sims.Api.Dto.Product
{
    public class CreateOrUpdateProductDto
    {
        public long Id { get; set; }
        public long ShopId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Sku { get; set; } = string.Empty;
        public long CategoryId { get; set; }
        public string? Description { get; set; }
        public decimal UnitPrice { get; set; }
    }
    public class GetProductByIdDto
    {
        public long Id { get; set; }
        public long ShopId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Sku { get; set; } = string.Empty;
        public long CategoryId { get; set; }
        public string? CategoryName { get; set; }
        public string? Description { get; set; }
        public decimal UnitPrice { get; set; }
    }

    public class ProductLandingDataDto
    {
        public int Sl { get; set; }
        public long Id { get; set; }
        public string Name { get; set; } = null!;
        public string Sku { get; set; } = string.Empty;
        public string Description { get; set; } = String.Empty;
        public string? CategoryName { get; set; }
        public decimal UnitPrice { get; set; }
    }
}
