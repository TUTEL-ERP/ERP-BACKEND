using System.Text.Json;

namespace server.Dto
{
    public class ChartOfAccountsDto
    {
        public int AccountId { get; set; }
        public string AccountCode { get; set; } = string.Empty;
        public string AccountName { get; set; } = string.Empty;
        public int? ParentAccountId { get; set; }
        public string? ParentAccountName { get; set; }
        public string AccountType { get; set; } = string.Empty;
        public string? AccountSubType { get; set; }
        public string NormalBalance { get; set; } = string.Empty;
        public int Level { get; set; }
        public bool IsGroup { get; set; }
        public decimal OpeningBalance { get; set; }
        public decimal CurrentBalance { get; set; }
        public string CurrencyCode { get; set; } = "PKR";
        public string? Description { get; set; }
        public bool IsActive { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTime? ModifiedAt { get; set; }
    }

    public class ChartOfAccountsRequestDto
    {
        public string? formId { get; set; }
        public JsonElement data { get; set; }
        public string? recordId { get; set; }
    }
}