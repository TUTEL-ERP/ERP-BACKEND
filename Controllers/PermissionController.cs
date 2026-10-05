using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using server.Dto;
using server.Helpers;
using server.Interfaces.Services;
using System.Security.Claims;

namespace server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class PermissionController : ControllerBase
    {
        private readonly IPermissionService _service;
        private readonly ILogger<PermissionController> _logger;

        public PermissionController(IPermissionService service, ILogger<PermissionController> logger)
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

        // ═══════════════════════════════════════════════════════════
        // GET /api/Permission/load?formId=receipt
        // ═══════════════════════════════════════════════════════════
        [HttpGet("load")]
        public async Task<IActionResult> LoadPermissions([FromQuery] string formId)
        {
            try
            {
                var userid = GetUserId();
                if (string.IsNullOrEmpty(userid)) return Ok(ApiResponseHelper.Fail("userid not found"));
                if (string.IsNullOrEmpty(formId)) return Ok(ApiResponseHelper.Fail("formId is required"));

                var result = await _service.LoadForUserAsync(userid, formId);

                if (result == null)
                    return Ok(ApiResponseHelper.Sucess(new PermissionResponseDto { FormId = formId }, "No permissions"));

                return Ok(ApiResponseHelper.Sucess(result, "Permissions loaded"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading permissions");
                return StatusCode(500, ApiResponseHelper.Fail("An error occurred"));
            }
        }

        // ═══════════════════════════════════════════════════════════
        // GET /api/Permission/all
        // ═══════════════════════════════════════════════════════════
        [HttpGet("all")]
        public async Task<IActionResult> GetAllPermissions()
        {
            try
            {
                var userid = GetUserId();
                if (string.IsNullOrEmpty(userid)) return Ok(ApiResponseHelper.Fail("userid not found"));

                var result = await _service.GetAllForUserAsync(userid);
                return Ok(ApiResponseHelper.Sucess(result, "All permissions retrieved"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting all permissions");
                return StatusCode(500, ApiResponseHelper.Fail("An error occurred"));
            }
        }

        // ═══════════════════════════════════════════════════════════
        // GET /api/Permission/check?formId=receipt&permission=ADD
        // ═══════════════════════════════════════════════════════════
        [HttpGet("check")]
        public async Task<IActionResult> CheckPermission([FromQuery] string formId, [FromQuery] string permission)
        {
            try
            {
                var userid = GetUserId();
                if (string.IsNullOrEmpty(userid)) return Ok(ApiResponseHelper.Fail("userid not found"));

                var allowed = await _service.CheckPermissionAsync(userid, formId, permission);
                return Ok(ApiResponseHelper.Sucess(new { isAllowed = allowed }, "Permission checked"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error checking permission");
                return StatusCode(500, ApiResponseHelper.Fail("An error occurred"));
            }
        }
    }
}