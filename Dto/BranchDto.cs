using System.Text.Json;

namespace server.Dto
{
    public class BranchDto
    {
        public int BranchId { get; set; }
        public int CompanyId { get; set; }
        public string CompanyName { get; set; } = string.Empty;
        public string BranchName { get; set; } = string.Empty;
        public string BranchCode { get; set; } = string.Empty;
        public string? Address { get; set; }
        public string? Phone { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
    }

    public class BranchRequestDto
    {
        public string? formId { get; set; }
        public JsonElement data { get; set; }
        public string? recordId { get; set; }
    }
}