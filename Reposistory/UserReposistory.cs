using Microsoft.EntityFrameworkCore;
using server.Data;
using server.Entities;
using server.Interfaces.Repository;

namespace server.Reposistory
{
    public class UserReposistory : IUserReposistory
    {

        private readonly DataContext context1;

        public UserReposistory (DataContext context) {  
        
         this.context1 = context;
        }


        public async Task<bool> AddUser(User user)
        {
            await this.context1.User.AddAsync(user);
                return await this.context1.SaveChangesAsync() > 0 ? true :false;
        }

        public async Task<List<User>> GetAllUsers()
        {
            return await this.context1.User.ToListAsync();

        }

        public async Task<User?> GetUserByEmail(string email)
        {
            return await this.context1.User
                .Where(u => u.Email == email)
                .SingleOrDefaultAsync();
        }

        public async Task<bool> UpdateUser(User user)
        {
           this.context1.User.Attach(user);
            return await this.context1.SaveChangesAsync() > 0 ? true : false;
        }

    }
}
