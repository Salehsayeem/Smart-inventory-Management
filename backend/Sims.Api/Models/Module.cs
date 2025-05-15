using System.ComponentModel.DataAnnotations;

namespace Sims.Api.Models
{
    public class Module
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        [StringLength(20)]
        public string ModuleIcon { get; set; } = string.Empty;
        [StringLength(20)]
        public string Path { get; set; } = string.Empty;
    }
}
