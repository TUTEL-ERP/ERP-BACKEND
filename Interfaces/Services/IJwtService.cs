using server.Enity;
using System.Security.Claims;

namespace server.Interfaces.Services
{
    public interface IJwtService
    {
        string GenerateAccessToken(User user);
        string GenerateRefreshToken();
        ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
        bool ValidateRefreshToken(string refreshToken);
    }
}
