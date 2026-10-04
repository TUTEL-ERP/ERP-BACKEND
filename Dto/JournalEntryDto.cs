using System.Text.Json;

namespace server.Dto
{
    public class JournalEntryDto
    {
        public int JournalEntryId { get; set; }
        public string JournalNo { get; set; } = string.Empty;
        public DateTime JournalDate { get; set; }
         public string VoucherType { get; set; } = "JV";     

        public string? ReferenceNo { get; set; }
        public string? Description { get; set; }
        public decimal TotalDebit { get; set; }
        public decimal TotalCredit { get; set; }
        public string Status { get; set; } = "DRAFT";
        public bool IsActive { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTime? ModifiedAt { get; set; }
    }

    public class JournalEntryDetailDto
    {
        public int JournalEntryDetailId { get; set; }
        public int JournalEntryId { get; set; }
        public int AccountId { get; set; }
        public string AccountCode { get; set; } = string.Empty;
        public string AccountName { get; set; } = string.Empty;
        public decimal DebitAmount { get; set; }
        public decimal CreditAmount { get; set; }
        public string? Description { get; set; }
        public int LineNumber { get; set; }
        public bool IsActive { get; set; }
    }

    public class JournalEntryRequestDto
    {
        public string? formId { get; set; }
        public JsonElement data { get; set; }
        public string? recordId { get; set; }
    }
}