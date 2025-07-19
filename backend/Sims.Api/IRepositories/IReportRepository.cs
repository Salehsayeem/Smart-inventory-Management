using Sims.Api.Dto;

namespace Sims.Api.IRepositories
{
    public interface IReportRepository
    {
        Task<PaginationDto<SalesSummaryLandingDataDto>> SalesSummaryPagination( long shopId, DateOnly fromDate, DateOnly endDate, int pageNo, int pageSize);
        Task<PaginationDto<TopProductsLandingDataDto>> TopProductsPagination( long shopId, DateOnly fromDate, DateOnly endDate, int pageNo, int pageSize);
        Task<PaginationDto<InventoryStatusLandingDataDto>> InventoryStatusPagination( long shopId, int pageNo, int pageSize);
        Task<PaginationDto<LowStockProductsLandingDataDto>> LowStockProductsPagination( long shopId, int pageNo, int pageSize);
        Task<PaginationDto<SupplierPerformanceLandingDataDto>> SupplierPerformancePagination(long shopId, int pageNo, int pageSize);
        Task<PaginationDto<StockMovementHistoryLandingDataDto>> StockMovementHistoryPagination( long shopId, DateOnly fromDate, DateOnly endDate, int pageNo, int pageSize);
    }
}
