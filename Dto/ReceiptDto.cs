using System.Text.Json;

namespace server.Dto
{
    public class ReceiptDto
    {
        public int ReceiptId { get; set; }
        public string ReceiptNo { get; set; } = string.Empty;
        public DateTime ReceiptDate { get; set; }
        public int CustomerId { get; set; }
        public string CustomerName { get; set; } = string.Empty;
        public int? PaymentMethodId { get; set; }
        public int? BankId { get; set; }
        public int? AccountId { get; set; }
        public string? ReferenceNo { get; set; }
        public decimal TotalAmount { get; set; }
        public string? Remarks { get; set; }
        public string Status { get; set; } = "DRAFT";
        public bool IsActive { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTime? ModifiedAt { get; set; }
    }

    public class ReceiptDetailDto
    {
        public int ReceiptDetailId { get; set; }
        public int ReceiptId { get; set; }
        public int? SaleId { get; set; }
        public string? SaleNo { get; set; }
        public DateTime? SaleDate { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal ReceiptAmount { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal NetAmount { get; set; }
        public string? Remarks { get; set; }
        public bool IsActive { get; set; }
    }

    public class ReceiptRequestDto
    {
        public string? formId { get; set; }
        public JsonElement data { get; set; }
        public string? recordId { get; set; }
    }
}