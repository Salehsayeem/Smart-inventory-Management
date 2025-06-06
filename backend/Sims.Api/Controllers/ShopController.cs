using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Sims.Api.Dto;
using Sims.Api.Dto.Shop;
using Sims.Api.Helper;
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
        [HttpGet("GetShopById")]
        public async Task<CommonResponseDto> GetShopById(string userId, long shopId)
        {
            try
            {
                 Ulid user = CommonHelper.StringToUlidConverter(userId);
                var currentUserId = Ulid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
                var role = (RoleEnums)Enum.Parse(typeof(RoleEnums), User.FindFirstValue("Role")!);

                if (currentUserId == Ulid.Empty)
                {
                    return new CommonResponseDto()
                    {
                        Message = "Unauthorized: Please log in!",
                        Data = null,
                        StatusCode = 403,
                    };
                }

                if (currentUserId != user || (role != RoleEnums.Admin && role != RoleEnums.SuperAdmin))
                {
                    return new CommonResponseDto()
                    {
                        Message = "Unauthorized: You can only view your own profile",
                        Data = null,
                        StatusCode = 403
                    };
                }
                return await _shopRepository.GetShopById(shopId);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        [HttpPut("DeleteShop")]
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
