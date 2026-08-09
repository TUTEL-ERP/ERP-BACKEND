using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using server.Dto;
using server.Helpers;  // ✅ Added for DataTableHelper
using server.Interfaces.Services;
using System.Security.Claims;
using System.Text.Json;

namespace server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class CountryController : ControllerBase
    {
        private readonly ICountryService _countryService;
        private readonly ILogger<CountryController> _logger;

        public CountryController(ICountryService countryService, ILogger<CountryController> logger)
        {
            _countryService = countryService;
            _logger = logger;
        }

        private string GetUserId()
        {
            return User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        }

        // ─── GRID DATA ──────────────────────────────────────────────────────
        [HttpGet("grid/{formName}")]
        public async Task<IActionResult> GetGridData(string formName)
        {
            try
            {
                var userid = GetUserId();
                if (string.IsNullOrEmpty(userid))
                    return Ok(ApiResponseHelper.Fail("userid not found"));

                var request = new CountryRequestDto
                {
                    formId = formName,
                    data = JsonDocument.Parse("{}").RootElement
                };

                var dataTable = await _countryService.GetGridDataAsync(userid, request);

                // ✅ FIX: Convert DataTable to List<dynamic>
                var cleanData = dataTable.ToDynamicList();

                return Ok(ApiResponseHelper.Sucess(cleanData, "Data retrieved successfully"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting grid data");
                return StatusCode(500, ApiResponseHelper.Fail("An error occurred"));
            }
        }

        // ─── SINGLE RECORD ──────────────────────────────────────────────────
        [HttpGet("record/{id}")]
        public async Task<IActionResult> SelectRecord(int id)
        {
            try
            {
                var userid = GetUserId();
                if (string.IsNullOrEmpty(userid))
                    return Ok(ApiResponseHelper.Fail("userid not found"));

                var request = new CountryRequestDto
                {
                    recordId = id.ToString(),
                    data = JsonDocument.Parse("{}").RootElement
                };

                var dataTable = await _countryService.SelectRecordAsync(userid, request);

                // ✅ FIX: Convert DataTable to List<dynamic>
                var cleanData = dataTable.ToDynamicList();

                if (cleanData.Count > 0)
                    return Ok(ApiResponseHelper.Sucess(cleanData, "Data retrieved successfully"));
                else
                    return Ok(ApiResponseHelper.Fail("Record not found"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting record");
                return StatusCode(500, ApiResponseHelper.Fail("An error occurred"));
            }
        }

        // ─── INSERT RECORD ──────────────────────────────────────────────────
        [HttpPost("insertRecord")]
        public async Task<IActionResult> InsertRecord([FromBody] CountryRequestDto request)
        {
            try
            {
                var userid = GetUserId();
                if (string.IsNullOrEmpty(userid))
                    return Ok(ApiResponseHelper.Fail("userid not found"));

                var data = request.data;
                if (data.ValueKind == JsonValueKind.Null || data.ValueKind == JsonValueKind.Undefined ||
                    (data.ValueKind == JsonValueKind.Object && !data.EnumerateObject().Any()))
                {
                    return Ok(ApiResponseHelper.Fail("Invalid/Empty data"));
                }

                var dataTable = await _countryService.InsertRecordAsync(userid, request);

                if (dataTable.Rows.Count > 0)
                    return Ok(ApiResponseHelper.Sucess(null, dataTable.Rows[0][0].ToString()));
                else
                    return Ok(ApiResponseHelper.Fail("No response received"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inserting record");
                return StatusCode(500, ApiResponseHelper.Fail("An error occurred"));
            }
        }

        // ─── UPDATE RECORD ──────────────────────────────────────────────────
        [HttpPut("updateRecord")]
        public async Task<IActionResult> UpdateRecord([FromBody] CountryRequestDto request)
        {
            try
            {
                var userid = GetUserId();
                if (string.IsNullOrEmpty(userid))
                    return Ok(ApiResponseHelper.Fail("userid not found"));

                if (string.IsNullOrEmpty(request.recordId))
                    return Ok(ApiResponseHelper.Fail("Empty Record ID"));

                var data = request.data;
                if (data.ValueKind == JsonValueKind.Null || data.ValueKind == JsonValueKind.Undefined ||
                    (data.ValueKind == JsonValueKind.Object && !data.EnumerateObject().Any()))
                {
                    return Ok(ApiResponseHelper.Fail("Invalid/Empty data"));
                }

                var dataTable = await _countryService.UpdateRecordAsync(userid, request);

                if (dataTable.Rows.Count > 0)
                    return Ok(ApiResponseHelper.Sucess(null, dataTable.Rows[0][0].ToString()));
                else
                    return Ok(ApiResponseHelper.Fail("No response received"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating record");
                return StatusCode(500, ApiResponseHelper.Fail("An error occurred"));
            }
        }

        // ─── DELETE RECORD ──────────────────────────────────────────────────
        [HttpDelete("deleteRecord/{id}")]
        public async Task<IActionResult> DeleteRecord(int id)
        {
            try
            {
                var userid = GetUserId();
                if (string.IsNullOrEmpty(userid))
                    return Ok(ApiResponseHelper.Fail("userid not found"));

                var request = new CountryRequestDto
                {
                    recordId = id.ToString(),
                    data = JsonDocument.Parse("{}").RootElement
                };

                var dataTable = await _countryService.DeleteRecordAsync(userid, request);

                if (dataTable.Rows.Count > 0)
                    return Ok(ApiResponseHelper.Sucess(null, dataTable.Rows[0][0].ToString()));
                else
                    return Ok(ApiResponseHelper.Fail("No response received"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting record");
                return StatusCode(500, ApiResponseHelper.Fail("An error occurred"));
            }
        }
    }
}       