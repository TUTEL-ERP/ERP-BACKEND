using System.Text.Json.Serialization;

namespace server.Dto
{
    public sealed record ReportDto
    {
        public const string FormId = "FinancialReports";

        [JsonPropertyName("fromDate")]
        public DateOnly? FromDate { get; init; }

        [JsonPropertyName("toDate")]
        public DateOnly? ToDate { get; init; }

        [JsonPropertyName("asOfDate")]
        public DateOnly? AsOfDate { get; init; }
    }
}
