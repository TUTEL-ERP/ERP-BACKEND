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
    public class PurchaseReturnController : ControllerBase
    {
        private readonly IPurchaseReturnService _service;
        private readonly ILogger<PurchaseReturnController> _logger;

        public PurchaseReturnController(IPurchaseReturnService service, ILogger<PurchaseReturnController> logger)
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

                var request = new PurchaseReturnRequestDto
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

        // ─── SINGLE RECORD (Master + Lines) ──────────────────────
        [HttpGet("record/{id}")]
        public async Task<IActionResult> SelectRecord(int id)
        {
            try
            {
                var userid = GetUserId();
                if (string.IsNullOrEmpty(userid)) return Ok(ApiResponseHelper.Fail("userid not found"));

                var request = new PurchaseReturnRequestDto
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

        // ─── ITEMS LOV ───────────────────────────────────────────
        [HttpGet("items")]
        public async Task<IActionResult> GetAllItems()
        {
            try
            {
                var userid = GetUserId();
                if (string.IsNullOrEmpty(userid)) return Ok(ApiResponseHelper.Fail("userid not found"));

                var request = new PurchaseReturnRequestDto { data = JsonDocument.Parse("{}").RootElement };
                var dt = await _service.GetAllItemsAsync(userid, request);
                return Ok(ApiResponseHelper.Sucess(DataTableHelper.ToDynamicList(dt), "Items retrieved"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting items");
                return StatusCode(500, ApiResponseHelper.Fail("An error occurred"));
            }
        }

        // ─── SUPPLIERS (for dropdown) ✅ NEW ─────────────────────
        [HttpGet("suppliers")]
        public async Task<IActionResult> GetAllSuppliers()
        {
            try
            {
                var userid = GetUserId();
                if (string.IsNullOrEmpty(userid)) return Ok(ApiResponseHelper.Fail("userid not found"));

                var request = new PurchaseReturnRequestDto { data = JsonDocument.Parse("{}").RootElement };
                var dt = await _service.GetAllSuppliersAsync(userid, request);
                return Ok(ApiResponseHelper.Sucess(DataTableHelper.ToDynamicList(dt), "Suppliers retrieved"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting suppliers");
                return StatusCode(500, ApiResponseHelper.Fail("An error occurred"));
            }
        }

        // ─── NEXT RETURN NUMBER ─────────────────────────────────
        [HttpGet("next-return-number")]
        public async Task<IActionResult> GetNextReturnNumber()
        {
            try
            {
                var userid = GetUserId();
                if (string.IsNullOrEmpty(userid)) return Ok(ApiResponseHelper.Fail("userid not found"));

                var request = new PurchaseReturnRequestDto { data = JsonDocument.Parse("{}").RootElement };
                var no = await _service.GetNextReturnNumberAsync(userid, request);
                return Ok(ApiResponseHelper.Sucess(new { purchaseReturnNo = no }, "Next Return Number"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting return number");
                return StatusCode(500, ApiResponseHelper.Fail("An error occurred"));
            }
        }

        // ─── INSERT ──────────────────────────────────────────────
        [HttpPost("insertRecord")]
        public async Task<IActionResult> InsertRecord([FromBody] PurchaseReturnRequestDto request)
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

        // ─── UPDATE ──────────────────────────────────────────────
        [HttpPut("updateRecord")]
        public async Task<IActionResult> UpdateRecord([FromBody] PurchaseReturnRequestDto request)
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

        // ─── DELETE ──────────────────────────────────────────────
        [HttpDelete("deleteRecord/{id}")]
        public async Task<IActionResult> DeleteRecord(int id)
        {
            try
            {
                var userid = GetUserId();
                if (string.IsNullOrEmpty(userid)) return Ok(ApiResponseHelper.Fail("userid not found"));

                var request = new PurchaseReturnRequestDto
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