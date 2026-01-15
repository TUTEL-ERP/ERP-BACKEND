using Microsoft.AspNetCore.Authentication.OAuth.Claims;
using server.Entities;
using System.Security.Claims;

namespace server.Helpers
{
    public interface IJwtHelper
    {
        string GenerateJwtToken(User user);
        string GenerateRefreshToken();


        ClaimsPrincipal GetPrincipalFromExpiredToken(string token);

    }
}
