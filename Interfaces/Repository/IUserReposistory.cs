using server.Entities;

namespace server.Interfaces.Repository
{
    public interface IUserReposistory
    {
        Task<User> GetUserByEmail(string email);
        Task<bool> AddUser(User user);
        Task<bool> UpdateUser(User user);
        Task<List<User>> GetAllUsers(); 
    }
}
