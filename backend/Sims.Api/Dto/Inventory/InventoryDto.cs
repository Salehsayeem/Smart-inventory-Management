using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sims.Api.Dto.Inventory
{
    public class CreateOrUpdateInventoryDto
    {
        public long Id { get; set; }
        public long ShopId { get; set; }
        public long ProductId { get; set; }
        public long LocationId { get; set; }
        public int Quantity { get; set; }
        public int RestockThreshold { get; set; } 
    }
    public class GetInventoryByIdDto
    {
        public long Id { get; set; }
        public int Quantity { get; set; }
        public int RestockThreshold { get; set; }
    }
    public  class GetInventoryDetailsByLocationIdLandingDto{
        public long Id { get; set; }
        public string LocationName { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public int RestockThreshold { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public string Sku { get; set; } = string.Empty;
        public string CategoryName { get; set; }
        public string? Description { get; set; }
        public decimal UnitPrice { get; set; }
    }
}
