    using server.Dto;

namespace server.Interfaces.Services
{
    public interface IAuthService
    {
        Task<AuthResponse> LoginAsync(LoginRequest request);
        Task<AuthResponse> RegisterAsync(RegisterRequest request);
        Task<AuthResponse> RefreshTokenAsync(RefreshTokenRequest request);
        Task<bool> LogoutAsync(int userId);
        Task<bool> RevokeAllTokensAsync(int userId);
        Task<bool> ValidateRefreshTokenAsync(string refreshToken);
        string HashPassword(string password);
        bool VerifyPassword(string password, string hash);
    }
}
