using System.Text.Json;

namespace server.Dto
{
    public class PaymentAdjustmentDto
    {
        public int AdjustmentId { get; set; }
        public string AdjustmentNo { get; set; } = string.Empty;
        public DateTime AdjustmentDate { get; set; }
        public int SupplierId { get; set; }
        public string SupplierName { get; set; } = string.Empty;
        public int? PaymentId { get; set; }
        public string? PaymentNo { get; set; }
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

    public class PaymentAdjustmentDetailDto
    {
        public int AdjustmentDetailId { get; set; }
        public int AdjustmentId { get; set; }
        public string? Description { get; set; }
        public decimal AdjustmentAmount { get; set; }
        public string? Remarks { get; set; }
        public bool IsActive { get; set; }
    }

    public class PaymentAdjustmentRequestDto
    {
        public string? formId { get; set; }
        public JsonElement data { get; set; }
        public string? recordId { get; set; }
    }
}