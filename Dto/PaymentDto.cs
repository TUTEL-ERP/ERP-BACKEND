using System.Text.Json;

namespace server.Dto
{
    public class PaymentDto
    {
        public int PaymentId { get; set; }
        public string PaymentNo { get; set; } = string.Empty;
        public DateTime PaymentDate { get; set; }
        public int SupplierId { get; set; }
        public string SupplierName { get; set; } = string.Empty;
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

    public class PaymentDetailDto
    {
        public int PaymentDetailId { get; set; }
        public int PaymentId { get; set; }
        public int? POId { get; set; }
        public string? PONumber { get; set; }
        public DateTime? PODate { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal PaymentAmount { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal NetAmount { get; set; }
        public string? Remarks { get; set; }
        public bool IsActive { get; set; }
    }

    public class PaymentRequestDto
    {
        public string? formId { get; set; }
        public JsonElement data { get; set; }
        public string? recordId { get; set; }
    }
}