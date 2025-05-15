using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Sims.Api.Models.Base;

namespace Sims.Api.Models
{
    public class PurchaseOrderItem : BaseModel
    {
        [Key]
        public Ulid Id { get; set; }

        [Required]
        public long ShopId { get; set; }

        [Required]
        public string PurchaseOrderId { get; set; } = string.Empty;

        [Required]
        public string ProductId { get; set; } = string.Empty;

        [Required]
        public int Quantity { get; set; }

        [Required]
        [Column(TypeName = "decimal(12,2)")]
        public decimal UnitPrice { get; set; }
    }
}
