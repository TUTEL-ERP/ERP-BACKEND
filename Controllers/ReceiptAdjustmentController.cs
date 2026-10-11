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
    public class ReceiptAdjustmentController : ControllerBase
    {
        private readonly IReceiptAdjustmentService _service;
        private readonly IPermissionService _permissionService;
        private readonly ILogger<ReceiptAdjustmentController> _logger;

        private const string FORM_ID = "receipt-adjustment";

        public ReceiptAdjustmentController(
            IReceiptAdjustmentService service,
            IPermissionService permissionService,
            ILogger<ReceiptAdjustmentController> logger)
        {
            _service = service;
            _permissionService = permissionService;
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

                var allowed = await _permissionService.CheckPermissionAsync(userid, FORM_ID, "VIEW");
                if (!allowed) return Ok(ApiResponseHelper.Fail("No permission to view."));

                var request = new ReceiptAdjustmentRequestDto { formId = formName, data = JsonDocument.Parse("{}").RootElement };
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

                var allowed = await _permissionService.CheckPermissionAsync(userid, FORM_ID, "VIEW");
                if (!allowed) return Ok(ApiResponseHelper.Fail("No permission to view."));

                var request = new ReceiptAdjustmentRequestDto { recordId = id.ToString(), data = JsonDocument.Parse("{}").RootElement };
                var ds = await _service.GetMasterDetailAsync(userid, request);

                if (ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
                    return Ok(ApiResponseHelper.Fail("Record not found"));

                var master = DataTableHelper.ToDynamicList(ds.Tables[0]).FirstOrDefault();
                var lines = ds.Tables.Count > 1 ? DataTableHelper.ToDynamicList(ds.Tables[1]) : new List<dynamic>();

                return Ok(ApiResponseHelper.Sucess(new { master, lines }, "Data retrieved successfully"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting record");
                return StatusCode(500, ApiResponseHelper.Fail("An error occurred"));
            }
        }

        [HttpGet("customers")]
        public async Task<IActionResult> GetCustomers()
        {
            try
            {
                var userid = GetUserId();
                if (string.IsNullOrEmpty(userid)) return Ok(ApiResponseHelper.Fail("userid not found"));

                var request = new ReceiptAdjustmentRequestDto { data = JsonDocument.Parse("{}").RootElement };
                var dt = await _service.GetAllCustomersAsync(userid, request);
                return Ok(ApiResponseHelper.Sucess(DataTableHelper.ToDynamicList(dt), "Customers retrieved"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting customers");
                return StatusCode(500, ApiResponseHelper.Fail("An error occurred"));
            }
        }

        [HttpGet("receipts/{customerId}")]
        public async Task<IActionResult> GetReceiptsByCustomer(int customerId)
        {
            try
            {
                var userid = GetUserId();
                if (string.IsNullOrEmpty(userid)) return Ok(ApiResponseHelper.Fail("userid not found"));

                var payload = JsonDocument.Parse(JsonSerializer.Serialize(new { customerId })).RootElement;
                var request = new ReceiptAdjustmentRequestDto { data = payload };
                var dt = await _service.GetReceiptsByCustomerAsync(userid, request);
                return Ok(ApiResponseHelper.Sucess(DataTableHelper.ToDynamicList(dt), "Receipts retrieved"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting receipts");
                return StatusCode(500, ApiResponseHelper.Fail("An error occurred"));
            }
        }

        [HttpGet("next-adjustment-number")]
        public async Task<IActionResult> GetNextNumber()
        {
            try
            {
                var userid = GetUserId();
                if (string.IsNullOrEmpty(userid)) return Ok(ApiResponseHelper.Fail("userid not found"));

                var request = new ReceiptAdjustmentRequestDto { data = JsonDocument.Parse("{}").RootElement };
                var no = await _service.GetNextAdjustmentNumberAsync(userid, request);
                return Ok(ApiResponseHelper.Sucess(new { adjustmentNo = no }, "Next Number"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting number");
                return StatusCode(500, ApiResponseHelper.Fail("An error occurred"));
            }
        }

        [HttpPost("insertRecord")]
        public async Task<IActionResult> InsertRecord([FromBody] ReceiptAdjustmentRequestDto request)
        {
            try
            {
                var userid = GetUserId();
                if (string.IsNullOrEmpty(userid)) return Ok(ApiResponseHelper.Fail("userid not found"));

                var allowed = await _permissionService.CheckPermissionAsync(userid, FORM_ID, "ADD");
                if (!allowed) return Ok(ApiResponseHelper.Fail("No permission to add."));

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
        public async Task<IActionResult> UpdateRecord([FromBody] ReceiptAdjustmentRequestDto request)
        {
            try
            {
                var userid = GetUserId();
                if (string.IsNullOrEmpty(userid)) return Ok(ApiResponseHelper.Fail("userid not found"));
                if (string.IsNullOrEmpty(request.recordId)) return Ok(ApiResponseHelper.Fail("Empty Record ID"));

                var allowed = await _permissionService.CheckPermissionAsync(userid, FORM_ID, "EDIT");
                if (!allowed) return Ok(ApiResponseHelper.Fail("No permission to edit."));

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

        [HttpPost("approve/{recordId}")]
        public async Task<IActionResult> Approve(int recordId)
        {
            try
            {
                var userid = GetUserId();
                if (string.IsNullOrEmpty(userid)) return Ok(ApiResponseHelper.Fail("userid not found"));

                var allowed = await _permissionService.CheckPermissionAsync(userid, FORM_ID, "APPROVE");
                if (!allowed) return Ok(ApiResponseHelper.Fail("No permission to approve."));

                var request = new ReceiptAdjustmentRequestDto
                {
                    recordId = recordId.ToString(),
                    data = JsonDocument.Parse("{}").RootElement
                };
                var dt = await _service.ApproveAsync(userid, request);
                return dt.Rows.Count > 0
                    ? Ok(ApiResponseHelper.Sucess(null, dt.Rows[0][0].ToString()))
                    : Ok(ApiResponseHelper.Fail("No response"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error approving");
                return StatusCode(500, ApiResponseHelper.Fail("An error occurred"));
            }
        }

        [HttpPost("reject/{recordId}")]
        public async Task<IActionResult> Reject(int recordId, [FromBody] JsonElement body)
        {
            try
            {
                var userid = GetUserId();
                if (string.IsNullOrEmpty(userid)) return Ok(ApiResponseHelper.Fail("userid not found"));

                var allowed = await _permissionService.CheckPermissionAsync(userid, FORM_ID, "REJECT");
                if (!allowed) return Ok(ApiResponseHelper.Fail("No permission to reject."));

                var payload = JsonDocument.Parse(JsonSerializer.Serialize(new
                {
                    reason = body.ValueKind != JsonValueKind.Undefined
                             ? body.TryGetProperty("reason", out var r) ? r.GetString() : null
                             : null
                })).RootElement;

                var request = new ReceiptAdjustmentRequestDto
                {
                    recordId = recordId.ToString(),
                    data = payload
                };
                var dt = await _service.RejectAsync(userid, request);
                return dt.Rows.Count > 0
                    ? Ok(ApiResponseHelper.Sucess(null, dt.Rows[0][0].ToString()))
                    : Ok(ApiResponseHelper.Fail("No response"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error rejecting");
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

                var allowed = await _permissionService.CheckPermissionAsync(userid, FORM_ID, "DELETE");
                if (!allowed) return Ok(ApiResponseHelper.Fail("No permission to delete."));

                var request = new ReceiptAdjustmentRequestDto
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