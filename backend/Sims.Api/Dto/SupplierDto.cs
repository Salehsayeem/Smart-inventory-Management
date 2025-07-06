using System.ComponentModel.DataAnnotations;

namespace Sims.Api.Dto
{
    public class CreateOrUpdateSupplierDto
    {
        public long Id { get; set; }
        public long ShopId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? ContactPerson { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public string? Address { get; set; }
    }
    public class GetSupplierDto
    {
        public long Id { get; set; }
        public long ShopId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? ContactPerson { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public string? Address { get; set; }
    }
}
