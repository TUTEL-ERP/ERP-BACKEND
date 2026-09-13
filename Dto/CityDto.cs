using System.Text.Json;

namespace server.Dto
{
    public class CityDto
    {
        public int CityId { get; set; }
        public int ProvinceId { get; set; }
        public string ProvinceName { get; set; } = string.Empty;
        public string CityName { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
    }

    public class CityRequestDto
    {
        public string? formId { get; set; }
        public JsonElement data { get; set; }
        public string? recordId { get; set; }
    }

    public class CityResponseDto
    {
        public int CityId { get; set; }
        public string? Message { get; set; }
    }
}