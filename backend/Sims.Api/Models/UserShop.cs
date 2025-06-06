using System.ComponentModel.DataAnnotations;
using Sims.Api.Models.Base;

namespace Sims.Api.Models
{
    public class UserShop : BaseModel
    {
        [Key]
        public long Id { get; set; }

        [Required]
        public Ulid UserId { get; set; }

        [Required]
        public long ShopId { get; set; }
        [Required]
        public int RoleId { get; set; }
    }
}
