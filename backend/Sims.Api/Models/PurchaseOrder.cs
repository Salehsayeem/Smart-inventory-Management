using System.ComponentModel.DataAnnotations;
using Sims.Api.Models.Base;

namespace Sims.Api.Models
{
    public class PurchaseOrder : BaseModel
    {
        [Key]
        public long Id { get; set; }

        [Required]
        public long ShopId { get; set; }

        [Required]
        public long SupplierId { get; set; }

        public DateTime OrderDate { get; set; } = DateTime.UtcNow;

        [StringLength(50)]
        public string Status { get; set; } = "pending";


    }
}
