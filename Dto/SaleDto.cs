using System.Text.Json;

namespace server.Dto
{
    public class SaleDto
    {
        public int SaleId { get; set; }
        public string SaleNo { get; set; } = string.Empty;
        public int CustomerId { get; set; }
        public string CustomerName { get; set; } = string.Empty;
        public DateTime SaleDate { get; set; }
        public string? Remarks { get; set; }
        public decimal TotalAmount { get; set; }
        public string Status { get; set; } = "DRAFT";
        public bool IsActive { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTime? ModifiedAt { get; set; }
    }

    public class SaleDetailDto
    {
        public int SaleDetailId { get; set; }
        public int SaleId { get; set; }
        public int ItemId { get; set; }
        public string ItemCode { get; set; } = string.Empty;
        public string ItemName { get; set; } = string.Empty;
        public decimal Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal LineTotal { get; set; }
        public string? Remarks { get; set; }
        public bool IsActive { get; set; }
    }

    public class SaleRequestDto
    {
        public string? formId { get; set; }
        public JsonElement data { get; set; }
        public string? recordId { get; set; }
    }
}