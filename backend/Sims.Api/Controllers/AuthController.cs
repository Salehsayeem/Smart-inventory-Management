using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sims.Api.Dto;
using Sims.Api.Dto.AuthDto;
using Sims.Api.IRepositories;
using System.Security.Claims;
using Sims.Api.Helper;
using Sims.Api.Models;

namespace Sims.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository _authRepository;
        public AuthController(IAuthRepository authRepository)
        {
            _authRepository = authRepository;
        }
        [HttpPost("register")]
        public async Task<CommonResponseDto> Register([FromBody] RegistrationDto model)
        {
            try
            {
                return await _authRepository.Register(model);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        [HttpPost("login")]
        public async Task<CommonResponseDto> Login([FromBody] LoginDto model)
        {
            try
            {
                return await _authRepository.Login(model);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        [Authorize]
        [HttpGet("Profile")]
        public async Task<CommonResponseDto> GetProfile(string userId)
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
                return await _authRepository.GetProfile(user, currentUserId);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        [Authorize]
        [HttpPut("UpdateProfile")]
        public async Task<CommonResponseDto> UpdateProfile([FromBody] UpdateProfileDto model)
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
                if (currentUserId != model.Id)
                {
                    return new CommonResponseDto()
                    {
                        Message = "Unauthorized: You can only edit your own profile",
                        Data = null,
                        StatusCode = 403
                    };
                }
                return await _authRepository.UpdateProfile(model);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        [Authorize]
        [HttpGet("GetAllUsersByShopId")]
        public async Task<CommonResponseDto> GetAllUsersByShopId(long shopId, string userId)
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
                        Message = "Unauthorized: You are not eligible to view the users",
                        Data = null,
                        StatusCode = 403
                    };
                }
                return await _authRepository.GetAllUsersByShopId(shopId, user, currentUserId);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        [Authorize]
        [HttpGet("GetPermissionsOfUser")]
        public async Task<CommonResponseDto> GetPermissionsOfUser(string userId)
        {
            try
            {
                Ulid user = CommonHelper.StringToUlidConverter(userId);
                var role = (RoleEnums)Enum.Parse(typeof(RoleEnums), User.FindFirstValue("Role")!);
                if (role != RoleEnums.Admin && role != RoleEnums.SuperAdmin)
                {
                    return new CommonResponseDto()
                    {
                        Message = "Unauthorized: You are not eligible to view the users",
                        Data = null,
                        StatusCode = 403
                    };
                }
                return await _authRepository.GetPermissionsOfUser(user);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

    }
}
