// Controllers/MenuController.cs
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using server.Enums;
using server.Interfaces.Repository;
using System.Security.Claims;

namespace server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class MenuController : ControllerBase
    {
        private readonly IMenuRepository _menuRepository;

        public MenuController(IMenuRepository menuRepository)
        {
            _menuRepository = menuRepository;
        }

        private string GetUserId()
        {
            var userid = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userid))
                userid = User.FindFirst("userID")?.Value;
            return userid;
        }

        // ✅ NOW: Returns only menus the current user has VIEW permission on
        [HttpGet("GetMenu")]
        public async Task<IActionResult> GetMenuHierarchy()
        {
            try
            {
                var userid = GetUserId();
                if (string.IsNullOrEmpty(userid))
                    return Ok(new { responseCode = 1, message = "userid not found" });

                var menus = await _menuRepository.GetMenuForUserAsync(userid, MenuAction.GET_MENU_FOR_USER);

                return Ok(new { responseCode = 0, data = menus });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { responseCode = 500, message = "An error occurred" });
            }
        }
    }
}