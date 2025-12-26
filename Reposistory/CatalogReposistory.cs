using Microsoft.EntityFrameworkCore;
using server.Data;
using server.Entities;
using server.Interfaces.Repository;
using server.Repository;

namespace server.Reposistory
{
    public class CategoryReposistory : GenericRepository<Categories>,ICategoryReposistory

    {
        private readonly DataContext context;

        public CategoryReposistory(DataContext contex):base(contex) {    
        {
            
          this.context  = contex;        
        }
    }

        public async Task<IEnumerable<Categories>> GetAllIncludingImage()
        {
            return await context.categories
                .Include(b => b.Image).ToListAsync();
        }
    }

}
