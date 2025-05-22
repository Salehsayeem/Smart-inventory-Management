using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Sims.Api.Dto;
using Sims.Api.Dto.Shop;
using Sims.Api.IRepositories;

namespace Sims.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShopController : ControllerBase
    {
        private readonly IShopRepository _shopRepository;
        public ShopController(IShopRepository shopRepository)
        {
            _shopRepository = shopRepository;
        }

        [HttpPost("CreateOrUpdateShop")]
        public async Task<CommonResponseDto> CreateOrUpdateShop([FromBody] CreateOrUpdateShopDto model)
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
                return await _shopRepository.CreateOrUpdateShop(model,currentUserId);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        [HttpPost("GetShopById")]
        public async Task<CommonResponseDto> GetShopById([FromBody] GetShopByIdDto model)
        {
            try
            {
                return await _shopRepository.GetShopById(model);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        [HttpPost("DeleteShop")]
        public async Task<CommonResponseDto> DeleteShop([FromBody] long shopId)
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
                return await _shopRepository.DeleteShop(shopId, currentUserId);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}
