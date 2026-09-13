using System.Text.Json;

namespace server.Dto
{
    public class DepartmentDto
    {
        public int DepartmentId { get; set; }
        public int BranchId { get; set; }
        public string BranchName { get; set; } = string.Empty;
        public string DepartmentName { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
    }

    public class DepartmentRequestDto
    {
        public string? formId { get; set; }
        public JsonElement data { get; set; }
        public string? recordId { get; set; }
    }
}