using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sims.Api.Dto
{
    public class CreatePurchaseDto
    {
        public long ShopId { get; set; }
        public long SupplierId { get; set; }
        public long LocationId { get; set; }
        public DateTime OrderDate { get; set; } = DateTime.UtcNow;
        public List<CreatePurchaseItemDto> Items { get; set; } = new List<CreatePurchaseItemDto>();
    }
    public class CreatePurchaseItemDto
    {
        public long Id { get; set; }
        public long ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
    }

    public class PurchaseOrderLandingDataDto
    {
        public long Id { get; set; }
        public long ShopId { get; set; }
        public string Code { get; set; }
        public string LocationName { get; set; }
        public string SupplierDetails { get; set; } = null!;
        public DateTime OrderDate { get; set; } = DateTime.UtcNow;
        public string Status { get; set; }
        public int TotalItems { get; set; }
        public decimal TotalAmount { get; set; }
    }
    public class GetPurchaseOrderDto
    {
        public long Id { get; set; }
        public long ShopId { get; set; }
        public string Code { get; set; }
        public string LocationName { get; set; } = string.Empty;
        public string SupplierDetails { get; set; } = null!;
        public DateTime OrderDate { get; set; } = DateTime.UtcNow;
        public List<GetPurchaseItemsDto> Items { get; set; } = [];
    }
    public class GetPurchaseItemsDto
    {
        public long Id { get; set; }
        public long PurchaseOrderId { get; set; }
        public long ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
    }
}
