using System.ComponentModel.DataAnnotations;

namespace Sims.Api.Models
{
    public class ForecastData
    {
        [Key]
        public Ulid Id { get; set; }

        [Required]
        public long ShopId { get; set; }

        [Required]
        public long ProductId { get; set; }

        [Required]
        public string LocationId { get; set; } = string.Empty;

        [Required]
        public DateTime ForecastDate { get; set; }

        [Required]
        public int ForecastQuantity { get; set; }

        [StringLength(50)]
        public string? ModelVersion { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
