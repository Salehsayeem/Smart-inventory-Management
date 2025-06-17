using System.ComponentModel.DataAnnotations;
using Sims.Api.Models.Base;

namespace Sims.Api.Models
{
    public class Inventory : BaseModel
    {
        [Key]
        public long Id { get; set; }

        [Required]
        public long ShopId { get; set; }

        [Required]
        public long ProductId { get; set; } 

        [Required]
        public long LocationId { get; set; } 

        public int Quantity { get; set; } = 0;

        public int RestockThreshold { get; set; } = 10;
    }
}
