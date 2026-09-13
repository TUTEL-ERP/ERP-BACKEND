using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using server.Dto;
using server.Helpers;
using server.Interfaces.Services;
using System.Security.Claims;
using System.Text.Json;

namespace server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class CityController : ControllerBase
    {
        private readonly ICityService _cityService;
        private readonly ILogger<CityController> _logger;

        public CityController(
            ICityService cityService,
            ILogger<CityController> logger)
        {
            _cityService = cityService;
            _logger = logger;
        }

        private string? GetUserId()
        {
            return User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        }

        // =========================
        // GET GRID DATA
        // =========================
        [HttpGet("grid/{formName}")]
        public async Task<IActionResult> GetGridData(string formName)
        {
            try
            {
                var userid = GetUserId();

                if (string.IsNullOrEmpty(userid))
                    return Ok(ApiResponseHelper.Fail("userid not found"));

                var request = new CityRequestDto
                {
                    formId = formName,
                    data = JsonDocument.Parse("{}").RootElement
                };

                var result = await _cityService.GetGridDataAsync(
                    userid,
                    request);

                return Ok(
                    ApiResponseHelper.Sucess(
                        result,
                        "Data retrieved successfully"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting grid data");

                return StatusCode(
                    500,
                    ApiResponseHelper.Fail("An error occurred"));
            }
        }

        // =========================
        // GET SINGLE RECORD
        // =========================
        [HttpGet("record/{id}")]
        public async Task<IActionResult> SelectRecord(int id)
        {
            try
            {
                var userid = GetUserId();

                if (string.IsNullOrEmpty(userid))
                    return Ok(ApiResponseHelper.Fail("userid not found"));

                var request = new CityRequestDto
                {
                    recordId = id.ToString(),
                    data = JsonDocument.Parse("{}").RootElement
                };

                var result = await _cityService.SelectRecordAsync(
                    userid,
                    request);

                if (result != null && result.Count > 0)
                {
                    return Ok(
                        ApiResponseHelper.Sucess(
                            result.First(),
                            "Data retrieved successfully"));
                }

                return Ok(
                    ApiResponseHelper.Fail("Record not found"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting record");

                return StatusCode(
                    500,
                    ApiResponseHelper.Fail("An error occurred"));
            }
        }

        // =========================
        // INSERT
        // =========================
        [HttpPost("insertRecord")]
        public async Task<IActionResult> InsertRecord(
            [FromBody] CityRequestDto request)
        {
            try
            {
                var userid = GetUserId();

                if (string.IsNullOrEmpty(userid))
                    return Ok(ApiResponseHelper.Fail("userid not found"));

                var data = request.data;

                if (data.ValueKind == JsonValueKind.Null ||
                    data.ValueKind == JsonValueKind.Undefined ||
                    (data.ValueKind == JsonValueKind.Object &&
                     !data.EnumerateObject().Any()))
                {
                    return Ok(
                        ApiResponseHelper.Fail("Invalid/Empty data"));
                }

                var result = await _cityService.InsertRecordAsync(
                    userid,
                    request);

                if (result != null && result.Count > 0)
                {
                    return Ok(
                        ApiResponseHelper.Sucess(
                            result,
                            "Record inserted successfully"));
                }

                return Ok(
                    ApiResponseHelper.Fail("No response received"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inserting record");

                return StatusCode(
                    500,
                    ApiResponseHelper.Fail("An error occurred"));
            }
        }

        // =========================
        // UPDATE
        // =========================
        [HttpPut("updateRecord")]
        public async Task<IActionResult> UpdateRecord(
            [FromBody] CityRequestDto request)
        {
            try
            {
                var userid = GetUserId();

                if (string.IsNullOrEmpty(userid))
                    return Ok(ApiResponseHelper.Fail("userid not found"));

                if (string.IsNullOrEmpty(request.recordId))
                    return Ok(ApiResponseHelper.Fail("Empty Record ID"));

                var data = request.data;

                if (data.ValueKind == JsonValueKind.Null ||
                    data.ValueKind == JsonValueKind.Undefined ||
                    (data.ValueKind == JsonValueKind.Object &&
                     !data.EnumerateObject().Any()))
                {
                    return Ok(
                        ApiResponseHelper.Fail("Invalid/Empty data"));
                }

                var result = await _cityService.UpdateRecordAsync(
                    userid,
                    request);

                if (result != null && result.Count > 0)
                {
                    return Ok(
                        ApiResponseHelper.Sucess(
                            result,
                            "Record updated successfully"));
                }

                return Ok(
                    ApiResponseHelper.Fail("No response received"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating record");

                return StatusCode(
                    500,
                    ApiResponseHelper.Fail("An error occurred"));
            }
        }

        // =========================
        // DELETE
        // =========================
        [HttpDelete("deleteRecord/{id}")]
        public async Task<IActionResult> DeleteRecord(int id)
        {
            try
            {
                var userid = GetUserId();

                if (string.IsNullOrEmpty(userid))
                    return Ok(ApiResponseHelper.Fail("userid not found"));

                var request = new CityRequestDto
                {
                    recordId = id.ToString(),
                    data = JsonDocument.Parse("{}").RootElement
                };

                var result = await _cityService.DeleteRecordAsync(
                    userid,
                    request);

                if (result != null && result.Count > 0)
                {
                    return Ok(
                        ApiResponseHelper.Sucess(
                            result,
                            "Record deleted successfully"));
                }

                return Ok(
                    ApiResponseHelper.Fail("No response received"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting record");

                return StatusCode(
                    500,
                    ApiResponseHelper.Fail("An error occurred"));
            }
        }
    }
}