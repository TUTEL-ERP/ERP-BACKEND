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
    public class ChartOfAccountsController : ControllerBase
    {
        private readonly IChartOfAccountsService _service;
        private readonly ILogger<ChartOfAccountsController> _logger;

        public ChartOfAccountsController(IChartOfAccountsService service, ILogger<ChartOfAccountsController> logger)
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

        // ─── GRID ─────────────────────────────────────────────────
        [HttpGet("grid/{formName}")]
        public async Task<IActionResult> GetGridData(string formName)
        {
            try
            {
                var userid = GetUserId();
                if (string.IsNullOrEmpty(userid)) return Ok(ApiResponseHelper.Fail("userid not found"));

                var request = new ChartOfAccountsRequestDto
                {
                    formId = formName,
                    data = JsonDocument.Parse("{}").RootElement
                };

                var dt = await _service.GetGridDataAsync(userid, request);
                return Ok(ApiResponseHelper.Sucess(DataTableHelper.ToDynamicList(dt), "Data retrieved successfully"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting grid data");
                return StatusCode(500, ApiResponseHelper.Fail("An error occurred"));
            }
        }

        // ─── SINGLE RECORD ────────────────────────────────────────
        [HttpGet("record/{id}")]
        public async Task<IActionResult> SelectRecord(int id)
        {
            try
            {
                var userid = GetUserId();
                if (string.IsNullOrEmpty(userid)) return Ok(ApiResponseHelper.Fail("userid not found"));

                var request = new ChartOfAccountsRequestDto
                {
                    recordId = id.ToString(),
                    data = JsonDocument.Parse("{}").RootElement
                };

                var dt = await _service.GetSingleRecordAsync(userid, request);
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

        // ─── PARENT ACCOUNTS (dropdown) ───────────────────────────
        [HttpGet("parent-accounts")]
        public async Task<IActionResult> GetParentAccounts()
        {
            try
            {
                var userid = GetUserId();
                if (string.IsNullOrEmpty(userid)) return Ok(ApiResponseHelper.Fail("userid not found"));

                var request = new ChartOfAccountsRequestDto { data = JsonDocument.Parse("{}").RootElement };
                var dt = await _service.GetParentAccountsAsync(userid, request);
                return Ok(ApiResponseHelper.Sucess(DataTableHelper.ToDynamicList(dt), "Parent accounts retrieved"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting parent accounts");
                return StatusCode(500, ApiResponseHelper.Fail("An error occurred"));
            }
        }

        // ─── NEXT ACCOUNT CODE ────────────────────────────────────
        [HttpGet("next-code")]
        public async Task<IActionResult> GetNextAccountCode([FromQuery] string accountType, [FromQuery] int? parentAccountId)
        {
            try
            {
                var userid = GetUserId();
                if (string.IsNullOrEmpty(userid)) return Ok(ApiResponseHelper.Fail("userid not found"));

                var payload = JsonDocument.Parse(
                    JsonSerializer.Serialize(new { accountType, parentAccountId })
                ).RootElement;

                var request = new ChartOfAccountsRequestDto { data = payload };
                var dt = await _service.GetNextAccountCodeAsync(userid, request);

                if (dt.Rows.Count > 0)
                    return Ok(ApiResponseHelper.Sucess(new { accountCode = dt.Rows[0][0]?.ToString() }, "Next code"));
                return Ok(ApiResponseHelper.Fail("No code generated"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting next code");
                return StatusCode(500, ApiResponseHelper.Fail("An error occurred"));
            }
        }

        // ─── TREE VIEW ────────────────────────────────────────────
        [HttpGet("tree")]
        public async Task<IActionResult> GetTree()
        {
            try
            {
                var userid = GetUserId();
                if (string.IsNullOrEmpty(userid)) return Ok(ApiResponseHelper.Fail("userid not found"));

                var request = new ChartOfAccountsRequestDto { data = JsonDocument.Parse("{}").RootElement };
                var dt = await _service.GetTreeAsync(userid, request);
                return Ok(ApiResponseHelper.Sucess(DataTableHelper.ToDynamicList(dt), "Tree retrieved"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting tree");
                return StatusCode(500, ApiResponseHelper.Fail("An error occurred"));
            }
        }

        // ─── INSERT ───────────────────────────────────────────────
        [HttpPost("insertRecord")]
        public async Task<IActionResult> InsertRecord([FromBody] ChartOfAccountsRequestDto request)
        {
            try
            {
                var userid = GetUserId();
                if (string.IsNullOrEmpty(userid)) return Ok(ApiResponseHelper.Fail("userid not found"));

                var dt = await _service.InsertRecordAsync(userid, request);
                return dt.Rows.Count > 0
                    ? Ok(ApiResponseHelper.Sucess(null, dt.Rows[0][0].ToString()))
                    : Ok(ApiResponseHelper.Fail("No response"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inserting");
                return StatusCode(500, ApiResponseHelper.Fail("An error occurred"));
            }
        }

        // ─── UPDATE ───────────────────────────────────────────────
        [HttpPut("updateRecord")]
        public async Task<IActionResult> UpdateRecord([FromBody] ChartOfAccountsRequestDto request)
        {
            try
            {
                var userid = GetUserId();
                if (string.IsNullOrEmpty(userid)) return Ok(ApiResponseHelper.Fail("userid not found"));
                if (string.IsNullOrEmpty(request.recordId)) return Ok(ApiResponseHelper.Fail("Empty Record ID"));

                var dt = await _service.UpdateRecordAsync(userid, request);
                return dt.Rows.Count > 0
                    ? Ok(ApiResponseHelper.Sucess(null, dt.Rows[0][0].ToString()))
                    : Ok(ApiResponseHelper.Fail("No response"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating");
                return StatusCode(500, ApiResponseHelper.Fail("An error occurred"));
            }
        }

        // ─── DELETE ───────────────────────────────────────────────
        [HttpDelete("deleteRecord/{id}")]
        public async Task<IActionResult> DeleteRecord(int id)
        {
            try
            {
                var userid = GetUserId();
                if (string.IsNullOrEmpty(userid)) return Ok(ApiResponseHelper.Fail("userid not found"));

                var request = new ChartOfAccountsRequestDto
                {
                    recordId = id.ToString(),
                    data = JsonDocument.Parse("{}").RootElement
                };

                var dt = await _service.DeleteRecordAsync(userid, request);
                return dt.Rows.Count > 0
                    ? Ok(ApiResponseHelper.Sucess(null, dt.Rows[0][0].ToString()))
                    : Ok(ApiResponseHelper.Fail("No response"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting");
                return StatusCode(500, ApiResponseHelper.Fail("An error occurred"));
            }
        }
    }
}