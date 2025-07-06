using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Sims.Api.Models.Base;
using Sims.Api.Helper;

namespace Sims.Api.Models
{
    public class SaleItem : BaseModel
    {
        [Key]
        public long Id { get; set; }
        public long  SaleId { get; set; }
        [Required]
        public long ProductId { get; set; }
        [Required]
        public int QuantitySold { get; set; }
        [Required]
        [Column(TypeName = "decimal(12,2)")]
        public decimal UnitPrice { get; set; }
    }
}
