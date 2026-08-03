using server.Enity;

namespace server.Interfaces.Repository
{
    public interface IRefreshTokenRepository
    {
        Task<RefreshToken> GetByTokenAsync(string token);
        Task<RefreshToken> CreateAsync(RefreshToken refreshToken);
        Task RevokeTokenAsync(string token);
        Task RevokeAllUserTokensAsync(int userId);
        Task<bool> ValidateTokenAsync(string token);
        Task<int> DeleteExpiredTokensAsync();
    }
}
