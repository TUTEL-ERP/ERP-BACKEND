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
    public class JournalEntryController : ControllerBase
    {
        private readonly IJournalEntryService _service;
        private readonly ILogger<JournalEntryController> _logger;

        public JournalEntryController(IJournalEntryService service, ILogger<JournalEntryController> logger)
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

        // ─── GRID ────────────────────────────────────────────────
        [HttpGet("grid/{formName}")]
        public async Task<IActionResult> GetGridData(string formName)
        {
            try
            {
                var userid = GetUserId();
                if (string.IsNullOrEmpty(userid)) return Ok(ApiResponseHelper.Fail("userid not found"));

                var request = new JournalEntryRequestDto
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

                var request = new JournalEntryRequestDto
                {
                    recordId = id.ToString(),
                    data = JsonDocument.Parse("{}").RootElement
                };

                var ds = await _service.GetMasterDetailAsync(userid, request);

                if (ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
                    return Ok(ApiResponseHelper.Fail("Record not found"));

                var master = DataTableHelper.ToDynamicList(ds.Tables[0]).FirstOrDefault();
                var lines = ds.Tables.Count > 1
                    ? DataTableHelper.ToDynamicList(ds.Tables[1])
                    : new List<dynamic>();

                return Ok(ApiResponseHelper.Sucess(new { master, lines }, "Data retrieved successfully"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting record");
                return StatusCode(500, ApiResponseHelper.Fail("An error occurred"));
            }
        }

        // ─── ACCOUNTS (dropdown) ──────────────────────────────────
        [HttpGet("accounts")]
        public async Task<IActionResult> GetAccounts()
        {
            try
            {
                var userid = GetUserId();
                if (string.IsNullOrEmpty(userid)) return Ok(ApiResponseHelper.Fail("userid not found"));

                var request = new JournalEntryRequestDto { data = JsonDocument.Parse("{}").RootElement };
                var dt = await _service.GetAccountsAsync(userid, request);
                return Ok(ApiResponseHelper.Sucess(DataTableHelper.ToDynamicList(dt), "Accounts retrieved"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting accounts");
                return StatusCode(500, ApiResponseHelper.Fail("An error occurred"));
            }
        }

        // ─── NEXT JOURNAL NUMBER ──────────────────────────────────
        [HttpGet("next-journal-number")]
        public async Task<IActionResult> GetNextJournalNumber([FromQuery] string? voucherType = "JV")
        {
            try
            {
                var userid = GetUserId();
                if (string.IsNullOrEmpty(userid)) return Ok(ApiResponseHelper.Fail("userid not found"));

                var payload = JsonDocument.Parse(
                    JsonSerializer.Serialize(new { voucherType = voucherType ?? "JV" })
                ).RootElement;

                var request = new JournalEntryRequestDto { data = payload };
                var no = await _service.GetNextJournalNumberAsync(userid, request);
                return Ok(ApiResponseHelper.Sucess(new { journalNo = no }, "Next Journal Number"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting journal number");
                return StatusCode(500, ApiResponseHelper.Fail("An error occurred"));
            }
        }

        // ─── INSERT ───────────────────────────────────────────────
        [HttpPost("insertRecord")]
        public async Task<IActionResult> InsertRecord([FromBody] JournalEntryRequestDto request)
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
        public async Task<IActionResult> UpdateRecord([FromBody] JournalEntryRequestDto request)
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

                var request = new JournalEntryRequestDto
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