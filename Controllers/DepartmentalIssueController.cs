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
    public class DepartmentalIssueController : ControllerBase
    {
        private const string FormId = "DepartmentalIssue";
        private readonly IDepartmentalIssueService _service;
        private readonly IPermissionService _permissionService;
        private readonly ILogger<DepartmentalIssueController> _logger;

        public DepartmentalIssueController(
            IDepartmentalIssueService service,
            IPermissionService permissionService,
            ILogger<DepartmentalIssueController> logger)
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

                var request = new DepartmentalIssueDto
                {
                    formId = formName,
                    data = JsonDocument.Parse("{}").RootElement
                };
                var data = await _service.GetGridDataAsync(userId, request);
                return Ok(ApiResponseHelper.Sucess(DataTableHelper.ToDynamicList(data), "Data retrieved successfully"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting Departmental Issue grid");
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

                var request = new DepartmentalIssueDto
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
                _logger.LogError(ex, "Error getting Departmental Issue record {RecordId}", id);
                return StatusCode(500, ApiResponseHelper.Fail("An error occurred"));
            }
        }

        [HttpGet("departments")]
        public Task<IActionResult> GetDepartments() => GetLookup(_service.GetDepartmentsAsync, "departments");

        [HttpGet("items")]
        public Task<IActionResult> GetItems() => GetLookup(_service.GetItemsAsync, "items");

        private async Task<IActionResult> GetLookup(
            Func<string, DepartmentalIssueDto, Task<DataTable>> getData,
            string name)
        {
            try
            {
                var userId = GetUserId();
                if (string.IsNullOrWhiteSpace(userId))
                    return Unauthorized(ApiResponseHelper.Fail("userid not found"));

                var request = new DepartmentalIssueDto { data = JsonDocument.Parse("{}").RootElement };
                var data = await getData(userId, request);
                return Ok(ApiResponseHelper.Sucess(DataTableHelper.ToDynamicList(data), $"{name} retrieved successfully"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting Departmental Issue {Lookup}", name);
                return StatusCode(500, ApiResponseHelper.Fail("An error occurred"));
            }
        }

        [HttpGet("next-number")]
        public async Task<IActionResult> GetNextNo()
        {
            try
            {
                var userId = GetUserId();
                if (string.IsNullOrWhiteSpace(userId))
                    return Unauthorized(ApiResponseHelper.Fail("userid not found"));

                var request = new DepartmentalIssueDto { data = JsonDocument.Parse("{}").RootElement };
                var number = await _service.GetNextNoAsync(userId, request);
                return Ok(ApiResponseHelper.Sucess(new { issueNo = number }, "Next issue number"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting next Departmental Issue number");
                return StatusCode(500, ApiResponseHelper.Fail("An error occurred"));
            }
        }

        [HttpGet("approved-drs")]
        public async Task<IActionResult> GetApprovedDRs()
        {
            try
            {
                var userId = GetUserId();
                if (string.IsNullOrWhiteSpace(userId))
                    return Unauthorized(ApiResponseHelper.Fail("userid not found"));

                var request = new DepartmentalIssueDto { data = JsonDocument.Parse("{}").RootElement };
                var data = await _service.GetApprovedDRsAsync(userId, request);
                return Ok(ApiResponseHelper.Sucess(
                    DataTableHelper.ToDynamicList(data), "Approved delivery returns retrieved successfully"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting approved delivery returns for Departmental Issue");
                return StatusCode(500, ApiResponseHelper.Fail("An error occurred"));
            }
        }

        [HttpGet("dr-items/{drId}")]
        public async Task<IActionResult> GetDRItems(long drId)
        {
            try
            {
                var userId = GetUserId();
                if (string.IsNullOrWhiteSpace(userId))
                    return Unauthorized(ApiResponseHelper.Fail("userid not found"));

                var request = new DepartmentalIssueDto
                {
                    data = JsonDocument.Parse(JsonSerializer.Serialize(new { drId })).RootElement
                };
                var data = await _service.GetDRItemsAsync(userId, request);
                return Ok(ApiResponseHelper.Sucess(
                    DataTableHelper.ToDynamicList(data), "Delivery return items retrieved successfully"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting items for delivery return {DeliveryReturnId}", drId);
                return StatusCode(500, ApiResponseHelper.Fail("An error occurred"));
            }
        }

        [HttpPost("insertRecord")]
        public async Task<IActionResult> InsertRecord([FromBody] DepartmentalIssueDto request)
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
                _logger.LogError(ex, "Error inserting Departmental Issue");
                return StatusCode(500, ApiResponseHelper.Fail("An error occurred"));
            }
        }

        [HttpPut("updateRecord")]
        public async Task<IActionResult> UpdateRecord([FromBody] DepartmentalIssueDto request)
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
                _logger.LogError(ex, "Error updating Departmental Issue");
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

                var request = new DepartmentalIssueDto
                {
                    recordId = id.ToString(),
                    data = JsonDocument.Parse("{}").RootElement
                };
                return ResultResponse(await _service.DeleteRecordAsync(GetUserName(), request));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting Departmental Issue {RecordId}", id);
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
            Func<string, DepartmentalIssueDto, Task<DataTable>> action)
        {
            try
            {
                var userId = GetUserId();
                if (string.IsNullOrWhiteSpace(userId))
                    return Unauthorized(ApiResponseHelper.Fail("userid not found"));
                if (!await HasPermission(userId, permission))
                    return Forbid();

                var request = new DepartmentalIssueDto
                {
                    recordId = id.ToString(),
                    data = JsonDocument.Parse("{}").RootElement
                };
                return ResultResponse(await action(GetUserName(), request));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error performing Departmental Issue action {Action} on {RecordId}", permission, id);
                return StatusCode(500, ApiResponseHelper.Fail("An error occurred"));
            }
        }

        private Task<bool> HasPermission(string userId, string permission) =>
            _permissionService.CheckPermissionAsync(userId, FormId, permission);

        private IActionResult ResultResponse(DataTable result)
        {
            if (result.Rows.Count == 0)
                return StatusCode(500, ApiResponseHelper.Fail("Stored procedure returned no result."));

            var message = result.Rows[0]["Message"]?.ToString() ?? "Operation failed";
            var success = result.Columns.Contains("isSuccess") &&
                          result.Rows[0]["isSuccess"] != DBNull.Value &&
                          Convert.ToBoolean(result.Rows[0]["isSuccess"]);

            return success
                ? Ok(ApiResponseHelper.Sucess(null, message))
                : BadRequest(ApiResponseHelper.Fail(message));
        }
    }
}
