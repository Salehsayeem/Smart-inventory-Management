using System.ComponentModel.DataAnnotations;
using Sims.Api.Helper;
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
        [Required]
        public long LocationId { get; set; }
        public Ulid Code { get; set; }
        public DateTime OrderDate { get; set; } = DateTime.UtcNow;
        public string Status { get; set; } = nameof(PurchaseOrderStatus.Pending);
    }
}
