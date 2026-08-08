// Services/CountryService.cs
using AutoMapper;
using server.Dto;
using server.Enums;
using server.Interfaces.Repository;
using server.Interfaces.Services;

namespace server.Services
{
    public class CountryService : ICountryService
    {
        private readonly ICountryRepository _countryRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<CountryService> _logger;

        public CountryService(
            ICountryRepository countryRepository,
            IMapper mapper,
            ILogger<CountryService> logger)
        {
            _countryRepository = countryRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<ApiResponse<IEnumerable<CountryDto>>> GetAllAsync()
        {
            try
            {
                var countries = await _countryRepository.GetAllCountriesAsync();

                return new ApiResponse<IEnumerable<CountryDto>>
                {
                    IsSuccess = true,
                    Data = countries,
                    Message = "Countries retrieved successfully",
                    ResponseCode = 0
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting all countries");
                return new ApiResponse<IEnumerable<CountryDto>>
                {
                    IsSuccess = false,
                    Message = "Failed to retrieve countries",
                    ResponseCode = 500
                };
            }
        }

        public async Task<ApiResponse<CountryDto>> GetByIdAsync(int countryId)
        {
            try
            {
                var country = await _countryRepository.GetCountryByIdAsync(countryId);

                if (country == null)
                {
                    return new ApiResponse<CountryDto>
                    {
                        IsSuccess = false,
                        Message = "Country not found",
                        ResponseCode = 404
                    };
                }

                return new ApiResponse<CountryDto>
                {
                    IsSuccess = true,
                    Data = country,
                    Message = "Country retrieved successfully",
                    ResponseCode = 0
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting country by id: {CountryId}", countryId);
                return new ApiResponse<CountryDto>
                {
                    IsSuccess = false,
                    Message = "Failed to retrieve country",
                    ResponseCode = 500
                };
            }
        }

        public async Task<ApiResponse<IEnumerable<CountryDto>>> GetActiveAsync()
        {
            try
            {
                var countries = await _countryRepository.GetActiveCountriesAsync();

                return new ApiResponse<IEnumerable<CountryDto>>
                {
                    IsSuccess = true,
                    Data = countries,
                    Message = "Active countries retrieved successfully",
                    ResponseCode = 0
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting active countries");
                return new ApiResponse<IEnumerable<CountryDto>>
                {
                    IsSuccess = false,
                    Message = "Failed to retrieve active countries",
                    ResponseCode = 500
                };
            }
        }

        public async Task<ApiResponse<CountryDto>> CreateAsync(CountryRequestDto request)
        {
            try
            {
                // Check if country already exists
                var exists = await _countryRepository.IsCountryExistsAsync(request.CountryName);
                if (exists)
                {
                    return new ApiResponse<CountryDto>
                    {
                        IsSuccess = false,
                        Message = "Country name already exists",
                        ResponseCode = 400
                    };
                }

                var country = await _countryRepository.CreateCountryAsync(request);

                return new ApiResponse<CountryDto>
                {
                    IsSuccess = true,
                    Data = country,
                    Message = "Country created successfully",
                    ResponseCode = 0
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating country: {CountryName}", request.CountryName);
                return new ApiResponse<CountryDto>
                {
                    IsSuccess = false,
                    Message = "Failed to create country",
                    ResponseCode = 500
                };
            }
        }

        public async Task<ApiResponse<CountryDto>> UpdateAsync(CountryRequestDto request)
        {
            try
            {
                // Check if country exists
                var existing = await _countryRepository.GetCountryByIdAsync(request.CountryId);
                if (existing == null)
                {
                    return new ApiResponse<CountryDto>
                    {
                        IsSuccess = false,
                        Message = "Country not found",
                        ResponseCode = 404
                    };
                }

                // Check if country name already exists (excluding current)
                var exists = await _countryRepository.IsCountryExistsAsync(request.CountryName, request.CountryId);
                if (exists)
                {
                    return new ApiResponse<CountryDto>
                    {
                        IsSuccess = false,
                        Message = "Country name already exists",
                        ResponseCode = 400
                    };
                }

                var country = await _countryRepository.UpdateCountryAsync(request);

                if (country == null)
                {
                    return new ApiResponse<CountryDto>
                    {
                        IsSuccess = false,
                        Message = "Failed to update country",
                        ResponseCode = 500
                    };
                }

                return new ApiResponse<CountryDto>
                {
                    IsSuccess = true,
                    Data = country,
                    Message = "Country updated successfully",
                    ResponseCode = 0
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating country: {CountryId}", request.CountryId);
                return new ApiResponse<CountryDto>
                {
                    IsSuccess = false,
                    Message = "Failed to update country",
                    ResponseCode = 500
                };
            }
        }

        public async Task<ApiResponse<bool>> DeleteAsync(int countryId)
        {
            try
            {
                // Check if country exists
                var existing = await _countryRepository.GetCountryByIdAsync(countryId);
                if (existing == null)
                {
                    return new ApiResponse<bool>
                    {
                        IsSuccess = false,
                        Message = "Country not found",
                        ResponseCode = 404
                    };
                }

                var result = await _countryRepository.DeleteCountryAsync(countryId);

                if (!result)
                {
                    return new ApiResponse<bool>
                    {
                        IsSuccess = false,
                        Message = "Failed to delete country",
                        ResponseCode = 500
                    };
                }

                return new ApiResponse<bool>
                {
                    IsSuccess = true,
                    Data = true,
                    Message = "Country deleted successfully",
                    ResponseCode = 0
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting country: {CountryId}", countryId);
                return new ApiResponse<bool>
                {
                    IsSuccess = false,
                    Message = "Failed to delete country",
                    ResponseCode = 500
                };
            }
        }

        public async Task<ApiResponse<bool>> CheckExistsAsync(string countryName, int? excludeId = null)
        {
            try
            {
                var exists = await _countryRepository.IsCountryExistsAsync(countryName, excludeId);

                return new ApiResponse<bool>
                {
                    IsSuccess = true,
                    Data = exists,
                    Message = "Check completed successfully",
                    ResponseCode = 0
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error checking country existence");
                return new ApiResponse<bool>
                {
                    IsSuccess = false,
                    Message = "Failed to check country existence",
                    ResponseCode = 500
                };
            }
        }
    }
}