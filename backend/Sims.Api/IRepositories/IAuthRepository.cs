using Sims.Api.Dto;
using Sims.Api.Dto.Auth;
using Sims.Api.Dto.AuthDto;

namespace Sims.Api.IRepositories
{
    public interface IAuthRepository
    {
        public Task<CommonResponseDto> Register(RegistrationDto model);
        public Task<CommonResponseDto> Login(LoginDto model);
        public Task<CommonResponseDto> GetProfile(Ulid userId, Ulid currentUserId);
        public Task<CommonResponseDto> UpdateProfile(UpdateProfileDto model);
        public Task<CommonResponseDto> GetAllUsersByShopId(long shopId);
        public Task<CommonResponseDto> GetPermissionsOfUser(Ulid userId);
        public Task<CommonResponseDto> UpdatePermissionsOfUser(List<UpdatePermissionOfUserDto> model, Ulid userId);
        public CommonResponseDto GetAvailableRoles();
        public Task<CommonResponseDto> CreateUser(CreateUserDto model,Ulid currentUserId);
        public Task<CommonResponseDto> DeleteUser(Ulid userId,Ulid currentUserId);
    }
}
