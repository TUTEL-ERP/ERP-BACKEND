using Microsoft.EntityFrameworkCore;
using server.Data;
using server.Entities;
using server.Interfaces.Repository;
using server.Repository;

public class WishListItemRepository : GenericRepository<WishListItems>, IWishListItemReposistory
{
    public WishListItemRepository(DataContext context) : base(context)
    {
    }
}
