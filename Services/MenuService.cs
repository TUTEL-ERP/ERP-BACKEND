using server.Dto;
using server.Interfaces.Repository;
using server.Interfaces.Services;

namespace server.Services
{
    public class MenuService : IMenuService
    {
        private readonly IMenuRepository repository;

        public MenuService(IMenuRepository repository)
        {
            this.repository = repository;
        }

        public async Task<List<MenuDto>> GetAll()
        {
            return await repository.GetMenuHierarchyAsync();
        }
    }
}
