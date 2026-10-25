using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using server.Dto;
using server.Helpers;
using server.Interfaces.Services;
using System.Data;
using System.Globalization;
using System.Security.Claims;
using System.Text.Json;

namespace server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class DepartmentalConsumptionController : ControllerBase
    {
        private const string FormId = "DepartmentalConsumption";
        private readonly IDepartmentalConsumptionService _service;
        private readonly IPermissionService _permissionService;
        private readonly ILogger<DepartmentalConsumptionController> _logger;

        public DepartmentalConsumptionController(
            IDepartmentalConsumptionService service,
            IPermissionService permissionService,
            ILogger<DepartmentalConsumptionController> logger)
        {
            _service = service;
            _permissionService = permissionService;
            _logger = logger;
        }

        private string? GetUserId() => User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        private string GetUserName() => User.FindFirst(ClaimTypes.Name)?.Value ?? "unknown";

        [HttpGet("grid/{formName}")]
        public async Task<IActionResult> GetGridData(string formName)
        {
            try
            {
                var userId = GetUserId();
                if (string.IsNullOrWhiteSpace(userId))
                    return Unauthorized(ApiResponseHelper.Fail("userid not found"));
                if (!await HasPermission(userId, "VIEW"))
                    return Forbid();

                var request = new DepartmentalConsumptionDto
                {
                    formId = formName,
                    data = JsonDocument.Parse("{}").RootElement
                };
                var data = await _service.GetGridDataAsync(userId, request);
                return Ok(ApiResponseHelper.Sucess(
                    DataTableHelper.ToDynamicList(data), "Data retrieved successfully"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting Departmental Consumption grid");
                return StatusCode(500, ApiResponseHelper.Fail("An error occurred"));
            }
        }

        [HttpGet("record/{id:long}")]
        public async Task<IActionResult> GetRecord(long id)
        {
            try
            {
                var userId = GetUserId();
                if (string.IsNullOrWhiteSpace(userId))
                    return Unauthorized(ApiResponseHelper.Fail("userid not found"));
                if (!await HasPermission(userId, "VIEW"))
                    return Forbid();

                var request = new DepartmentalConsumptionDto
                {
                    recordId = id.ToString(),
                    data = JsonDocument.Parse("{}").RootElement
                };
                var dataSet = await _service.GetMasterDetailAsync(userId, request);
                if (dataSet.Tables.Count == 0 || dataSet.Tables[0].Rows.Count == 0)
                    return NotFound(ApiResponseHelper.Fail("Record not found"));

                var master = DataTableHelper.ToDynamicList(dataSet.Tables[0]).FirstOrDefault();
                var lines = dataSet.Tables.Count > 1
                    ? DataTableHelper.ToDynamicList(dataSet.Tables[1])
                    : new List<dynamic>();
                return Ok(ApiResponseHelper.Sucess(new { master, lines }, "Data retrieved successfully"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting Departmental Consumption record {RecordId}", id);
                return StatusCode(500, ApiResponseHelper.Fail("An error occurred"));
            }
        }

        [HttpGet("departments")]
        public Task<IActionResult> GetDepartments() =>
            GetLookup(_service.GetDepartmentsAsync, "departments");

        [HttpGet("items")]
        public Task<IActionResult> GetItems() =>
            GetLookup(_service.GetItemsAsync, "items");

        private async Task<IActionResult> GetLookup(
            Func<string, DepartmentalConsumptionDto, Task<DataTable>> getData,
            string name)
        {
            try
            {
                var userId = GetUserId();
                if (string.IsNullOrWhiteSpace(userId))
                    return Unauthorized(ApiResponseHelper.Fail("userid not found"));

                var request = new DepartmentalConsumptionDto
                {
                    data = JsonDocument.Parse("{}").RootElement
                };
                var data = await getData(userId, request);
                return Ok(ApiResponseHelper.Sucess(
                    DataTableHelper.ToDynamicList(data), $"{name} retrieved successfully"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting Departmental Consumption {Lookup}", name);
                return StatusCode(500, ApiResponseHelper.Fail("An error occurred"));
            }
        }

        [HttpGet("item-batches/{itemId:int}")]
        public async Task<IActionResult> GetItemBatches(int itemId)
        {
            try
            {
                var userId = GetUserId();
                if (string.IsNullOrWhiteSpace(userId))
                    return Unauthorized(ApiResponseHelper.Fail("userid not found"));

                var request = new DepartmentalConsumptionDto
                {
                    data = JsonDocument.Parse(JsonSerializer.Serialize(new { itemId })).RootElement
                };
                var result = await _service.GetItemBatchesAsync(userId, request);
                if (result.Rows.Count == 0)
                    return NotFound(ApiResponseHelper.Fail("No batch information was returned."));

                var batchesJson = GetColumnValue(result.Rows[0], "availableBatchesJson")?.ToString();
                var availableQtyValue = GetColumnValue(result.Rows[0], "availableQty")
                    ?? throw new InvalidOperationException("Stored procedure response is missing availableQty.");

                var availableBatches = string.IsNullOrWhiteSpace(batchesJson)
                    ? new List<AvailableBatch>()
                    : JsonSerializer.Deserialize<List<AvailableBatch>>(
                          batchesJson,
                          new JsonSerializerOptions { PropertyNameCaseInsensitive = true })
                      ?? new List<AvailableBatch>();

                var availableQty = Convert.ToDecimal(availableQtyValue, CultureInfo.InvariantCulture);
                return Ok(ApiResponseHelper.Sucess(
                    new { availableBatches, availableQty },
                    "Item batches retrieved successfully"));
            }
            catch (JsonException ex)
            {
                _logger.LogError(ex, "Stored procedure returned invalid batch JSON for item {ItemId}", itemId);
                return StatusCode(500, ApiResponseHelper.Fail("Invalid batch data returned."));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting batches for item {ItemId}", itemId);
                return StatusCode(500, ApiResponseHelper.Fail("An error occurred"));
            }
        }

        [HttpGet("next-number")]
        public async Task<IActionResult> GetNextNumber()
        {
            try
            {
                var userId = GetUserId();
                if (string.IsNullOrWhiteSpace(userId))
                    return Unauthorized(ApiResponseHelper.Fail("userid not found"));

                var request = new DepartmentalConsumptionDto
                {
                    data = JsonDocument.Parse("{}").RootElement
                };
                var number = await _service.GetNextNumberAsync(userId, request);
                return Ok(ApiResponseHelper.Sucess(
                    new { consumptionNo = number }, "Next consumption number"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting next Departmental Consumption number");
                return StatusCode(500, ApiResponseHelper.Fail("An error occurred"));
            }
        }

        [HttpPost("insertRecord")]
        public async Task<IActionResult> InsertRecord([FromBody] DepartmentalConsumptionDto request)
        {
            try
            {
                var userId = GetUserId();
                if (string.IsNullOrWhiteSpace(userId))
                    return Unauthorized(ApiResponseHelper.Fail("userid not found"));
                if (!await HasPermission(userId, "ADD"))
                    return Forbid();

                return ResultResponse(await _service.InsertRecordAsync(GetUserName(), request));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inserting Departmental Consumption");
                return StatusCode(500, ApiResponseHelper.Fail("An error occurred"));
            }
        }

        [HttpPut("updateRecord")]
        public async Task<IActionResult> UpdateRecord([FromBody] DepartmentalConsumptionDto request)
        {
            try
            {
                var userId = GetUserId();
                if (string.IsNullOrWhiteSpace(userId))
                    return Unauthorized(ApiResponseHelper.Fail("userid not found"));
                if (!await HasPermission(userId, "EDIT"))
                    return Forbid();
                if (string.IsNullOrWhiteSpace(request.recordId))
                    return BadRequest(ApiResponseHelper.Fail("Empty Record ID"));

                return ResultResponse(await _service.UpdateRecordAsync(GetUserName(), request));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating Departmental Consumption");
                return StatusCode(500, ApiResponseHelper.Fail("An error occurred"));
            }
        }

        [HttpDelete("deleteRecord/{id:long}")]
        public async Task<IActionResult> DeleteRecord(long id)
        {
            try
            {
                var userId = GetUserId();
                if (string.IsNullOrWhiteSpace(userId))
                    return Unauthorized(ApiResponseHelper.Fail("userid not found"));
                if (!await HasPermission(userId, "DELETE"))
                    return Forbid();

                var request = new DepartmentalConsumptionDto
                {
                    recordId = id.ToString(),
                    data = JsonDocument.Parse("{}").RootElement
                };
                return ResultResponse(await _service.DeleteRecordAsync(GetUserName(), request));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting Departmental Consumption {RecordId}", id);
                return StatusCode(500, ApiResponseHelper.Fail("An error occurred"));
            }
        }

        [HttpPost("approve/{id:long}")]
        public Task<IActionResult> Approve(long id) =>
            RunApprovalAction(id, "APPROVE", _service.ApproveAsync);

        [HttpPost("reject/{id:long}")]
        public Task<IActionResult> Reject(long id) =>
            RunApprovalAction(id, "REJECT", _service.RejectAsync);

        private async Task<IActionResult> RunApprovalAction(
            long id,
            string permission,
            Func<string, DepartmentalConsumptionDto, Task<DataTable>> action)
        {
            try
            {
                var userId = GetUserId();
                if (string.IsNullOrWhiteSpace(userId))
                    return Unauthorized(ApiResponseHelper.Fail("userid not found"));
                if (!await HasPermission(userId, permission))
                    return Forbid();

                var request = new DepartmentalConsumptionDto
                {
                    recordId = id.ToString(),
                    data = JsonDocument.Parse("{}").RootElement
                };
                return ResultResponse(await action(GetUserName(), request));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error performing Departmental Consumption action {Action} on {RecordId}",
                    permission, id);
                return StatusCode(500, ApiResponseHelper.Fail("An error occurred"));
            }
        }

        private Task<bool> HasPermission(string userId, string permission) =>
            _permissionService.CheckPermissionAsync(userId, FormId, permission);

        private IActionResult ResultResponse(DataTable result)
        {
            if (result.Rows.Count == 0)
                return StatusCode(500, ApiResponseHelper.Fail("Stored procedure returned no result."));

            var message = result.Columns.Contains("Message")
                ? result.Rows[0]["Message"]?.ToString() ?? "Operation failed"
                : "Operation failed";
            var success = result.Columns.Contains("isSuccess") &&
                          result.Rows[0]["isSuccess"] != DBNull.Value &&
                          Convert.ToBoolean(result.Rows[0]["isSuccess"]);

            return success
                ? Ok(ApiResponseHelper.Sucess(null, message))
                : BadRequest(ApiResponseHelper.Fail(message));
        }

        private static object? GetColumnValue(DataRow row, string columnName)
        {
            var column = row.Table.Columns
                .Cast<DataColumn>()
                .FirstOrDefault(candidate => string.Equals(
                    candidate.ColumnName, columnName, StringComparison.OrdinalIgnoreCase));

            if (column is null)
                throw new InvalidOperationException(
                    $"Stored procedure response is missing {columnName}.");

            return row[column] == DBNull.Value ? null : row[column];
        }

        private sealed record AvailableBatch(string? BatchNo, decimal BatchQty);
    }
}
