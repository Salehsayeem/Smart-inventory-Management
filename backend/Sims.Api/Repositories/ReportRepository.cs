using Sims.Api.Dto;
using Sims.Api.IRepositories;
using Sims.Api.StoredProcedure;
using static Sims.Api.Helper.CommonHelper;

namespace Sims.Api.Repositories
{
    public class ReportRepository : IReportRepository
    {
        private readonly CallStoredProcedure _spCaller;

        public ReportRepository(CallStoredProcedure spCaller)
        {
            _spCaller = spCaller;
        }

        public async Task<PaginationDto<SalesSummaryLandingDataDto>> SalesSummaryPagination(long shopId, DateOnly fromDate, DateOnly endDate, int pageNo, int pageSize)
        {
            try
            {
                if (shopId <= 0)
                    throw new ArgumentException("Shop ID must be a positive number", nameof(shopId));
                if (pageNo < 1)
                    throw new ArgumentException("Page number must be at least 1", nameof(pageNo));
                if (pageSize < 1)
                    throw new ArgumentException("Page size must be at least 1", nameof(pageSize));

                return await _spCaller.CallPagedFunctionAsync<SalesSummaryLandingDataDto>(
                    StoredProcedureNames.GetAllSalesSummaryPagination,
                    new { p0 = shopId, p2 = fromDate, p3 = endDate, p4 = pageNo, p5 = pageSize },
                    pageNo,
                    pageSize
                );
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<PaginationDto<TopProductsLandingDataDto>> TopProductsPagination(long shopId, DateOnly fromDate, DateOnly endDate, int pageNo, int pageSize)
        {
            try
            {
                if (shopId <= 0)
                    throw new ArgumentException("Shop ID must be a positive number", nameof(shopId));
                if (pageNo < 1)
                    throw new ArgumentException("Page number must be at least 1", nameof(pageNo));
                if (pageSize < 1)
                    throw new ArgumentException("Page size must be at least 1", nameof(pageSize));

                return await _spCaller.CallPagedFunctionAsync<TopProductsLandingDataDto>(
                    StoredProcedureNames.GetAllTopProductsPagination,
                    new { p0 = shopId, p2 = fromDate, p3 = endDate, p4 = pageNo, p5 = pageSize },
                    pageNo,
                    pageSize
                );

            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<PaginationDto<InventoryStatusLandingDataDto>> InventoryStatusPagination(long shopId, int pageNo, int pageSize)
        {
            try
            {
                if (shopId <= 0)
                    throw new ArgumentException("Shop ID must be a positive number", nameof(shopId));
                if (pageNo < 1)
                    throw new ArgumentException("Page number must be at least 1", nameof(pageNo));
                if (pageSize < 1)
                    throw new ArgumentException("Page size must be at least 1", nameof(pageSize));

                return await _spCaller.CallPagedFunctionAsync<InventoryStatusLandingDataDto>(
                    StoredProcedureNames.GetInventoryStatusPagination,
                    new { p0 = shopId,p4 = pageNo, p5 = pageSize },
                    pageNo,
                    pageSize
                );

            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<PaginationDto<LowStockProductsLandingDataDto>> LowStockProductsPagination(long shopId, int pageNo, int pageSize)
        {
            try
            {
                if (shopId <= 0)
                    throw new ArgumentException("Shop ID must be a positive number", nameof(shopId));
                if (pageNo < 1)
                    throw new ArgumentException("Page number must be at least 1", nameof(pageNo));
                if (pageSize < 1)
                    throw new ArgumentException("Page size must be at least 1", nameof(pageSize));

                return await _spCaller.CallPagedFunctionAsync<LowStockProductsLandingDataDto>(
                    StoredProcedureNames.GetLowStockProductsPagination,
                    new { p0 = shopId, p4 = pageNo, p5 = pageSize },
                    pageNo,
                    pageSize
                );

            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<PaginationDto<SupplierPerformanceLandingDataDto>> SupplierPerformancePagination(long shopId, int pageNo, int pageSize)
        {
            try
            {
                if (shopId <= 0)
                    throw new ArgumentException("Shop ID must be a positive number", nameof(shopId));
                if (pageNo < 1)
                    throw new ArgumentException("Page number must be at least 1", nameof(pageNo));
                if (pageSize < 1)
                    throw new ArgumentException("Page size must be at least 1", nameof(pageSize));

                return await _spCaller.CallPagedFunctionAsync<SupplierPerformanceLandingDataDto>(
                    StoredProcedureNames.GetSupplierPerformancePagination,
                    new { p0 = shopId, p4 = pageNo, p5 = pageSize },
                    pageNo,
                    pageSize
                );

            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<PaginationDto<StockMovementHistoryLandingDataDto>> StockMovementHistoryPagination(long shopId, DateOnly fromDate, DateOnly endDate, int pageNo, int pageSize)
        {
            try
            {
                if (shopId <= 0)
                    throw new ArgumentException("Shop ID must be a positive number", nameof(shopId));
                if (pageNo < 1)
                    throw new ArgumentException("Page number must be at least 1", nameof(pageNo));
                if (pageSize < 1)
                    throw new ArgumentException("Page size must be at least 1", nameof(pageSize));

                return await _spCaller.CallPagedFunctionAsync<StockMovementHistoryLandingDataDto>(
                    StoredProcedureNames.GetStockMovementHistoryPagination ,
                    new { p0 = shopId, p2 = fromDate, p3 = endDate, p4 = pageNo, p5 = pageSize },
                    pageNo,
                    pageSize
                );

            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}
