using server.Data;
using server.Entities;
using server.Interfaces.Repository;
using server.Repository;

namespace server.Reposistory
{
    public class CartItemReposistory : GenericRepository<CartItems>, ICartItemReposistory
    {
        private readonly DataContext _context;
        public CartItemReposistory(DataContext context) : base(context)
        {
            _context = context;
        }

    }
}

