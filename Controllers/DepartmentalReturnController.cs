using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using server.Dto;
using server.Enums;
using server.Helpers;
using server.Interfaces.Services;
using System.Security.Claims;
using System.Text.Json;

namespace server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class DepartmentalReturnController : ControllerBase
    {
        private const string FormId = "DepartmentalReturn";
        private readonly IDepartmentalReturnService _service;
        private readonly IPermissionService _permissionService;
        private readonly ILogger<DepartmentalReturnController> _logger;

        public DepartmentalReturnController(
            IDepartmentalReturnService service,
            IPermissionService permissionService,
            ILogger<DepartmentalReturnController> logger)
        {
            _service = service;
            _permissionService = permissionService;
            _logger = logger;
        }

        private string? GetUserId() =>
            User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? User.FindFirst("userID")?.Value;

        private string GetUserName() =>
            User.FindFirst(ClaimTypes.Name)?.Value ?? "unknown";

        [HttpGet("grid/{formName}")]
        public async Task<IActionResult> GetGridData(string formName)
        {
            try
            {
                var userId = GetUserId();
                if (string.IsNullOrEmpty(userId)) return Ok(ApiResponseHelper.Fail("userid not found"));

                var request = new DepartmentalReturnDto
                {
                    formId = formName,
                    data = JsonDocument.Parse("{}").RootElement
                };
                var data = await _service.GetGridDataAsync(userId, request);
                return Ok(ApiResponseHelper.Sucess(DataTableHelper.ToDynamicList(data), "Data retrieved successfully"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting departmental return grid");
                return StatusCode(500, ApiResponseHelper.Fail("An error occurred"));
            }
        }

        [HttpGet("record/{id:long}")]
        public async Task<IActionResult> GetRecord(long id)
        {
            try
            {
                var userId = GetUserId();
                if (string.IsNullOrEmpty(userId)) return Ok(ApiResponseHelper.Fail("userid not found"));

                var request = new DepartmentalReturnDto
                {
                    recordId = id.ToString(),
                    data = JsonDocument.Parse("{}").RootElement
                };
                var dataSet = await _service.GetMasterDetailAsync(userId, request);
                if (dataSet.Tables.Count == 0 || dataSet.Tables[0].Rows.Count == 0)
                    return Ok(ApiResponseHelper.Fail("Record not found"));

                var master = DataTableHelper.ToDynamicList(dataSet.Tables[0]).FirstOrDefault();
                var lines = dataSet.Tables.Count > 1
                    ? DataTableHelper.ToDynamicList(dataSet.Tables[1])
                    : new List<dynamic>();
                return Ok(ApiResponseHelper.Sucess(new { master, lines }, "Data retrieved successfully"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting departmental return record");
                return StatusCode(500, ApiResponseHelper.Fail("An error occurred"));
            }
        }

        [HttpGet("departments")]
        public Task<IActionResult> GetDepartments() => GetLov(DepartmentalReturnAction.GET_DEPARTMENTS);

        [HttpGet("items")]
        public Task<IActionResult> GetItems() => GetLov(DepartmentalReturnAction.GET_ITEMS);

        private async Task<IActionResult> GetLov(DepartmentalReturnAction action)
        {
            try
            {
                var userId = GetUserId();
                if (string.IsNullOrEmpty(userId)) return Ok(ApiResponseHelper.Fail("userid not found"));

                var request = new DepartmentalReturnDto { data = JsonDocument.Parse("{}").RootElement };
                var data = await _service.GetLovAsync(userId, action, request);
                return Ok(ApiResponseHelper.Sucess(DataTableHelper.ToDynamicList(data), "Data retrieved successfully"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting departmental return lookup data for {Action}", action);
                return StatusCode(500, ApiResponseHelper.Fail("An error occurred"));
            }
        }

        [HttpGet("next-number")]
        public async Task<IActionResult> GetNextNumber()
        {
            try
            {
                var userId = GetUserId();
                if (string.IsNullOrEmpty(userId)) return Ok(ApiResponseHelper.Fail("userid not found"));

                var request = new DepartmentalReturnDto { data = JsonDocument.Parse("{}").RootElement };
                var number = await _service.GetNextNumberAsync(userId, request);
                return Ok(ApiResponseHelper.Sucess(new { returnNo = number }, "Next return number"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting departmental return number");
                return StatusCode(500, ApiResponseHelper.Fail("An error occurred"));
            }
        }

        [HttpPost("insertRecord")]
        public async Task<IActionResult> InsertRecord([FromBody] DepartmentalReturnDto request)
        {
            try
            {
                var result = await _service.InsertRecordAsync(GetUserName(), request);
                return ResultResponse(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inserting departmental return");
                return StatusCode(500, ApiResponseHelper.Fail("An error occurred"));
            }
        }

        [HttpPut("updateRecord")]
        public async Task<IActionResult> UpdateRecord([FromBody] DepartmentalReturnDto request)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(request.recordId))
                    return Ok(ApiResponseHelper.Fail("Empty Record ID"));

                var result = await _service.UpdateRecordAsync(GetUserName(), request);
                return ResultResponse(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating departmental return");
                return StatusCode(500, ApiResponseHelper.Fail("An error occurred"));
            }
        }

        [HttpDelete("deleteRecord/{id:long}")]
        public async Task<IActionResult> DeleteRecord(long id)
        {
            try
            {
                var request = new DepartmentalReturnDto
                {
                    recordId = id.ToString(),
                    data = JsonDocument.Parse("{}").RootElement
                };
                return ResultResponse(await _service.DeleteRecordAsync(GetUserName(), request));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting departmental return");
                return StatusCode(500, ApiResponseHelper.Fail("An error occurred"));
            }
        }

        [HttpPost("approve/{id:long}")]
        public Task<IActionResult> Approve(long id) =>
            RunApprovalAction(id, DepartmentalReturnAction.APPROVE);

        [HttpPost("reject/{id:long}")]
        public async Task<IActionResult> Reject(long id, [FromBody] JsonElement body)
        {
            try
            {
                var reason = body.ValueKind == JsonValueKind.Object &&
                             body.TryGetProperty("reason", out var reasonValue)
                    ? reasonValue.GetString()
                    : null;
                var request = new DepartmentalReturnDto
                {
                    recordId = id.ToString(),
                    data = JsonDocument.Parse(JsonSerializer.Serialize(new { reason })).RootElement
                };
                return await RunApprovalAction(request, DepartmentalReturnAction.REJECT);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error rejecting departmental return");
                return StatusCode(500, ApiResponseHelper.Fail("An error occurred"));
            }
        }

        private async Task<IActionResult> RunApprovalAction(long id, DepartmentalReturnAction action)
        {
            var request = new DepartmentalReturnDto
            {
                recordId = id.ToString(),
                data = JsonDocument.Parse("{}").RootElement
            };
            return await RunApprovalAction(request, action);
        }

        private async Task<IActionResult> RunApprovalAction(DepartmentalReturnDto request, DepartmentalReturnAction action)
        {
            try
            {
                var userId = GetUserId();
                if (string.IsNullOrEmpty(userId)) return Ok(ApiResponseHelper.Fail("userid not found"));

                var permission = action == DepartmentalReturnAction.APPROVE ? "APPROVE" : "REJECT";
                if (!await _permissionService.CheckPermissionAsync(userId, FormId, permission))
                    return Ok(ApiResponseHelper.Fail($"No permission to {permission.ToLower()} departmental returns."));

                var result = action == DepartmentalReturnAction.APPROVE
                    ? await _service.ApproveAsync(GetUserName(), request)
                    : await _service.RejectAsync(GetUserName(), request);
                return ResultResponse(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing departmental return action {Action}", action);
                return StatusCode(500, ApiResponseHelper.Fail("An error occurred"));
            }
        }

        private IActionResult ResultResponse(System.Data.DataTable result)
        {
            if (result.Rows.Count == 0)
                return Ok(ApiResponseHelper.Fail("No response"));

            var message = result.Rows[0][0]?.ToString() ?? "Operation failed";
            var success = result.Columns.Count > 1 &&
                          result.Rows[0][1] != DBNull.Value &&
                          Convert.ToBoolean(result.Rows[0][1]);
            return Ok(success
                ? ApiResponseHelper.Sucess(null, message)
                : ApiResponseHelper.Fail(message));
        }
    }
}
