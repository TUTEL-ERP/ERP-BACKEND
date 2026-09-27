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
    public class SaleController : ControllerBase
    {
        private readonly ISaleService _service;
        private readonly ILogger<SaleController> _logger;

        public SaleController(ISaleService service, ILogger<SaleController> logger)
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

                var request = new SaleRequestDto
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

                var request = new SaleRequestDto
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

        // ─── CUSTOMERS (for dropdown) ─────────────────────────────
        [HttpGet("customers")]
        public async Task<IActionResult> GetAllCustomers()
        {
            try
            {
                var userid = GetUserId();
                if (string.IsNullOrEmpty(userid)) return Ok(ApiResponseHelper.Fail("userid not found"));

                var request = new SaleRequestDto { data = JsonDocument.Parse("{}").RootElement };
                var dt = await _service.GetAllCustomersAsync(userid, request);
                return Ok(ApiResponseHelper.Sucess(DataTableHelper.ToDynamicList(dt), "Customers retrieved"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting customers");
                return StatusCode(500, ApiResponseHelper.Fail("An error occurred"));
            }
        }

        // ─── ITEMS (for dropdown) ─────────────────────────────────
        [HttpGet("items")]
        public async Task<IActionResult> GetAllItems()
        {
            try
            {
                var userid = GetUserId();
                if (string.IsNullOrEmpty(userid)) return Ok(ApiResponseHelper.Fail("userid not found"));

                var request = new SaleRequestDto { data = JsonDocument.Parse("{}").RootElement };
                var dt = await _service.GetAllItemsAsync(userid, request);
                return Ok(ApiResponseHelper.Sucess(DataTableHelper.ToDynamicList(dt), "Items retrieved"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting items");
                return StatusCode(500, ApiResponseHelper.Fail("An error occurred"));
            }
        }

        // ─── NEXT SALE NUMBER ─────────────────────────────────────
        [HttpGet("next-sale-number")]
        public async Task<IActionResult> GetNextSaleNumber()
        {
            try
            {
                var userid = GetUserId();
                if (string.IsNullOrEmpty(userid)) return Ok(ApiResponseHelper.Fail("userid not found"));

                var request = new SaleRequestDto { data = JsonDocument.Parse("{}").RootElement };
                var no = await _service.GetNextSaleNumberAsync(userid, request);
                return Ok(ApiResponseHelper.Sucess(new { saleNo = no }, "Next Sale Number"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting sale number");
                return StatusCode(500, ApiResponseHelper.Fail("An error occurred"));
            }
        }

        // ─── INSERT ───────────────────────────────────────────────
        [HttpPost("insertRecord")]
        public async Task<IActionResult> InsertRecord([FromBody] SaleRequestDto request)
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
        public async Task<IActionResult> UpdateRecord([FromBody] SaleRequestDto request)
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

                var request = new SaleRequestDto
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