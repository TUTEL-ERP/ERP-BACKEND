// Repository/RefreshTokenRepository.cs
using ERP_API.Data;
using Microsoft.EntityFrameworkCore;
using server.Data;
using server.Enity;
using server.Interfaces.Repository;

namespace server.Repository
{
    public class RefreshTokenRepository : GenericRepository<RefreshToken>, IRefreshTokenRepository
    {
        private readonly ApplicationDbContext _context;

        public RefreshTokenRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<RefreshToken?> GetByTokenAsync(string token)
        {
            return await _dbSet
                .Include(rt => rt.User)
                .FirstOrDefaultAsync(rt => rt.Token == token && !rt.IsRevoked);
        }

        public async Task RevokeTokenAsync(string token)
        {
            var refreshToken = await _dbSet
                .FirstOrDefaultAsync(rt => rt.Token == token);

            if (refreshToken != null)
            {
                refreshToken.IsRevoked = true;
                await SaveChangesAsync();
            }
        }

        public async Task RevokeAllUserTokensAsync(int userId)
        {
            var tokens = await _dbSet
                .Where(rt => rt.UserId == userId && !rt.IsRevoked)
                .ToListAsync();

            foreach (var token in tokens)
            {
                token.IsRevoked = true;
            }

            await SaveChangesAsync();
        }

        public async Task<bool> ValidateTokenAsync(string token)
        {
            var refreshToken = await _dbSet
                .FirstOrDefaultAsync(rt => rt.Token == token &&
                                         !rt.IsRevoked &&
                                         rt.ExpiryDate > DateTime.UtcNow);

            return refreshToken != null;
        }

        public async Task<int> DeleteExpiredTokensAsync()
        {
            var expiredTokens = await _dbSet
                .Where(rt => rt.ExpiryDate <= DateTime.UtcNow || rt.IsRevoked)
                .ToListAsync();

            if (expiredTokens.Any())
            {
                _dbSet.RemoveRange(expiredTokens);
                await SaveChangesAsync();
            }

            return expiredTokens.Count;
        }

        public async Task<RefreshToken> CreateAsync(RefreshToken refreshToken)
        {
            await _dbSet.AddAsync(refreshToken);
            await SaveChangesAsync();
            return refreshToken;
        }
    }
}