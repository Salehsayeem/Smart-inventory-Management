using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sims.Api.Dto;
using Sims.Api.IRepositories;

namespace Sims.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SalesController : ControllerBase
    {
        private readonly ISaleRepository _repository;
        public SalesController(ISaleRepository repository)
        {
            _repository = repository;
        }
        [HttpPost("CreateSale")]
        public async Task<CommonResponseDto> CreateSale([FromBody] CreateSaleDto model)
        {
            try
            {
                var currentUserId = Ulid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
                if (currentUserId == Ulid.Empty)
                {
                    return new CommonResponseDto()
                    {
                        Message = "Unauthorized: Please log in!",
                        Data = null,
                        StatusCode = 403,
                    };
                }
                return await _repository.CreateSale(model, currentUserId);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        [HttpGet("GetAllSales")]
        public async Task<PaginationDto<SaleLandingDataDto>> GetAllSales(string? search, long shopId, DateOnly fromDate, DateOnly endDate, int pageNo, int pageSize)
        {
            try
            {
                return await _repository.GetAllSales(search, shopId, fromDate, endDate, pageNo, pageSize);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        [HttpGet("GetSaleById")]
        public async Task<GetSaleDto> GetSaleById(long id)
        {
            try
            {
                return await _repository.GetSaleById(id);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        [HttpPut("UpdateSaleItems")]
        public async Task<CommonResponseDto> UpdateSaleItems([FromBody] List<CreateSaleItemDto> items)
        {
            try
            {
                var currentUserId = Ulid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
                if (currentUserId == Ulid.Empty)
                {
                    return new CommonResponseDto()
                    {
                        Message = "Unauthorized: Please log in!",
                        Data = null,
                        StatusCode = 403,
                    };
                }
                return await _repository.UpdateSaleItems(items, currentUserId);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}
