using server.Dto;

namespace server.Interfaces.Services
{
    public interface ICountryService
    {
        Task<ApiResponse<IEnumerable<CountryDto>>> GetAllAsync();
        Task<ApiResponse<CountryDto>> GetByIdAsync(int countryId);
        Task<ApiResponse<IEnumerable<CountryDto>>> GetActiveAsync();
        Task<ApiResponse<CountryDto>> CreateAsync(CountryRequestDto request);
        Task<ApiResponse<CountryDto>> UpdateAsync(CountryRequestDto request);
        Task<ApiResponse<bool>> DeleteAsync(int countryId);
        Task<ApiResponse<bool>> CheckExistsAsync(string countryName, int? excludeId = null);
    }
}