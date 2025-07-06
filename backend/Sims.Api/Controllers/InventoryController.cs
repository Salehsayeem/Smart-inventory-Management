using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sims.Api.Dto.Location;
using Sims.Api.Dto;
using Sims.Api.Dto.Inventory;
using Sims.Api.IRepositories;
using Sims.Api.Repositories;

namespace Sims.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InventoryController : ControllerBase
    
    {
        private readonly IInventoryRepository _inventoryRepository;

        public InventoryController(IInventoryRepository inventoryRepository)
        {
            _inventoryRepository = inventoryRepository;
        }

        [HttpPost("CreateOrUpdateInventory")]
        public async Task<CommonResponseDto> CreateOrUpdateWarehouse([FromBody] CreateOrUpdateInventoryDto model)
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

                return await _inventoryRepository.CreateOrUpdateInventory(model, currentUserId);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        [HttpGet("GetInventoryById")]
        public async Task<CommonResponseDto> GetInventoryById(long inventoryId)
        {
            try
            {
                var data =  await _inventoryRepository.GetInventoryById(inventoryId);
                return new CommonResponseDto() {
                    Data = data,
                    StatusCode = 200
                };
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        [HttpGet("GetAllInventoryDetailsPagination")]
        public async Task<CommonResponseDto> GetAllInventoryDetailsPagination(string? search, long shopId, long locationId, int pageNo, int pageSize)
        {
            try
            {
                var data = await _inventoryRepository.GetAllInventoryDetailsPagination(search, shopId, locationId, pageNo, pageSize);
                return new CommonResponseDto()
                {
                    Data = data,
                    StatusCode = 200
                };
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        [HttpDelete("DeleteInventory")]
        public async Task<CommonResponseDto> DeleteInventory(long inventoryId)
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

                return await _inventoryRepository.DeleteInventory(inventoryId, currentUserId);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}
