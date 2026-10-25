using System.Text.Json;

namespace server.Dto
{
    public class DepartmentalIssueDto
    {
        public string? formId { get; set; }
        public JsonElement data { get; set; }
        public string? recordId { get; set; }
    }
}
