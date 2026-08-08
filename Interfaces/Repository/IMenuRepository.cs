using server.Dto;
using server.Enity;

namespace server.Interfaces.Repository
{
    public interface IMenuRepository : IGenericRepository<Menu>
    {
        Task<List<MenuDto>> GetMenuHierarchyAsync();
        Task<List<Menu>> GetActiveMenusAsync();
        Task<List<Menu>> GetMenusByParentIdAsync(int? parentId);
    }
}