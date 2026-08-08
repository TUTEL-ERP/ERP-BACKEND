// Dto/CountryDto.cs
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
        public int CountryId { get; set; }
        public string CountryName { get; set; } = string.Empty;
        public string? CountryCode { get; set; }
        public bool IsActive { get; set; } = true;
        public int? ExcludeId { get; set; }
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

    public class ApiResponse<T>
    {
        public bool IsSuccess { get; set; }
        public T? Data { get; set; }
        public string Message { get; set; } = string.Empty;
        public int ResponseCode { get; set; }
    }
}