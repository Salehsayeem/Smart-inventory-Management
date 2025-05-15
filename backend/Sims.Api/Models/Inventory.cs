using System.ComponentModel.DataAnnotations;
using Sims.Api.Models.Base;

namespace Sims.Api.Models
{
    public class Inventory : BaseModel
    {
        [Key]
        public Ulid Id { get; set; }

        [Required]
        public long ShopId { get; set; }

        [Required]
        public string ProductId { get; set; } = string.Empty;

        [Required]
        public string LocationId { get; set; } = string.Empty;

        public int Quantity { get; set; } = 0;

        public int RestockThreshold { get; set; } = 10;
    }
}
