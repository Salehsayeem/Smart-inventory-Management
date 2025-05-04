using System.ComponentModel.DataAnnotations;
using Sims.Api.Models.Base;

namespace Sims.Api.Models
{
    public class User
    {
        [Key]
        public Ulid Id { get; set; }

        [StringLength(100)]
        public string FullName { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string PasswordHash { get; set; } = string.Empty;

        public int RoleId { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public Ulid? ModifiedBy { get; set; }
        public DateTime? ModifiedAt { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
