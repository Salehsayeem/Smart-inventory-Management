using Sims.Api.Models.Base;
using System.ComponentModel.DataAnnotations;

namespace Sims.Api.Models
{
    public class StockMovement : BaseModel
    {
        [Key]
        public Ulid Id { get; set; }

        [Required]
        public long ShopId { get; set; }

        [Required]
        public string ProductId { get; set; } = string.Empty;

        [Required]
        public string LocationId { get; set; } = string.Empty;

        [Required]
        public int QuantityChange { get; set; }

        [StringLength(255)]
        public string? Reason { get; set; }
    }
}
