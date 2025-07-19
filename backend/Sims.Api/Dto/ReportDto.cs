namespace Sims.Api.Dto
{
    public class SalesSummaryLandingDataDto
    {
        public long ProductId { get; set; }
        public string ProductName { get; set; }
        public int TotalQuantitySold { get; set; }
        public decimal TotalRevenue { get; set; }
    }
    public class TopProductsLandingDataDto
    {
        public long ProductId { get; set; }
        public string ProductName { get; set; }
        public int TotalSold { get; set; }
    }

    public class InventoryStatusLandingDataDto
    {
        public long ProductId { get; set; }
        public string ProductName { get; set; }
        public long LocationId { get; set; }
        public string LocationName { get; set; }
        public int Quantity { get; set; }
    }
    public class LowStockProductsLandingDataDto
    {
        public long ProductId { get; set; }
        public string ProductName { get; set; }
        public long LocationId { get; set; }
        public string LocationName { get; set; }
        public int RestockThreshold { get; set; }
    }
    public class SupplierPerformanceLandingDataDto
    {
        public string SupplierName { get; set; }
        public int TotalOrders { get; set; }
    }

    public class StockMovementHistoryLandingDataDto
    {
        public string ProductName { get; set; }
        public string LocationName { get; set; }
        public int RestockThreshold { get; set; }
    }
}
