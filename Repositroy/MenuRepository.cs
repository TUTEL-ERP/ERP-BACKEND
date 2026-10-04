// Repository/MenuRepository.cs
using ERP_API.Data;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using server.Data;
using server.Dto;
using server.Enity;
using server.Enums;
using server.Interfaces.Repository;
using System.Data;

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
            var menus = await _dbSet
                .Where(m => m.IsActive)
                .OrderBy(m => m.DisplayOrder)
                .ToListAsync();

            return BuildTree(menus);
        }

        public async Task<List<Menu>> GetActiveMenusAsync()
        {
            return await _dbSet.Where(m => m.IsActive).OrderBy(m => m.DisplayOrder).ToListAsync();
        }

        public async Task<List<Menu>> GetMenusByParentIdAsync(int? parentId)
        {
            return await _dbSet
                .Where(m => m.ParentMenuId == parentId && m.IsActive)
                .OrderBy(m => m.DisplayOrder)
                .ToListAsync();
        }

        // ✅ NEW — Filter menus by user's role permissions
        public async Task<List<MenuDto>> GetMenuForUserAsync(string userid, MenuAction action)
        {
            // Call SP to get allowed menus
            using var connection = new SqlConnection(_context.Database.GetConnectionString());
            using var cmd = new SqlCommand("SP_MENU_SETUP", connection);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@p_user", userid);
            cmd.Parameters.AddWithValue("@p_action", action.ToString());
            cmd.Parameters.AddWithValue("@p_formid", DBNull.Value);
            cmd.Parameters.AddWithValue("@p_jsondata", DBNull.Value);
            cmd.Parameters.AddWithValue("@p_record_id", DBNull.Value);

            await connection.OpenAsync();

            var dt = new DataTable();
            using (var reader = await cmd.ExecuteReaderAsync())
            {
                dt.Load(reader);
            }

            // Convert DataTable → List<Menu>
            var allowedMenus = new List<Menu>();
            foreach (DataRow row in dt.Rows)
            {
                if (row["MenuId"] == DBNull.Value) continue;  // skip empty placeholder row
                allowedMenus.Add(new Menu
                {
                    MenuId = Convert.ToInt32(row["MenuId"]),
                    ParentMenuId = row["ParentMenuId"] == DBNull.Value ? null : Convert.ToInt32(row["ParentMenuId"]),
                    MenuName = row["MenuName"]?.ToString() ?? "",
                    Icon = row["Icon"]?.ToString(),
                    Route = row["Route"]?.ToString(),
                    DisplayOrder = row["DisplayOrder"] == DBNull.Value ? 0 : Convert.ToInt32(row["DisplayOrder"]),
                    IsActive = true
                });
            }

            return BuildTree(allowedMenus);
        }

        // ✅ Helper — build hierarchical tree from flat list
        private List<MenuDto> BuildTree(List<Menu> menus)
        {
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
    }
}