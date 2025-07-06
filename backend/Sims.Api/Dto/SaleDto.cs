using Sims.Api.Helper;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Sims.Api.Models.Base;

namespace Sims.Api.Dto
{
    public class CreateSaleDto
    {
        public long ShopId { get; set; }
        public long LocationId { get; set; }
        public decimal TotalPrice { get; set; }
        public DateTime SaleDate { get; set; }
        public string Status { get; set; }
        public List<CreateSaleItemDto> Items { get; set; } = new List<CreateSaleItemDto>();
    }

    public class CreateSaleItemDto
    {
        public long Id { get; set; }
        public long ProductId { get; set; }
        public int QuantitySold { get; set; }
        public decimal UnitPrice { get; set; }
    }

    public class SaleLandingDataDto
    {
        public long Id { get; set; }
        public long ShopId { get; set; }
        public string Code { get; set; }
        public string LocationName { get; set; } = string.Empty;
        public DateTime SaleDate { get; set; }
        public string Status { get; set; } = string.Empty;
        public int TotalItems { get; set; }
        public decimal TotalAmount { get; set; }
    }

    public class GetSaleDto
    {
        public long Id { get; set; }
        public long ShopId { get; set; }
        public string Code { get; set; } = string.Empty;
        public string LocationName { get; set; } = string.Empty;
        public DateTime SaleDate { get; set; }
        public List<GetSaleItemsDto> Items { get; set; } = new List<GetSaleItemsDto>();
    }

    public class GetSaleItemsDto
    {
        public long Id { get; set; }
        public long SaleId { get; set; }
        public long ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public int QuantitySold { get; set; }
        public decimal UnitPrice { get; set; }
    }
}
