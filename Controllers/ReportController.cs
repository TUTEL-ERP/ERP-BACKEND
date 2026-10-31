using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using server.Helpers;
using server.Interfaces.Services;
using System.Security.Claims;

namespace server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ReportController : ControllerBase
    {
        private readonly IReportService _reportService;
        private readonly ILogger<ReportController> _logger;

        public ReportController(IReportService reportService, ILogger<ReportController> logger)
        {
            _reportService = reportService;
            _logger = logger;
        }

        private string? GetUserId()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return userId ?? User.FindFirst("userID")?.Value;
        }

        [HttpGet("trial-balance")]
        public async Task<IActionResult> GetTrialBalance(
            [FromQuery] DateOnly fromDate,
            [FromQuery] DateOnly toDate,
            CancellationToken cancellationToken)
        {
            if (fromDate > toDate)
                return BadRequest(ApiResponseHelper.Fail("fromDate must be on or before toDate"));

            var userId = GetUserId();
            if (string.IsNullOrEmpty(userId))
                return Ok(ApiResponseHelper.Fail("userid not found"));

            try
            {
                var data = await _reportService.GetTrialBalanceAsync(
                    userId,
                    fromDate,
                    toDate,
                    cancellationToken);

                return Ok(ApiResponseHelper.Sucess(
                    DataTableHelper.ToDynamicList(data),
                    "Trial balance retrieved successfully"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting trial balance");
                return StatusCode(500, ApiResponseHelper.Fail("An error occurred"));
            }
        }

        [HttpGet("balance-sheet")]
        public async Task<IActionResult> GetBalanceSheet(
            [FromQuery] DateOnly asOfDate,
            CancellationToken cancellationToken)
        {
            var userId = GetUserId();
            if (string.IsNullOrEmpty(userId))
                return Ok(ApiResponseHelper.Fail("userid not found"));

            try
            {
                var data = await _reportService.GetBalanceSheetAsync(
                    userId,
                    asOfDate,
                    cancellationToken);

                return Ok(ApiResponseHelper.Sucess(
                    DataTableHelper.ToDynamicList(data),
                    "Balance sheet retrieved successfully"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting balance sheet");
                return StatusCode(500, ApiResponseHelper.Fail("An error occurred"));
            }
        }
    }
}
