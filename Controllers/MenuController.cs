// Controllers/MenuController.cs
using Microsoft.AspNetCore.Mvc;
using server.Dto;
using server.Interfaces.Repository;

namespace server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MenuController : ControllerBase
    {
        private readonly IMenuRepository _menuRepository;

        public MenuController(IMenuRepository menuRepository)
        {
            _menuRepository = menuRepository;
        }

        [HttpGet("GetMenu")]
        public async Task<IActionResult> GetMenuHierarchy()
        {
            try
            {
                var menus = await _menuRepository.GetMenuHierarchyAsync();
                return Ok(new { responseCode = 0, data = menus });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { responseCode = 500, message = "An error occurred" });
            }
        }
    }
}