using server.Entities;

namespace server.Interfaces.Services
{
    public interface IImageServies
    {
        Task<Image> SaveImageAsync(IFormFile file);
        Task DeleteImageAsync(int id);
    }
}
