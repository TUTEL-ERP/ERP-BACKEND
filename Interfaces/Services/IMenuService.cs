using server.Dto;

namespace server.Interfaces.Services
{
    public interface IMenuService
    {
        Task<List<MenuDto>> GetAll();
    }
}
