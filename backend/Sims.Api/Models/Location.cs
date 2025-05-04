using System.ComponentModel.DataAnnotations;
using Sims.Api.Models.Base;

namespace Sims.Api.Models
{
    public class Location : BaseModel
    {
        [Key]
        public Ulid Id { get; set; }

        [Required]
        public long ShopId { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        public string? Address { get; set; }
    }
}
