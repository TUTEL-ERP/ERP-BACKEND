using server.Dto;
using server.Enity;
using server.Enums;

namespace server.Interfaces.Repository
{
    public interface ICountryRepository : IGenericRepository<Country>
    {
        Task<IEnumerable<CountryDto>> GetAllCountriesAsync();
        Task<CountryDto?> GetCountryByIdAsync(int countryId);
        Task<IEnumerable<CountryDto>> GetActiveCountriesAsync();
        Task<bool> IsCountryExistsAsync(string countryName, int? excludeId = null);
        Task<CountryDto> CreateCountryAsync(CountryRequestDto country);
        Task<CountryDto?> UpdateCountryAsync(CountryRequestDto country);
        Task<bool> DeleteCountryAsync(int countryId);
        Task<List<dynamic>> GetGridDataAsync(string formName);
        Task<dynamic> ExecuteSpAsync(ActionType action, CountryRequestDto request, string formName = "Country");
        Task<dynamic> GetRecordByIdAsync(string formName, int id);
    }
}