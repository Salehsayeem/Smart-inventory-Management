﻿namespace Sims.Api.Dto.Auth
{
    public class PermissionDto
    {
        public Ulid UserId { get; set; }
        public int RoleId { get; set; }
        public string RoleName { get; set; } = string.Empty;
        public List<PermissionDetailsDto> PermissionDetails { get; set; } = new List<PermissionDetailsDto>();
    }
    public class PermissionDetailsDto
    {
        public long Id { get; set; }
        public int ModuleId { get; set; }
        public string ModuleName { get; set; } = string.Empty;
        public string MenuIcon { get; set; } = string.Empty;
        public string Path { get; set; } = string.Empty;
        public bool IsCreate { get; set; }
        public bool IsView { get; set; }
        public bool IsEdit { get; set; }
        public bool IsList { get; set; }
        public bool IsDelete { get; set; }
        public bool IsActive { get; set; }
    }
    public class UpdatePermissionOfUserDto
    {
        public long Id { get; set; }
        public bool IsCreate { get; set; }
        public bool IsView { get; set; }
        public bool IsEdit { get; set; }
        public bool IsList { get; set; }
        public bool IsDelete { get; set; }
    }
}
