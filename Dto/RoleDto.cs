using System.Text.Json;

namespace server.Dto
{
    public class RoleDto
    {
        public int RoleId { get; set; }
        public string RoleCode { get; set; } = string.Empty;
        public string RoleName { get; set; } = string.Empty;
        public string RoleType { get; set; } = "CUSTOM";
        public string? Description { get; set; }
        public bool IsCustom { get; set; } = true;
        public bool IsActive { get; set; } = true;
        public string? CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTime? ModifiedAt { get; set; }
    }

    public class PermissionDto
    {
        public int PermissionId { get; set; }
        public string PermissionCode { get; set; } = string.Empty;
        public string PermissionName { get; set; } = string.Empty;
        public int DisplayOrder { get; set; }
    }

    public class FormDto
    {
        public int MenuId { get; set; }
        public int? ParentMenuId { get; set; }
        public string MenuName { get; set; } = string.Empty;
        public string? Route { get; set; }
        public string? Icon { get; set; }
        public int DisplayOrder { get; set; }
        public string? FormId { get; set; }
    }

    public class RolePermissionDto
    {
        public string FormId { get; set; } = string.Empty;
        public int PermissionId { get; set; }
        public string PermissionCode { get; set; } = string.Empty;
        public bool IsAllowed { get; set; }
    }

    public class RoleRequestDto
    {
        public string? formId { get; set; }
        public JsonElement data { get; set; }
        public string? recordId { get; set; }
    }
}