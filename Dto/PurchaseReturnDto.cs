using System.Text.Json;

namespace server.Dto
{
    public class PurchaseReturnDto
    {
        public int PurchaseReturnId { get; set; }
        public string PurchaseReturnNo { get; set; } = string.Empty;
        public int? PurchaseId { get; set; }
        public int SupplierId { get; set; }
        public string SupplierName { get; set; } = string.Empty;
        public DateTime PurchaseReturnDate { get; set; }
        public string? Remarks { get; set; }
        public decimal TotalAmount { get; set; }
        public string Status { get; set; } = "DRAFT";
        public bool IsActive { get; set; }

        // Audit
        public string? CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTime? ModifiedAt { get; set; }
    }

    public class PurchaseReturnLineDto
    {
        public int PurchaseReturnLineId { get; set; }
        public int PurchaseReturnId { get; set; }
        public int ItemId { get; set; }
        public string ItemCode { get; set; } = string.Empty;
        public string ItemName { get; set; } = string.Empty;
        public decimal Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal LineTotal { get; set; }
        public string? Remarks { get; set; }
        public bool IsActive { get; set; }
    }

    public class PurchaseReturnRequestDto
    {
        public string? formId { get; set; }
        public JsonElement data { get; set; }
        public string? recordId { get; set; }
    }
}