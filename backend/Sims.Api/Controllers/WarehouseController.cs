using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sims.Api.Dto;
using Sims.Api.Dto.Location;
using Sims.Api.IRepositories;

namespace Sims.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WarehouseController : ControllerBase
    {
        public readonly ILocationRepository _locationRepository;

        public WarehouseController(ILocationRepository locationRepository)
        {
            _locationRepository = locationRepository;
        }

        [HttpPost("CreateOrUpdateWarehouse")]
        public async Task<CommonResponseDto> CreateOrUpdateWarehouse([FromBody] CreateOrUpdateWarehouseDto model)
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

                return await _locationRepository.CreateOrUpdateWarehouse(model, currentUserId);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        [HttpGet("GetWarehouseById")]
        public async Task<CommonResponseDto> GetWarehouseById(long warehouseId)
        {
            try
            {
                return await _locationRepository.GetWarehouseById(warehouseId);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        [HttpGet("GetAllWarehousesByShopId")]
        public async Task<CommonResponseDto> GetAllWarehousesByShopId(string? search, long shopId, int pageNo,
            int pageSize)
        {
            try
            {
                var data = await _locationRepository.GetAllWarehousesByShopId(search, shopId, pageNo, pageSize);
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

        [HttpDelete("DeleteWarehouse")]
        public async Task<CommonResponseDto> DeleteWarehouse(long warehouseId)
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

                return await _locationRepository.DeleteWarehouse(warehouseId, currentUserId);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}
