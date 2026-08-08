using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using server.Dto;
using server.Interfaces.Repository;
using server.Enums;

namespace server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class CountryController : ControllerBase
    {
        private readonly ICountryRepository _countryRepository;
        private readonly ILogger<CountryController> _logger;

        public CountryController(ICountryRepository countryRepository, ILogger<CountryController> logger)
        {
            _countryRepository = countryRepository;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var countries = await _countryRepository.GetAllCountriesAsync();
                return Ok(new { responseCode = 0, data = countries });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting all countries");
                return StatusCode(500, new { responseCode = 500, message = "An error occurred" });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var country = await _countryRepository.GetCountryByIdAsync(id);
                if (country == null)
                {
                    return NotFound(new { responseCode = 404, message = "Country not found" });
                }
                return Ok(new { responseCode = 0, data = country });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting country by id: {Id}", id);
                return StatusCode(500, new { responseCode = 500, message = "An error occurred" });
            }
        }

        [HttpGet("active")]
        public async Task<IActionResult> GetActive()
        {
            try
            {
                var countries = await _countryRepository.GetActiveCountriesAsync();
                return Ok(new { responseCode = 0, data = countries });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting active countries");
                return StatusCode(500, new { responseCode = 500, message = "An error occurred" });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CountryRequestDto countryDto)
        {
            try
            {
                var exists = await _countryRepository.IsCountryExistsAsync(countryDto.CountryName);
                if (exists)
                {
                    return BadRequest(new { responseCode = 400, message = "Country name already exists" });
                }

                var country = await _countryRepository.CreateCountryAsync( countryDto);
                return Ok(new { responseCode = 0, data = country, message = "Country created successfully" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating country");
                return StatusCode(500, new { responseCode = 500, message = "An error occurred" });
            }
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] CountryRequestDto countryDto)
        {
            try
            {
                var exists = await _countryRepository.IsCountryExistsAsync(countryDto.CountryName, countryDto.CountryId);
                if (exists)
                {
                    return BadRequest(new { responseCode = 400, message = "Country name already exists" });
                }

                var country = await _countryRepository.UpdateCountryAsync(countryDto);
                if (country == null)
                {
                    return NotFound(new { responseCode = 404, message = "Country not found" });
                }

                return Ok(new { responseCode = 0, data = country, message = "Country updated successfully" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating country: {CountryId}", countryDto.CountryId);
                return StatusCode(500, new { responseCode = 500, message = "An error occurred" });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var result = await _countryRepository.DeleteCountryAsync(id);
                if (!result)
                {
                    return NotFound(new { responseCode = 404, message = "Country not found" });
                }
                return Ok(new { responseCode = 0, message = "Country deleted successfully" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting country: {Id}", id);
                return StatusCode(500, new { responseCode = 500, message = "An error occurred" });
            }
        }

        [HttpGet("check-exists")]
        public async Task<IActionResult> CheckExists([FromQuery] string countryName, [FromQuery] int? excludeId = null)
        {
            try
            {
                var exists = await _countryRepository.IsCountryExistsAsync(countryName, excludeId);
                return Ok(new { responseCode = 0, data = exists });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error checking country existence");
                return StatusCode(500, new { responseCode = 500, message = "An error occurred" });
            }
        }
    }
}