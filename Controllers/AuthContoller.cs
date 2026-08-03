// AuthController.cs
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using server.Dto;
using server.Interfaces.Services;
using System.Security.Claims;

namespace server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly ILogger<AuthController> _logger;

        public AuthController(IAuthService authService, ILogger<AuthController> logger)
        {
            _authService = authService;
            _logger = logger;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            try
            {
                var response = await _authService.LoginAsync(request);
                return Ok(new { responseCode = 0, message = "Login successful", data = response });
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { responseCode = 401, message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Login error");
                return StatusCode(500, new { responseCode = 500, message = "An error occurred" });
            }
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            try
            {
                var response = await _authService.RegisterAsync(request);
                return Ok(new { responseCode = 0, message = "Registration successful", data = response });
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { responseCode = 409, message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Registration error");
                return StatusCode(500, new { responseCode = 500, message = "An error occurred" });
            }
        }

        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequest request)
        {
            try
            {
                var response = await _authService.RefreshTokenAsync(request);
                return Ok(new { responseCode = 0, message = "Token refreshed successfully", data = response });
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { responseCode = 401, message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Refresh token error");
                return StatusCode(500, new { responseCode = 500, message = "An error occurred" });
            }
        }

        [Authorize]
        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            try
            {
                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
                await _authService.LogoutAsync(userId);
                return Ok(new { responseCode = 0, message = "Logged out successfully" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Logout error");
                return StatusCode(500, new { responseCode = 500, message = "An error occurred" });
            }
        }

        [Authorize]
        [HttpPost("revoke-all")]
        public async Task<IActionResult> RevokeAllTokens()
        {
            try
            {
                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
                await _authService.RevokeAllTokensAsync(userId);
                return Ok(new { responseCode = 0, message = "All tokens revoked successfully" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Revoke tokens error");
                return StatusCode(500, new { responseCode = 500, message = "An error occurred" });
            }
        }
    }
}