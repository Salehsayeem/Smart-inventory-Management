using Sims.Api.Models.Base;
using System.ComponentModel.DataAnnotations;

namespace Sims.Api.Models
{
    public class Supplier : BaseModel
    {
        [Key]
        public long Id { get; set; }

        [Required]
        public long ShopId { get; set; }

        [Required]
        [StringLength(150)]
        public string Name { get; set; } = string.Empty;

        [StringLength(100)]
        public string? ContactPerson { get; set; }

        [StringLength(20)]
        public string? Phone { get; set; }

        [StringLength(100)]
        public string? Email { get; set; }

        public string? Address { get; set; }
    }
}
