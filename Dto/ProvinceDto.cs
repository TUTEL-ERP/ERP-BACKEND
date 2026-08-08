using System.Text.Json;

namespace server.Dto
{
    public class ProvinceDto
    {
        public int ProvinceId { get; set; }
        public int CountryId { get; set; }
        public string ProvinceName { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
    }

    public class ProvinceRequestDto
    {
        public string? formId { get; set; }
        public JsonElement data { get; set; }
        public string? recordId { get; set; }
    }
}