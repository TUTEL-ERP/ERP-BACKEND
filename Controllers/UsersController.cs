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
    public class UsersController : ControllerBase
    {
        private readonly IUserSetupService _service;
        private readonly ILogger<UsersController> _logger;

        public UsersController(IUserSetupService service, ILogger<UsersController> logger)
        {
            _service = service;
            _logger = logger;
        }

        private string GetUserId()
        {
            var userid = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userid))
                userid = User.FindFirst("userID")?.Value;
            return userid;
        }

        [HttpGet("grid/{formName}")]
        public async Task<IActionResult> GetGridData(string formName)
        {
            try
            {
                var userid = GetUserId();
                if (string.IsNullOrEmpty(userid)) return Ok(ApiResponseHelper.Fail("userid not found"));

                var request = new UserRequestDto { formId = formName, data = JsonDocument.Parse("{}").RootElement };
                var dt = await _service.GetGridDataAsync(userid, request);
                return Ok(ApiResponseHelper.Sucess(DataTableHelper.ToDynamicList(dt), "Data retrieved successfully"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting grid data");
                return StatusCode(500, ApiResponseHelper.Fail("An error occurred"));
            }
        }

        [HttpGet("record/{id}")]
        public async Task<IActionResult> SelectRecord(int id)
        {
            try
            {
                var userid = GetUserId();
                if (string.IsNullOrEmpty(userid)) return Ok(ApiResponseHelper.Fail("userid not found"));

                var request = new UserRequestDto { recordId = id.ToString(), data = JsonDocument.Parse("{}").RootElement };
                var dt = await _service.SelectRecordAsync(userid, request);
                var data = DataTableHelper.ToDynamicList(dt);

                return data.Count > 0
                    ? Ok(ApiResponseHelper.Sucess(data.FirstOrDefault(), "Data retrieved successfully"))
                    : Ok(ApiResponseHelper.Fail("Record not found"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting record");
                return StatusCode(500, ApiResponseHelper.Fail("An error occurred"));
            }
        }

        [HttpPost("insertRecord")]
        public async Task<IActionResult> InsertRecord([FromBody] UserRequestDto request)
        {
            try
            {
                var userid = GetUserId();
                if (string.IsNullOrEmpty(userid)) return Ok(ApiResponseHelper.Fail("userid not found"));

                var data = request.data;
                if (data.ValueKind == JsonValueKind.Null || data.ValueKind == JsonValueKind.Undefined ||
                    (data.ValueKind == JsonValueKind.Object && !data.EnumerateObject().Any()))
                    return Ok(ApiResponseHelper.Fail("Invalid/Empty data"));

                var password = data.TryGetProperty("password", out var pw) ? pw.GetString() : null;
                if (!string.IsNullOrEmpty(password))
                {
                    var hash = BCrypt.Net.BCrypt.HashPassword(password);
                    var jsonDict = JsonSerializer.Deserialize<Dictionary<string, object>>(data.GetRawText());
                    jsonDict["passwordHash"] = hash;
                    data = JsonSerializer.SerializeToElement(jsonDict);
                }

                var finalRequest = new UserRequestDto
                {
                    formId = request.formId,
                    data = data,
                    recordId = request.recordId
                };

                var dt = await _service.InsertRecordAsync(userid, finalRequest);
                return dt.Rows.Count > 0
                    ? Ok(ApiResponseHelper.Sucess(null, dt.Rows[0][0].ToString()))
                    : Ok(ApiResponseHelper.Fail("No response received"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inserting record");
                return StatusCode(500, ApiResponseHelper.Fail("An error occurred"));
            }
        }

        [HttpPut("updateRecord")]
        public async Task<IActionResult> UpdateRecord([FromBody] UserRequestDto request)
        {
            try
            {
                var userid = GetUserId();
                if (string.IsNullOrEmpty(userid)) return Ok(ApiResponseHelper.Fail("userid not found"));
                if (string.IsNullOrEmpty(request.recordId)) return Ok(ApiResponseHelper.Fail("Empty Record ID"));

                var data = request.data;
                if (data.ValueKind == JsonValueKind.Null || data.ValueKind == JsonValueKind.Undefined)
                    return Ok(ApiResponseHelper.Fail("Invalid/Empty data"));
                var password = data.TryGetProperty("password", out var pw) ? pw.GetString() : null;
                if (!string.IsNullOrEmpty(password))
                {
                    var hash = BCrypt.Net.BCrypt.HashPassword(password);
                    var jsonDict = JsonSerializer.Deserialize<Dictionary<string, object>>(data.GetRawText());
                    jsonDict["passwordHash"] = hash;
                    data = JsonSerializer.SerializeToElement(jsonDict);
                }

                var finalRequest = new UserRequestDto
                {
                    formId = request.formId,
                    data = data,
                    recordId = request.recordId
                };

                var dt = await _service.UpdateRecordAsync(userid, finalRequest);
                return dt.Rows.Count > 0
                    ? Ok(ApiResponseHelper.Sucess(null, dt.Rows[0][0].ToString()))
                    : Ok(ApiResponseHelper.Fail("No response received"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating record");
                return StatusCode(500, ApiResponseHelper.Fail("An error occurred"));
            }
        }

        [HttpDelete("deleteRecord/{id}")]
        public async Task<IActionResult> DeleteRecord(int id)
        {
            try
            {
                var userid = GetUserId();
                if (string.IsNullOrEmpty(userid)) return Ok(ApiResponseHelper.Fail("userid not found"));

                var request = new UserRequestDto { recordId = id.ToString(), data = JsonDocument.Parse("{}").RootElement };
                var dt = await _service.DeleteRecordAsync(userid, request);
                return dt.Rows.Count > 0
                    ? Ok(ApiResponseHelper.Sucess(null, dt.Rows[0][0].ToString()))
                    : Ok(ApiResponseHelper.Fail("No response received"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting record");
                return StatusCode(500, ApiResponseHelper.Fail("An error occurred"));
            }
        }
    }
}