// Dto/CountryDto.cs
using System.Text.Json;

namespace server.Dto
{
    public class CountryDto
    {
        public int CountryId { get; set; }
        public string CountryName { get; set; } = string.Empty;
        public string? CountryCode { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
    }

    public class CountryRequestDto
    {
        public string? formId { get; set; }
        public JsonElement data { get; set; }
        public string? recordId { get; set; }
    }

    public class CountryResponseDto
    {
        public int CountryId { get; set; }
        public string CountryName { get; set; } = string.Empty;
        public string? CountryCode { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
        public string? Status { get; set; }
        public string? Message { get; set; }
    }

 
}