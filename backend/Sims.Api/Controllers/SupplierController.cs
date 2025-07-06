using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sims.Api.Dto;
using Sims.Api.IRepositories;

namespace Sims.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SupplierController : ControllerBase
    {
        private readonly ISupplierRepository _repository;

        public SupplierController(ISupplierRepository repository)
        {
            _repository = repository;
        }

        [HttpPost("CreateOrUpdateSupplier")]
        public async Task<CommonResponseDto> CreateOrUpdateSupplier([FromBody] CreateOrUpdateSupplierDto model)
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
                return await _repository.CreateOrUpdateSupplier(model, currentUserId);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        [HttpGet("GetSupplierById")]
        public async Task<CommonResponseDto> GetSupplierById(long supplierId)
        {
            try
            {
                return await _repository.GetSupplierById(supplierId);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        [HttpGet("GetAllSuppliersByShopId")]
        public async Task<CommonResponseDto> GetAllSuppliersByShopId(string? search, long shopId, int pageNo, int pageSize)
        {
            try
            {
                var data = await _repository.GetAllSupplierPagination(search, shopId, pageNo, pageSize);
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

        [HttpDelete("DeleteSupplier")]
        public async Task<CommonResponseDto> DeleteSupplier(long supplierId)
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

                return await _repository.DeleteSupplier(supplierId, currentUserId);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

    }
}
