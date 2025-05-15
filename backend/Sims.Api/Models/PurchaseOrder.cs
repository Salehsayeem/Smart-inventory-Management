using System.ComponentModel.DataAnnotations;
using Sims.Api.Models.Base;

namespace Sims.Api.Models
{
    public class PurchaseOrder : BaseModel
    {
        [Key]
        public Ulid Id { get; set; }

        [Required]
        public long ShopId { get; set; }

        [Required]
        public string SupplierId { get; set; } = string.Empty;

        public DateTime OrderDate { get; set; } = DateTime.UtcNow;

        [StringLength(50)]
        public string Status { get; set; } = "pending";


    }
}
