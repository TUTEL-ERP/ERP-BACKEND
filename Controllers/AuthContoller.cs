using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using server.Dto;
using server.Interfaces.Services;
using System.Security.Claims;
using System.Text.Json;

namespace server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly ILogger<AuthController> _logger;
        private readonly IWebHostEnvironment _env;

        public AuthController(
            IAuthService authService,
            ILogger<AuthController> logger,
            IWebHostEnvironment env)
        {
            _authService = authService;
            _logger = logger;
            _env = env;
        }

        // ═══════════════════════════════════════════════════════════
        // LOGIN — sets HttpOnly cookies + plain "user" cookie
        // ═══════════════════════════════════════════════════════════
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            try
            {
                var response = await _authService.LoginAsync(request);

                WriteAuthCookies(response);
                var u = response.User;
                return Ok(new
                {
                    responseCode = 0,
                    message = "Login successful",
                    data = new
                    {
                        userId = u.Id,
                        username = u.Username,
                        email = u.Email,
                        role = u.Role
                    }
                });
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

        // ═══════════════════════════════════════════════════════════
        // REFRESH TOKEN — reads cookie, sets new cookies
        // ═══════════════════════════════════════════════════════════
        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken()
        {
            try
            {
                if (!Request.Cookies.TryGetValue("refresh_token", out var refreshToken)
                    || string.IsNullOrEmpty(refreshToken))
                {
                    return Unauthorized(new { responseCode = 401, message = "No refresh token" });
                }

                var response = await _authService.RefreshTokenAsync(new RefreshTokenRequest
                {
                    RefreshToken = refreshToken
                });

                WriteAuthCookies(response);

                return Ok(new { responseCode = 0, message = "Token refreshed" });
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

        // ═══════════════════════════════════════════════════════════
        // LOGOUT — revoke + clear cookies
        // ═══════════════════════════════════════════════════════════
        [Authorize]
        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            try
            {
                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
                await _authService.LogoutAsync(userId);

                ClearAuthCookies();

                return Ok(new { responseCode = 0, message = "Logged out successfully" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Logout error");
                ClearAuthCookies();   // still clear even if server fails
                return Ok(new { responseCode = 0, message = "Logged out" });
            }
        }


        [Authorize]
        [HttpGet("me")]
        public IActionResult Me()
        {
            var user = new
            {
                id = User.FindFirst(ClaimTypes.NameIdentifier)?.Value,
                username = User.FindFirst(ClaimTypes.Name)?.Value,
                email = User.FindFirst(ClaimTypes.Email)?.Value,
                role = User.FindFirst(ClaimTypes.Role)?.Value
            };
            return Ok(new { responseCode = 0, data = user });
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
        private void WriteAuthCookies(AuthResponse response)
        {
            // Cookies MUST be:
            //   - SameSite=None  (cross-domain)
            //   - Secure=true    (required when SameSite=None)
            //   - Path=/         (for user + access_token; refresh is scoped)

            Response.Cookies.Append("access_token", response.AccessToken, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,                        
                SameSite = SameSiteMode.None,         
                Path = "/",
                Expires = DateTimeOffset.UtcNow.AddHours(1)
            });

            Response.Cookies.Append("refresh_token", response.RefreshToken, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,                       
                SameSite = SameSiteMode.None,         
                Path = "/api/auth",                   
                Expires = DateTimeOffset.UtcNow.AddDays(7)
            });

            var u = response.User ?? new UserDto
            {
                Id = response.UserId,
                Username = response.Username,
                Email = response.Email,
                Role = response.Role
            };

            Response.Cookies.Append("user", JsonSerializer.Serialize(new
            {
                id = u.Id,
                username = u.Username,
                email = u.Email,
                role = u.Role
            }), new CookieOptions
            {
                HttpOnly = false,                    
                Secure = true,                        
                SameSite = SameSiteMode.None,         
                Path = "/",
                Expires = DateTimeOffset.UtcNow.AddDays(7)
            });
        }

        private void ClearAuthCookies()
        {
            var crossSiteOpts = new CookieOptions
            {
                Path = "/",
                Secure = true,
                SameSite = SameSiteMode.None
            };

            Response.Cookies.Delete("access_token", crossSiteOpts);
            Response.Cookies.Delete("user", crossSiteOpts);

            Response.Cookies.Delete("refresh_token", new CookieOptions
            {
                Path = "/api/auth",
                Secure = true,
                SameSite = SameSiteMode.None
            });
        }
    }
}