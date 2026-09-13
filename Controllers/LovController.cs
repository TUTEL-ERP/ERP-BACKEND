using Lov.Application.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using server.Helpers;
using System.Data;
using System.Security.Claims;

namespace Lov.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class LovController : ControllerBase
    {
        private readonly ILovDataService _service;

        public LovController(ILovDataService lovDataService)
        {
            _service = lovDataService;
        }

        [HttpGet("GetLovData")]
        public async Task<IActionResult> GetLovData(string LovName, string? Action)
            {
            try
            {
                var userid = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

                // Fallback: try old key
                if (string.IsNullOrEmpty(userid))
                    userid = User.FindFirst("userID")?.Value;

                if (string.IsNullOrEmpty(userid))
                    return Ok(ApiResponseHelper.Fail("userid not found"));

                if (string.IsNullOrWhiteSpace(LovName))
                    return Ok(ApiResponseHelper.Fail("Empty LovName."));

                DataTable dataTable = await _service.GetLovData(userid, LovName, Action);

                if (dataTable.Rows.Count > 0)
                {
                    if (dataTable.Columns.Contains("message"))
                        return Ok(ApiResponseHelper.Fail(dataTable.Rows[0]["message"].ToString()));

                    var cleanData = DataTableHelper.ToDynamicList(dataTable);
                    return Ok(ApiResponseHelper.Sucess(cleanData, "Successful"));
                }
                else
                {
                    return Ok(ApiResponseHelper.Fail("No Data Found"));
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponseHelper.Fail(ex.Message));
            }
        }
    }
}