// Repository/MenuRepository.cs
using ERP_API.Data;
using Microsoft.EntityFrameworkCore;
using server.Data;
using server.Dto;
using server.Enity;
using server.Interfaces.Repository;

namespace server.Repository
{
    public class MenuRepository : GenericRepository<Menu>, IMenuRepository
    {
        private readonly ApplicationDbContext _context;

        public MenuRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<MenuDto>> GetMenuHierarchyAsync()
        {
            // Get all active menus ordered by DisplayOrder
            var menus = await _dbSet
                .Where(m => m.IsActive)
                .OrderBy(m => m.DisplayOrder)
                .ToListAsync();

            // Convert to DTOs
            var menuDtos = menus.Select(m => new MenuDto
            {
                MenuId = m.MenuId,
                ParentMenuId = m.ParentMenuId,
                MenuName = m.MenuName,
                Icon = m.Icon,
                Route = m.Route,
                DisplayOrder = m.DisplayOrder,
                IsActive = m.IsActive,
                Children = new List<MenuDto>(),
                Expanded = false
            }).ToList();

            // Build hierarchy
            var menuDict = menuDtos.ToDictionary(m => m.MenuId);
            var rootMenus = new List<MenuDto>();

            foreach (var menu in menuDtos)
            {
                if (menu.ParentMenuId.HasValue && menuDict.ContainsKey(menu.ParentMenuId.Value))
                {
                    var parent = menuDict[menu.ParentMenuId.Value];
                    parent.Children.Add(menu);
                }
                else
                {
                    rootMenus.Add(menu);
                }
            }

            return rootMenus;
        }

        public async Task<List<Menu>> GetActiveMenusAsync()
        {
            return await _dbSet
                .Where(m => m.IsActive)
                .OrderBy(m => m.DisplayOrder)
                .ToListAsync();
        }

        public async Task<List<Menu>> GetMenusByParentIdAsync(int? parentId)
        {
            return await _dbSet
                .Where(m => m.ParentMenuId == parentId && m.IsActive)
                .OrderBy(m => m.DisplayOrder)
                .ToListAsync();
        }
    }
}