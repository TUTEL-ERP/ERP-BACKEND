using server.Dto;

namespace server.Interfaces.Repository
{
    public interface IMenuRepository
    {
        Task<List<MenuDto>> GetMenuHierarchyAsync();
    }
}