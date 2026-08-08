using ERP_API.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using server.Data; // Assuming your DbContext is here
using server.Dto;
using System.Reflection;

namespace server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class DynamicGridController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<DynamicGridController> _logger;

        public DynamicGridController(ApplicationDbContext context, ILogger<DynamicGridController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet("{formName}")]
        public async Task<IActionResult> GetGridData(string formName)
        {
            try
            {
                object data = null;

                switch (formName.ToLower())
                {
                    case "country":
                        data = await _context.Countries.ToListAsync();
                        break;
                    case "company":
                        //data = await _context.Companies.ToListAsync(); 
                        break;
                    case "users":
                        data = await _context.Users.ToListAsync();
                        break;
                    case "customer":
                        //data = await _context.Customers.ToListAsync();
                        break;
                    default:
                        return BadRequest(new { responseCode = 400, message = $"Form '{formName}' not found." });
                }

                return Ok(new { responseCode = 0, data = data });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting grid data for {FormName}", formName);
                return StatusCode(500, new { responseCode = 500, message = "An error occurred while fetching grid data." });
            }
        }

        // GET: api/DynamicGrid/{formName}/{id}
        [HttpGet("{formName}/{id}")]
        public async Task<IActionResult> GetRecordById(string formName, int id)
        {
            try
            {
                object data = null;

                switch (formName.ToLower())
                {
                    case "country":
                        data = await _context.Countries.FindAsync(id);
                        break;
                    case "company":
                        //data = await _context.Companies.FindAsync(id);
                        break;
                    case "user":
                        //data = await _context.Users.FindAsync(id);
                        break;
                    case "customer":
                        //data = await _context.Customers.FindAsync(id);
                        break;
                    default:
                        return BadRequest(new { responseCode = 400, message = $"Form '{formName}' not found." });
                }

                if (data == null)
                {
                    return NotFound(new { responseCode = 404, message = "Record not found" });
                }

                return Ok(new { responseCode = 0, data = data });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting record for {FormName} with ID {Id}", formName, id);
                return StatusCode(500, new { responseCode = 500, message = "An error occurred while fetching the record." });
            }
        }
    }
}