using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sims.Api.IRepositories;

namespace Sims.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportController : ControllerBase
    {
        private readonly IReportRepository _repository;

        public ReportController(IReportRepository repository)
        {
            _repository = repository;
        }
        [HttpGet("SalesSummaryPagination")]
        public async Task<IActionResult> SalesSummaryPagination(long shopId, DateOnly fromDate, DateOnly endDate, int pageNo, int pageSize)
        {
            try
            {
                var result = await _repository.SalesSummaryPagination(shopId, fromDate, endDate, pageNo, pageSize);
                return Ok(result);
            }
            catch (Exception e)
            {
                return BadRequest(new { Message = e.Message });
            }
        }
        [HttpGet("TopProductsPagination")]
        public async Task<IActionResult> TopProductsPagination(long shopId, DateOnly fromDate, DateOnly endDate, int pageNo, int pageSize)
        {
            try
            {
                var result = await _repository.TopProductsPagination(shopId, fromDate, endDate, pageNo, pageSize);
                return Ok(result);
            }
            catch (Exception e)
            {
                return BadRequest(new { Message = e.Message });
            }
        }
        [HttpGet("InventoryStatusPagination")]
        public async Task<IActionResult> InventoryStatusPagination(long shopId, int pageNo, int pageSize)
        {
            try
            {
                var result = await _repository.InventoryStatusPagination(shopId, pageNo, pageSize);
                return Ok(result);
            }
            catch (Exception e)
            {
                return BadRequest(new { Message = e.Message });
            }
        }
        [HttpGet("LowStockProductsPagination")]
        public async Task<IActionResult> LowStockProductsPagination(long shopId, int pageNo, int pageSize)
        {
            try
            {
                var result = await _repository.LowStockProductsPagination(shopId, pageNo, pageSize);
                return Ok(result);
            }
            catch (Exception e)
            {
                return BadRequest(new { Message = e.Message });
            }
        }
        [HttpGet("SupplierPerformancePagination")]
        public async Task<IActionResult> SupplierPerformancePagination(long shopId, int pageNo, int pageSize)
        {
            try
            {
                var result = await _repository.SupplierPerformancePagination(shopId, pageNo, pageSize);
                return Ok(result);
            }
            catch (Exception e)
            {
                return BadRequest(new { Message = e.Message });
            }
        }
        [HttpGet("StockMovementHistoryPagination")]
        public async Task<IActionResult> StockMovementHistoryPagination(long shopId, DateOnly fromDate, DateOnly endDate, int pageNo, int pageSize)
        {
            try
            {
                var result = await _repository.StockMovementHistoryPagination(shopId, fromDate, endDate, pageNo, pageSize);
                return Ok(result);
            }
            catch (Exception e)
            {
                return BadRequest(new { Message = e.Message });
            }
        }

    }
}
