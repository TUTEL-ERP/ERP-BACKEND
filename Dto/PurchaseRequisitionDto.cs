using System.Text.Json;

namespace server.Dto
{
    public class PurchaseRequisitionDto
    {
        public int RequisitionId { get; set; }
        public int CompanyId { get; set; }
        public string CompanyName { get; set; } = string.Empty;
        public string? RequestedBy { get; set; }
        public DateTime RequestDate { get; set; }
        public string Status { get; set; } = string.Empty;
        public string? Remarks { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
        public List<PurchaseRequisitionLineDto> Lines { get; set; } = new();
    }

    public class PurchaseRequisitionLineDto
    {
        public int RequisitionLineId { get; set; }
        public int RequisitionId { get; set; }
        public int ItemId { get; set; }
        public string ItemCode { get; set; } = string.Empty;
        public string ItemName { get; set; } = string.Empty;
        public decimal SalePrice { get; set; }
        public decimal Quantity { get; set; }
        public decimal LineTotal { get; set; }
        public bool IsActive { get; set; }
    }

    public class PurchaseRequisitionRequestDto
    {
        public string? formId { get; set; }
        public JsonElement data { get; set; }
        public string? recordId { get; set; }
    }
}