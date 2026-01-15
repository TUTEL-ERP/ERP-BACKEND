using Microsoft.EntityFrameworkCore;
using server.Data;
using server.Dto;
using server.Entities;
using server.Interfaces.Repository;
using server.Repository;

namespace server.Reposistory
{
    public class WishListReposistory:GenericRepository<WishList>, IWishListReposistory
    {
        private readonly DataContext context;
        public WishListReposistory(DataContext contex) : base(contex)
        {
            {

                this.context = contex;
            }
        }
        public async Task<WishList?> GetWishlistByUserIdAsync(int userId)
        {
            return await context.WishLists
                .Include(w => w.User)  // User ko include karo
                .Include(w => w.WishListItems)
                .FirstOrDefaultAsync(w => w.UserId == userId);
        }

      public async Task<WishList?> GetWishlistByUserIdIncludeProductAsync(int userId)
{
            return await _context.WishLists
                            .Include(w => w.WishListItems)
                                .ThenInclude(i => i.Product)
                                    .ThenInclude(p => p.Thumbnail)
                            .FirstOrDefaultAsync(w => w.UserId == userId);


        }

        public async Task<WishListItems> AddToWishlistAsync(AddWishListitemDto item)
        {
            // 1️⃣ Check user exists
            var user = await context.User.FindAsync(item.UserId);
            if (user == null) throw new Exception("User not found");

            // 2️⃣ Check product exists
            var product = await context.Products.FindAsync(item.ProductId);
            if (product == null) throw new Exception("Product not found");

            // 3️⃣ Get or create wishlist
            var wishlist = await GetWishlistByUserIdAsync(item.UserId);
            if (wishlist == null)
            {
                wishlist = new WishList
                {
                    UserId = item.UserId,
                    WishListItems = new List<WishListItems>()
                };
                await context.WishLists.AddAsync(wishlist);
                await context.SaveChangesAsync(); // ⚡ Save first → wishlist.Id valid
            }

            // 4️⃣ Check duplicate
            var existsInDb = await context.Set<WishListItems>()
                .AnyAsync(x => x.WishListId == wishlist.Id && x.ProductId == item.ProductId);

            if (!existsInDb)
            {
                var wishListItem = new WishListItems
                {
                    ProductId = item.ProductId,
                    WishListId = wishlist.Id
                };

                await context.Set<WishListItems>().AddAsync(wishListItem);
                await context.SaveChangesAsync();

                return wishListItem;
            }

            // 5️⃣ Return existing
            return await context.Set<WishListItems>()
                .FirstAsync(x => x.WishListId == wishlist.Id && x.ProductId == item.ProductId);
        }


        public async Task RemoveFromWishlistAsync(int userId, int productId)
        {
            var wishlist = await GetWishlistByUserIdAsync(userId);
            if (wishlist == null) throw new Exception("Wishlist not found");

            var wishListItem = wishlist.WishListItems.FirstOrDefault(w => w.ProductId == productId);
            if (wishListItem == null) throw new Exception("Item not found in wishlist");

            context.Set<WishListItems>().Remove(wishListItem);
            await context.SaveChangesAsync();
        }
        public async Task<bool> UserExistsAsync(int userId)
        {
            return await context.User.AnyAsync(u => u.UserId == userId);
        }

    }
}
