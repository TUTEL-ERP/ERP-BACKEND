// Interfaces/Repository/IRefreshTokenRepository.cs
using server.Enity;

namespace server.Interfaces.Repository
{
    public interface IRefreshTokenRepository : IGenericRepository<RefreshToken>
    {
        Task<RefreshToken?> GetByTokenAsync(string token);
        Task RevokeTokenAsync(string token);
        Task RevokeAllUserTokensAsync(int userId);
        Task<bool> ValidateTokenAsync(string token);
        Task<int> DeleteExpiredTokensAsync();
    }
}