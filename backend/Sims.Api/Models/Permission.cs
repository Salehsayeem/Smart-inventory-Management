namespace Sims.Api.Models
{
    public class Permission
    {
        public long Id { get; set; }
        public Ulid UserId { get; set; }
        public int RoleId { get; set; }
        public int ModuleId { get; set; }
        public bool IsCreate { get; set; } = false;
        public bool IsView { get; set; } = false;
        public bool IsEdit { get; set; } = false;
        public bool IsList { get; set; } = false;
        public bool IsDelete { get; set; } = false;
        public bool IsActive { get; set; } = true;
        public Ulid? ActionBy { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
