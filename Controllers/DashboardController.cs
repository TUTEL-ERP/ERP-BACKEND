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
    public class DashboardController : ControllerBase
    {
        private readonly IDashboardService _service;
        private readonly ILogger<DashboardController> _logger;

        public DashboardController(IDashboardService service, ILogger<DashboardController> logger)
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

        // ✅ MAIN ENDPOINT — returns everything in one call
        [HttpGet("all")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var userid = GetUserId();
                if (string.IsNullOrEmpty(userid)) return Ok(ApiResponseHelper.Fail("userid not found"));

                var request = new DashboardRequestDto { data = JsonDocument.Parse("{}").RootElement };
                var ds = await _service.GetAllDashboardDataAsync(userid, request);

                var result = new
                {
                    kpis = ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0
                        ? DataTableHelper.ToDynamicList(ds.Tables[0]).FirstOrDefault()
                        : null,

                    salesTrend = ds.Tables.Count > 1
                        ? DataTableHelper.ToDynamicList(ds.Tables[1])
                        : new List<dynamic>(),

                    topCustomers = ds.Tables.Count > 2
                        ? DataTableHelper.ToDynamicList(ds.Tables[2])
                        : new List<dynamic>(),

                    topItems = ds.Tables.Count > 3
                        ? DataTableHelper.ToDynamicList(ds.Tables[3])
                        : new List<dynamic>(),

                    recentSales = ds.Tables.Count > 4
                        ? DataTableHelper.ToDynamicList(ds.Tables[4])
                        : new List<dynamic>(),

                    recentPurchases = ds.Tables.Count > 5
                        ? DataTableHelper.ToDynamicList(ds.Tables[5])
                        : new List<dynamic>()
                };

                return Ok(ApiResponseHelper.Sucess(result, "Dashboard data retrieved"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting dashboard data");
                return StatusCode(500, ApiResponseHelper.Fail("An error occurred"));
            }
        }

        // Optional — just KPIs
        [HttpGet("kpis")]
        public async Task<IActionResult> GetKpis()
        {
            try
            {
                var userid = GetUserId();
                if (string.IsNullOrEmpty(userid)) return Ok(ApiResponseHelper.Fail("userid not found"));

                var request = new DashboardRequestDto { data = JsonDocument.Parse("{}").RootElement };
                var dt = await _service.GetKpisAsync(userid, request);
                var list = DataTableHelper.ToDynamicList(dt);
                return Ok(ApiResponseHelper.Sucess(list.FirstOrDefault(), "KPIs retrieved"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting KPIs");
                return StatusCode(500, ApiResponseHelper.Fail("An error occurred"));
            }
        }
    }
}