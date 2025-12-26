using Microsoft.EntityFrameworkCore;
using server.Data;
using server.Entities;
using server.Interfaces.Repository;
using server.Repository;

namespace server.Reposistory
{
    public class BrandRepository:GenericRepository<Brand>,IBrandReposistory

    {
        private readonly DataContext context;
        public BrandRepository(DataContext context):base(context)
        {
            this.context = context;
        }

      public async Task<IEnumerable<Brand>> GetAllIncludingImage()
        {
            return await context.brand
                .Include(b=> b.Image).ToListAsync();
        }


    }
}
