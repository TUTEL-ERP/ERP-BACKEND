using System.Text.Json;

namespace server.Dto
{
    public class AreaDto
    {
        public int AreaId { get; set; }
        public int CityId { get; set; }
        public string CityName { get; set; } = string.Empty;
        public string AreaName { get; set; } = string.Empty;
        public string? PostalCode { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
    }

    public class AreaRequestDto
    {
        public string? formId { get; set; }
        public JsonElement data { get; set; }
        public string? recordId { get; set; }
    }
}