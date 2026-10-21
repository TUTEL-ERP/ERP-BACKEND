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
    public class GRNController : ControllerBase
    {
        private readonly IGRNService _service;
        private readonly ILogger<GRNController> _logger;
        private const string FORM_ID = "GoodsReceivedNote";

        public GRNController(IGRNService service, ILogger<GRNController> logger)
        {
            _service = service;
            _logger = logger;
        }

        private string GetUserId() => User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        private string GetUserName() => User.FindFirst(ClaimTypes.Name)?.Value ?? "unknown";

        [HttpGet("grid/{formName}")]
        public async Task<IActionResult> GetGridData(string formName)
        {
            var userid = GetUserId();
            var req = new GRNRequestDto { formId = formName, data = JsonDocument.Parse("{}").RootElement };
            var dt = await _service.GetGridDataAsync(userid, req);
            return Ok(ApiResponseHelper.Sucess(DataTableHelper.ToDynamicList(dt), "Success"));
        }

        [HttpGet("record/{id}")]
        public async Task<IActionResult> SelectRecord(int id)
        {
            var userid = GetUserId();
            var req = new GRNRequestDto { recordId = id.ToString(), data = JsonDocument.Parse("{}").RootElement };
            var ds = await _service.GetMasterDetailAsync(userid, req);
            if (ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0) return Ok(ApiResponseHelper.Fail("Not found"));
            return Ok(ApiResponseHelper.Sucess(new { master = DataTableHelper.ToDynamicList(ds.Tables[0]).FirstOrDefault(), lines = DataTableHelper.ToDynamicList(ds.Tables[1]) }, "Success"));
        }

    [HttpGet("approved-pos")] 
public async Task<IActionResult> GetApprovedPOs()
{
    var req = new GRNRequestDto { data = JsonDocument.Parse("{}").RootElement };
    var dt = await _service.GetAllApprovedPOsAsync(GetUserId(), req);
    return Ok(ApiResponseHelper.Sucess(DataTableHelper.ToDynamicList(dt), "Success"));
}

        [HttpGet("items")]
        public async Task<IActionResult> GetItems()
        {
            var req = new GRNRequestDto { data = JsonDocument.Parse("{}").RootElement };
            var dt = await _service.GetAllItemsAsync(GetUserId(), req);
            return Ok(ApiResponseHelper.Sucess(DataTableHelper.ToDynamicList(dt), "Success"));
        }
    [HttpGet("po-items/{poCode}")]
public async Task<IActionResult> GetPOItems(string poCode)
{
    var payload = JsonDocument.Parse(JsonSerializer.Serialize(new { poCode })).RootElement;
    var req = new GRNRequestDto { data = payload };
    var dt = await _service.GetPOItemsAsync(GetUserId(), req);
    return Ok(ApiResponseHelper.Sucess(DataTableHelper.ToDynamicList(dt), "Success"));
}

        [HttpGet("next-number")]
        public async Task<IActionResult> GetNextNumber()
        {
            var req = new GRNRequestDto { data = JsonDocument.Parse("{}").RootElement };
            var no = await _service.GetNextGRNNumberAsync(GetUserId(), req);
            return Ok(ApiResponseHelper.Sucess(new { grnNo = no }, "Success"));
        }

        [HttpPost("insertRecord")]
        public async Task<IActionResult> InsertRecord([FromBody] GRNRequestDto request)
        {
            var dt = await _service.InsertRecordAsync(GetUserName(), request);
            return Ok(ApiResponseHelper.Sucess(null, dt.Rows[0][0].ToString()));
        }

        [HttpPut("updateRecord")]
        public async Task<IActionResult> UpdateRecord([FromBody] GRNRequestDto request)
        {
            var dt = await _service.UpdateRecordAsync(GetUserName(), request);
            return Ok(ApiResponseHelper.Sucess(null, dt.Rows[0][0].ToString()));
        }

        [HttpPost("approve/{id}")]
        public async Task<IActionResult> Approve(int id)
        {
            var req = new GRNRequestDto { recordId = id.ToString(), data = JsonDocument.Parse("{}").RootElement };
            var dt = await _service.ApproveAsync(GetUserName(), req);
            return Ok(ApiResponseHelper.Sucess(null, dt.Rows[0][0].ToString()));
        }

        [HttpPost("reject/{id}")]
        public async Task<IActionResult> Reject(int id, [FromBody] JsonElement body)
        {
            var reason = body.ValueKind != JsonValueKind.Undefined && body.TryGetProperty("reason", out var r) ? r.GetString() : null;
            var req = new GRNRequestDto { recordId = id.ToString(), data = JsonDocument.Parse(JsonSerializer.Serialize(new { reason })).RootElement };
            var dt = await _service.RejectAsync(GetUserName(), req);
            return Ok(ApiResponseHelper.Sucess(null, dt.Rows[0][0].ToString()));
        }

        [HttpDelete("deleteRecord/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var req = new GRNRequestDto { recordId = id.ToString(), data = JsonDocument.Parse("{}").RootElement };
            var dt = await _service.DeleteRecordAsync(GetUserName(), req);
            return Ok(ApiResponseHelper.Sucess(null, dt.Rows[0][0].ToString()));
        }
    }
}