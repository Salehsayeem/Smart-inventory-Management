using Sims.Api.Models.Base;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Sims.Api.Helper;

namespace Sims.Api.Models
{
    public class Sale : BaseModel
    {
        [Key]
        public long Id { get; set; }
        [Required]
        public long ShopId { get; set; }
        [Required]
        public long LocationId { get; set; }
        public Ulid Code { get; set; }
        public DateTime SaleDate { get; set; }
        public string Status { get; set; } = nameof(SalesStatus.Completed);
    }
}
