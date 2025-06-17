using Sims.Api.Models.Base;
using System.ComponentModel.DataAnnotations;

namespace Sims.Api.Models
{
    public class StockMovement : BaseModel
    {
        [Key]
        public long Id { get; set; }

        [Required]
        public long ShopId { get; set; }

        [Required]
        public long ProductId { get; set; } 

        [Required]
        public long LocationId { get; set; }

        [Required]
        public int QuantityChange { get; set; }

        [StringLength(255)]
        public string? Reason { get; set; }
    }
}
