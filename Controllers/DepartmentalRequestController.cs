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
    public class DepartmentalRequestController : ControllerBase
    {
        private readonly IDepartmentalRequestService _service;
        public DepartmentalRequestController(IDepartmentalRequestService service) { _service = service; }

        private string GetUserId() => User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        private string GetUserName() => User.FindFirst(ClaimTypes.Name)?.Value ?? "unknown";

        [HttpGet("grid/{formName}")]
        public async Task<IActionResult> GetGridData(string formName)
        {
            var req = new DepartmentalRequestDto { formId = formName, data = JsonDocument.Parse("{}").RootElement };
            var dt = await _service.GetGridDataAsync(GetUserId(), req);
            return Ok(ApiResponseHelper.Sucess(DataTableHelper.ToDynamicList(dt), "Success"));
        }

        [HttpGet("record/{id}")]
        public async Task<IActionResult> SelectRecord(int id)
        {
            var req = new DepartmentalRequestDto { recordId = id.ToString(), data = JsonDocument.Parse("{}").RootElement };
            var ds = await _service.GetMasterDetailAsync(GetUserId(), req);
            if (ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0) return Ok(ApiResponseHelper.Fail("Not found"));
            return Ok(ApiResponseHelper.Sucess(new { master = DataTableHelper.ToDynamicList(ds.Tables[0]).FirstOrDefault(), lines = DataTableHelper.ToDynamicList(ds.Tables[1]) }, "Success"));
        }

        [HttpGet("departments")]
        public async Task<IActionResult> GetDepartments()
        {
            var req = new DepartmentalRequestDto { data = JsonDocument.Parse("{}").RootElement };
            var dt = await _service.GetDepartmentsAsync(GetUserId(), req);
            return Ok(ApiResponseHelper.Sucess(DataTableHelper.ToDynamicList(dt), "Success"));
        }

        [HttpGet("items")]
        public async Task<IActionResult> GetItems()
        {
            var req = new DepartmentalRequestDto { data = JsonDocument.Parse("{}").RootElement };
            var dt = await _service.GetItemsAsync(GetUserId(), req);
            return Ok(ApiResponseHelper.Sucess(DataTableHelper.ToDynamicList(dt), "Success"));
        }

        [HttpGet("next-number")]
        public async Task<IActionResult> GetNextNumber()
        {
            var req = new DepartmentalRequestDto { data = JsonDocument.Parse("{}").RootElement };
            var no = await _service.GetNextRequestNoAsync(GetUserId(), req);
            return Ok(ApiResponseHelper.Sucess(new { requestNo = no }, "Success"));
        }

        [HttpPost("insertRecord")]
        public async Task<IActionResult> InsertRecord([FromBody] DepartmentalRequestDto request)
        {
            var dt = await _service.InsertRecordAsync(GetUserName(), request);
            return Ok(ApiResponseHelper.Sucess(null, dt.Rows[0][0].ToString()));
        }

        [HttpPut("updateRecord")]
        public async Task<IActionResult> UpdateRecord([FromBody] DepartmentalRequestDto request)
        {
            var dt = await _service.UpdateRecordAsync(GetUserName(), request);
            return Ok(ApiResponseHelper.Sucess(null, dt.Rows[0][0].ToString()));
        }

        [HttpPost("approve/{id}")]
        public async Task<IActionResult> Approve(int id)
        {
            var req = new DepartmentalRequestDto { recordId = id.ToString(), data = JsonDocument.Parse("{}").RootElement };
            var dt = await _service.ApproveAsync(GetUserName(), req);
            return Ok(ApiResponseHelper.Sucess(null, dt.Rows[0][0].ToString()));
        }

        [HttpPost("reject/{id}")]
        public async Task<IActionResult> Reject(int id, [FromBody] JsonElement body)
        {
            var reason = body.TryGetProperty("reason", out var r) ? r.GetString() : "Rejected by user";
            var req = new DepartmentalRequestDto { recordId = id.ToString(), data = JsonDocument.Parse(JsonSerializer.Serialize(new { reason })).RootElement };
            var dt = await _service.RejectAsync(GetUserName(), req);
            return Ok(ApiResponseHelper.Sucess(null, dt.Rows[0][0].ToString()));
        }

        [HttpDelete("deleteRecord/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var req = new DepartmentalRequestDto { recordId = id.ToString(), data = JsonDocument.Parse("{}").RootElement };
            var dt = await _service.DeleteRecordAsync(GetUserName(), req);
            return Ok(ApiResponseHelper.Sucess(null, dt.Rows[0][0].ToString()));
        }
    }
}