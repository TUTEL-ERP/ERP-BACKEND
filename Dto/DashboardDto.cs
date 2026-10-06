using System.Text.Json;

namespace server.Dto
{
    public class DashboardKpiDto
    {
        public int TotalCustomers { get; set; }
        public int TotalSuppliers { get; set; }
        public int TotalItems { get; set; }
        public decimal SalesThisMonth { get; set; }
        public decimal SalesLastMonth { get; set; }
        public decimal SalesTrend { get; set; }
        public decimal PurchasesThisMonth { get; set; }
        public decimal ReceiptsThisMonth { get; set; }
        public decimal PaymentsThisMonth { get; set; }
        public decimal ReturnsThisMonth { get; set; }
        public decimal NetCashFlow { get; set; }
    }

    public class DashboardRequestDto
    {
        public string? formId { get; set; }
        public JsonElement data { get; set; }
        public string? recordId { get; set; }
    }
}