using System.Text.Json;

namespace server.Dto
{
    public class PurchaseOrderDto
    {
        public int POId { get; set; }
        public string PONumber { get; set; } = string.Empty;
        public int SupplierId { get; set; }
        public string SupplierName { get; set; } = string.Empty;
        public int? RequisitionId { get; set; }
        public int? WarehouseId { get; set; }
        public int? DepartmentId { get; set; }
        public string? DepartmentName { get; set; }
        public DateTime PODate { get; set; }
        public DateTime? ExpectedDate { get; set; }
        public string? CurrencyCode { get; set; }

        public string OrderStatus { get; set; } = "OPEN";
        public string ApprovalStatus { get; set; } = "DRAFT";

        public decimal SubTotal { get; set; }
        public decimal TaxAmount { get; set; }
        public decimal TotalAmount { get; set; }
        public string? Remarks { get; set; }
        public bool IsActive { get; set; }

        // ✅ Audit
        public string? CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTime? ModifiedAt { get; set; }
    }

    public class PurchaseOrderLineDto
    {
        public int POLineId { get; set; }
        public int POId { get; set; }
        public int ItemId { get; set; }
        public string ItemCode { get; set; } = string.Empty;
        public string ItemName { get; set; } = string.Empty;
        public decimal Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public int? TaxCodeId { get; set; }
        public decimal LineTotal { get; set; }
        public decimal QuantityReceived { get; set; }
        public decimal QuantityInvoiced { get; set; }
        public string? Remarks { get; set; }
        public bool IsActive { get; set; }
    }

    public class PurchaseOrderRequestDto
    {
        public string? formId { get; set; }
        public JsonElement data { get; set; }
        public string? recordId { get; set; }
    }
}