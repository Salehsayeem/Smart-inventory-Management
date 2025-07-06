using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sims.Api.Dto;
using Sims.Api.IRepositories;

namespace Sims.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PurchaseController : ControllerBase
    {
        private readonly IPurchaseRepository _repository;
        public PurchaseController(IPurchaseRepository repository)
        {
            _repository = repository;
        }
        [HttpPost("CreatePurchaseOrder")]
        public async Task<CommonResponseDto> CreatePurchaseOrder([FromBody] CreatePurchaseDto model)
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
                return await _repository.CreatePurchaseOrder(model, currentUserId);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        [HttpGet("GetAllPurchaseOrders")]
        public async Task<PaginationDto<PurchaseOrderLandingDataDto>> GetAllPurchaseOrders(string? search, long shopId, DateOnly fromDate, DateOnly endDate, int pageNo, int pageSize)
        {
            try
            {
                return await _repository.GetAllPurchaseOrders(search,shopId, fromDate, endDate,  pageNo, pageSize);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        [HttpGet("GetPurchaseOrderById")]
        public async Task<GetPurchaseOrderDto> GetPurchaseOrderById(long id)
        {
            try
            {
                return await _repository.GetPurchaseOrderById(id);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        [HttpPost("UpdatePurchaseOrderItems")]
        public async Task<CommonResponseDto> UpdatePurchaseOrderItems([FromBody] List<CreatePurchaseItemDto> items)
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
                return await _repository.UpdatePurchaseOrderItems(items, currentUserId);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        [HttpPost("UpdatePurchaseOrderStatus")]
        public async Task<CommonResponseDto> UpdatePurchaseOrderStatus(long purchaseOrderId, int statusId)
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
                return await _repository.UpdatePurchaseOrderStatus(purchaseOrderId, statusId, currentUserId);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}
