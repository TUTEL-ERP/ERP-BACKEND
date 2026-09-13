using System.Security.Claims;
using server.Enity;

namespace server.Helpers
{
    public interface IJwtHelper
    {
        string GenerateJwtToken(User user);
        string GenerateRefreshToken();
        ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
    }
}