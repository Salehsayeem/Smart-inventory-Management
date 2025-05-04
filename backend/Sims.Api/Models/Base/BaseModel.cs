namespace Sims.Api.Models.Base
{
    public class BaseModel
    {
        public Ulid CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public Ulid? ModifiedBy { get; set; }
        public DateTime? ModifiedAt { get; set; }
        public bool IsActive { get; set; } = true;

    }
}
