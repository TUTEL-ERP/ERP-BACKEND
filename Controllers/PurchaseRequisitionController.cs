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
    public class PurchaseRequisitionController : ControllerBase
    {
        private readonly IPurchaseRequisitionService _service;
        private readonly ILogger<PurchaseRequisitionController> _logger;

        public PurchaseRequisitionController(IPurchaseRequisitionService service, ILogger<PurchaseRequisitionController> logger)
        {
            _service = service;
            _logger = logger;
        }

        private string GetUserId()
        {
            var userid = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userid)) userid = User.FindFirst("userID")?.Value;
            return userid;
        }

        [HttpGet("grid/{formName}")]
        public async Task<IActionResult> GetGridData(string formName)
        {
            try
            {
                var userid = GetUserId();
                if (string.IsNullOrEmpty(userid)) return Ok(ApiResponseHelper.Fail("userid not found"));

                var request = new PurchaseRequisitionRequestDto { formId = formName, data = JsonDocument.Parse("{}").RootElement };
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

                var request = new PurchaseRequisitionRequestDto { recordId = id.ToString(), data = JsonDocument.Parse("{}").RootElement };
                var ds = await _service.GetMasterDetailAsync(userid, request);

                if (ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
                    return Ok(ApiResponseHelper.Fail("Record not found"));

                // ✅ Get Master
                var master = DataTableHelper.ToDynamicList(ds.Tables[0]).FirstOrDefault();

                // ✅ Get Lines (if exists)
                var lines = ds.Tables.Count > 1
                    ? DataTableHelper.ToDynamicList(ds.Tables[1])
                    : new List<dynamic>();

                // ✅ Combine into single response object
                var result = new
                {
                    master = master,
                    lines = lines
                };

                return Ok(ApiResponseHelper.Sucess(result, "Data retrieved successfully"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting record");
                return StatusCode(500, ApiResponseHelper.Fail("An error occurred"));
            }
        }
        [HttpPost("insertRecord")]
        public async Task<IActionResult> InsertRecord([FromBody] PurchaseRequisitionRequestDto request)
        {
            try
            {
                var userid = GetUserId();
                if (string.IsNullOrEmpty(userid)) return Ok(ApiResponseHelper.Fail("userid not found"));

                var data = request.data;
                if (data.ValueKind == JsonValueKind.Null || data.ValueKind == JsonValueKind.Undefined)
                    return Ok(ApiResponseHelper.Fail("Invalid/Empty data"));

                var dt = await _service.InsertRecordAsync(userid, request);
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
        public async Task<IActionResult> UpdateRecord([FromBody] PurchaseRequisitionRequestDto request)
        {
            try
            {
                var userid = GetUserId();
                if (string.IsNullOrEmpty(userid)) return Ok(ApiResponseHelper.Fail("userid not found"));
                if (string.IsNullOrEmpty(request.recordId)) return Ok(ApiResponseHelper.Fail("Empty Record ID"));

                var dt = await _service.UpdateRecordAsync(userid, request);
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
        [HttpGet("GetAllItems")]
        public async Task<IActionResult> GetAllItems()
        {
            try
            {
                var userid = GetUserId();
                if (string.IsNullOrEmpty(userid))
                    return Ok(ApiResponseHelper.Fail("userid not found"));

                var request = new PurchaseRequisitionRequestDto
                {
                    formId = "PurchaseRequisition",
                    data = JsonDocument.Parse("{}").RootElement
                };

                var dt = await _service.GetAllItemsAsync(userid, request);
                var cleanData = DataTableHelper.ToDynamicList(dt);

                return Ok(ApiResponseHelper.Sucess(cleanData, "Items retrieved successfully"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting items");
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

                var request = new PurchaseRequisitionRequestDto { recordId = id.ToString(), data = JsonDocument.Parse("{}").RootElement };
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