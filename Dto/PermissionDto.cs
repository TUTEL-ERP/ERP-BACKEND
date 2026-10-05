using System.Text.Json;

namespace server.Dto
{
    public class PermissionResponseDto
    {
        public string FormId { get; set; } = string.Empty;
        public string? RoleCode { get; set; }
        public bool IsTransaction { get; set; }        
        public bool View { get; set; }
        public bool Add { get; set; }
        public bool Edit { get; set; }
        public bool Delete { get; set; }
        public bool Approve { get; set; }             
        public bool Reject { get; set; }
        public bool Print { get; set; }
    }

    public class PermissionRequestDto
    {
        public string? formId { get; set; }
        public JsonElement data { get; set; }
        public string? recordId { get; set; }
    }
}