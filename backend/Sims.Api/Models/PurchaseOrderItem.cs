using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Sims.Api.Models.Base;

namespace Sims.Api.Models
{
    public class PurchaseOrderItem : BaseModel
    {
        [Key]
        public long Id { get; set; }

        [Required]
        public long ShopId { get; set; }

        [Required]
        public long PurchaseOrderId { get; set; } 

        [Required]
        public long ProductId { get; set; } 

        [Required]
        public int Quantity { get; set; }

        [Required]
        [Column(TypeName = "decimal(12,2)")]
        public decimal UnitPrice { get; set; }
    }
}
