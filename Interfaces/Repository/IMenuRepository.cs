using server.Dto;
using server.Enums;
using server.Enity;
using System.Data;

namespace server.Interfaces.Repository
{
    public interface IMenuRepository : IGenericRepository<Menu>
    {
        Task<List<MenuDto>> GetMenuHierarchyAsync();
        Task<List<Menu>> GetActiveMenusAsync();
        Task<List<Menu>> GetMenusByParentIdAsync(int? parentId);
        Task<List<MenuDto>> GetMenuForUserAsync(string userid, MenuAction action);   
    }
}