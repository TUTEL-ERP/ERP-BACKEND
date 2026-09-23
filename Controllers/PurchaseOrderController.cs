using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using server.Dto;
using server.Helpers;
using server.Interfaces.Services;
using System.Data;
using System.Security.Claims;
using System.Text.Json;

namespace server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class PurchaseOrderController : ControllerBase
    {
        private readonly IPurchaseOrderService _service;
        private readonly ILogger<PurchaseOrderController> _logger;

        public PurchaseOrderController(IPurchaseOrderService service, ILogger<PurchaseOrderController> logger)
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

                var request = new PurchaseOrderRequestDto { formId = formName, data = JsonDocument.Parse("{}").RootElement };
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

                var request = new PurchaseOrderRequestDto { recordId = id.ToString(), data = JsonDocument.Parse("{}").RootElement };
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

        [HttpGet("items")]
        public async Task<IActionResult> GetAllItems()
        {
            try
            {
                var userid = GetUserId();
                if (string.IsNullOrEmpty(userid)) return Ok(ApiResponseHelper.Fail("userid not found"));

                var request = new PurchaseOrderRequestDto { data = JsonDocument.Parse("{}").RootElement };
                var dt = await _service.GetAllItemsAsync(userid, request);
                return Ok(ApiResponseHelper.Sucess(DataTableHelper.ToDynamicList(dt), "Items retrieved"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting items");
                return StatusCode(500, ApiResponseHelper.Fail("An error occurred"));
            }
        }

        [HttpGet("next-po-number")]
        public async Task<IActionResult> GetNextPONumber()
        {
            try
            {
                var userid = GetUserId();
                if (string.IsNullOrEmpty(userid)) return Ok(ApiResponseHelper.Fail("userid not found"));

                var request = new PurchaseOrderRequestDto { data = JsonDocument.Parse("{}").RootElement };
                var poNumber = await _service.GetNextPONumberAsync(userid, request);
                return Ok(ApiResponseHelper.Sucess(new { poNumber }, "Next PO Number"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting PO number");
                return StatusCode(500, ApiResponseHelper.Fail("An error occurred"));
            }
        }

        [HttpPost("insertRecord")]
        public async Task<IActionResult> InsertRecord([FromBody] PurchaseOrderRequestDto request)
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

        [HttpPut("updateRecord")]
        public async Task<IActionResult> UpdateRecord([FromBody] PurchaseOrderRequestDto request)
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

        [HttpDelete("deleteRecord/{id}")]
        public async Task<IActionResult> DeleteRecord(int id)
        {
            try
            {
                var userid = GetUserId();
                if (string.IsNullOrEmpty(userid)) return Ok(ApiResponseHelper.Fail("userid not found"));

                var request = new PurchaseOrderRequestDto { recordId = id.ToString(), data = JsonDocument.Parse("{}").RootElement };
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
        // ✅ GET: Approved PR list (for LOV modal)
        [HttpGet("approved-pr-list")]
        public async Task<IActionResult> GetApprovedPRList()
        {
            try
            {
                var userid = GetUserId();
                if (string.IsNullOrEmpty(userid)) return Ok(ApiResponseHelper.Fail("userid not found"));

                var request = new PurchaseOrderRequestDto
                {
                    formId = "PurchaseOrder",
                    data = JsonDocument.Parse("{}").RootElement
                };

                // Uses the same repo method (action = LOV_APPROVED_PR)
                var ds = await _service.GetPRDetailsAsync_ApprovedPR(userid, request); // see below
                var dt = ds.Tables.Count > 0 ? ds.Tables[0] : new DataTable();

                return Ok(ApiResponseHelper.Sucess(DataTableHelper.ToDynamicList(dt), "Approved PRs retrieved"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting approved PR list");
                return StatusCode(500, ApiResponseHelper.Fail("An error occurred"));
            }
        }

        [HttpGet("pr-details/{requisitionId}")]
        public async Task<IActionResult> GetPRDetails(int requisitionId)
        {
            try
            {
                var userid = GetUserId();
                if (string.IsNullOrEmpty(userid)) return Ok(ApiResponseHelper.Fail("userid not found"));

                var payload = JsonDocument.Parse(
                    JsonSerializer.Serialize(new { requisitionId })
                ).RootElement;

                var request = new PurchaseOrderRequestDto { data = payload };
                var ds = await _service.GetPRDetailsAsync(userid, request);

                if (ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
                    return Ok(ApiResponseHelper.Fail("PR not found"));

                var master = DataTableHelper.ToDynamicList(ds.Tables[0]).FirstOrDefault();
                var lines = ds.Tables.Count > 1
                    ? DataTableHelper.ToDynamicList(ds.Tables[1])
                    : new List<dynamic>();

                return Ok(ApiResponseHelper.Sucess(new { master, lines }, "PR details retrieved"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting PR details");
                return StatusCode(500, ApiResponseHelper.Fail("An error occurred"));
            }
        }
    }
}