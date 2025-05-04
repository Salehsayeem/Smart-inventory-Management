using System.ComponentModel.DataAnnotations;
using Sims.Api.Models.Base;

namespace Sims.Api.Models
{
    public class Shop : BaseModel
    {
        [Key] public long Id { get; set; }
        [Required][StringLength(100)] public string Name { get; set; } = string.Empty;
        [StringLength(100)] public string Address { get; set; } = string.Empty;

    }
}
