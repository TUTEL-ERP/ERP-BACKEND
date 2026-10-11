using System.Text.Json;

namespace server.Dto
{
    public class ReceiptAdjustmentDto
    {
        public int AdjustmentId { get; set; }
        public string AdjustmentNo { get; set; } = string.Empty;
        public DateTime AdjustmentDate { get; set; }
        public int CustomerId { get; set; }
        public string CustomerName { get; set; } = string.Empty;
        public string AdjustmentType { get; set; } = "DISCOUNT";
        public decimal TotalAmount { get; set; }
        public string? ReferenceNo { get; set; }
        public string? Remarks { get; set; }
        public string Status { get; set; } = "DRAFT";
        public bool IsActive { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTime? ModifiedAt { get; set; }
        public string? ApprovedBy { get; set; }
        public DateTime? ApprovedAt { get; set; }
    }

    public class ReceiptAdjustmentDetailDto
    {
        public int AdjustmentDetailId { get; set; }
        public int AdjustmentId { get; set; }
        public int? ReceiptId { get; set; }
        public string? ReceiptNo { get; set; }
        public DateTime? ReceiptDate { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal PaidAmount { get; set; }
        public decimal AdjustmentAmount { get; set; }
        public decimal Balance { get; set; }
        public string? Description { get; set; }
        public string? Remarks { get; set; }
        public bool IsSelected { get; set; }
        public bool IsActive { get; set; }
    }

    public class ReceiptAdjustmentRequestDto
    {
        public string? formId { get; set; }
        public JsonElement data { get; set; }
        public string? recordId { get; set; }
    }
}