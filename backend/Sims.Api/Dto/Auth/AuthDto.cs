using System.ComponentModel.DataAnnotations;
using Sims.Api.Dto.Auth;

namespace Sims.Api.Dto.AuthDto
{
    public class RegistrationDto
    {
        [Required]
        [StringLength(100)]
        public string Email { get; set; } = string.Empty;
        [Required]
        public string Password { get; set; } = string.Empty;
        public string ConfirmPassword { get; set; } = string.Empty;
        public string BusinessName { get; set; } = string.Empty;
    }
    public class LoginDto
    {
        [Required]
        public string Email { get; set; } = string.Empty;
        [Required]
        public string Password { get; set; } = string.Empty;
    }

    public class GetProfileDto
    {
        public Ulid Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string RoleName { get; set; } = string.Empty;
        public List<RegisteredShopsDto> RegisteredShops { get; set; } = new List<RegisteredShopsDto>();
    }
    
    public class UpdateProfileDto
    {
        public string Id { get; set; }
        public string FullName { get; set; } = string.Empty;
    }
    public class RegisteredShopsDto
    {
        public long Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
    }

    public class CreateUserDto
    {
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public int RoleId { get; set; }
        public long ShopId { get; set; }
    }

   
}
