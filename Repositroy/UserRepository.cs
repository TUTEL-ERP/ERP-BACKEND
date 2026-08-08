// Repository/UserRepository.cs
using ERP_API.Data;
using Microsoft.EntityFrameworkCore;
using server.Data;
using server.Enity;
using server.Interfaces.Repository;

namespace server.Repository
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        private readonly ApplicationDbContext _context;

        public UserRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<User?> GetByUsernameAsync(string username)
        {
            return await _dbSet
                .FirstOrDefaultAsync(u => u.Username == username);
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _dbSet
                .FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<User?> GetByRefreshTokenAsync(string refreshToken)
        {
            return await _dbSet
                .FirstOrDefaultAsync(u => u.RefreshToken == refreshToken);
        }

        public async Task<bool> UsernameExistsAsync(string username)
        {
            return await _dbSet
                .AnyAsync(u => u.Username == username);
        }

        public async Task<bool> EmailExistsAsync(string email)
        {
            return await _dbSet
                .AnyAsync(u => u.Email == email);
        }

        public async Task<User> CreateAsync(User user)
        {
            await _dbSet.AddAsync(user);
            await SaveChangesAsync();
            return user;
        }

        public async Task<User> UpdateAsync(User user)
        {
            _dbSet.Update(user);
            await SaveChangesAsync();
            return user;
        }
    }
}