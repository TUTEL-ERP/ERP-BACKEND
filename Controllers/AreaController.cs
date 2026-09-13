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
    public class AreaController : ControllerBase
    {
        private readonly IAreaService _areaService;
        private readonly ILogger<AreaController> _logger;

        public AreaController(IAreaService areaService, ILogger<AreaController> logger)
        {
            _areaService = areaService;
            _logger = logger;
        }

        private string GetUserId()
        {
            return User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        }

        [HttpGet("grid/{formName}")]
        public async Task<IActionResult> GetGridData(string formName)
        {
            try
            {
                var userid = GetUserId();
                if (string.IsNullOrEmpty(userid))
                    return Ok(ApiResponseHelper.Fail("userid not found"));

                var request = new AreaRequestDto
                {
                    formId = formName,
                    data = JsonDocument.Parse("{}").RootElement
                };

                var dataTable = await _areaService.GetGridDataAsync(userid, request);
                var cleanData = DataTableHelper.ToDynamicList(dataTable);

                return Ok(ApiResponseHelper.Sucess(cleanData, "Data retrieved successfully"));
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
                if (string.IsNullOrEmpty(userid))
                    return Ok(ApiResponseHelper.Fail("userid not found"));

                var request = new AreaRequestDto
                {
                    recordId = id.ToString(),
                    data = JsonDocument.Parse("{}").RootElement
                };

                var dataTable = await _areaService.SelectRecordAsync(userid, request);
                var cleanData = DataTableHelper.ToDynamicList(dataTable);

                if (cleanData.Count > 0)
                    return Ok(ApiResponseHelper.Sucess(cleanData.FirstOrDefault(), "Data retrieved successfully"));
                else
                    return Ok(ApiResponseHelper.Fail("Record not found"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting record");
                return StatusCode(500, ApiResponseHelper.Fail("An error occurred"));
            }
        }

        [HttpPost("insertRecord")]
        public async Task<IActionResult> InsertRecord([FromBody] AreaRequestDto request)
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

                var dataTable = await _areaService.InsertRecordAsync(userid, request);

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

        [HttpPut("updateRecord")]
        public async Task<IActionResult> UpdateRecord([FromBody] AreaRequestDto request)
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

                var dataTable = await _areaService.UpdateRecordAsync(userid, request);

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

        [HttpDelete("deleteRecord/{id}")]
        public async Task<IActionResult> DeleteRecord(int id)
        {
            try
            {
                var userid = GetUserId();
                if (string.IsNullOrEmpty(userid))
                    return Ok(ApiResponseHelper.Fail("userid not found"));

                var request = new AreaRequestDto
                {
                    recordId = id.ToString(),
                    data = JsonDocument.Parse("{}").RootElement
                };

                var dataTable = await _areaService.DeleteRecordAsync(userid, request);

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