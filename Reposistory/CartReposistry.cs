using Microsoft.EntityFrameworkCore;
using server.Data;
using server.Dto;
using server.Entities;
using server.Interfaces.Repository;
using server.Repository;

namespace server.Reposistory
{
    public class CartReposistory : GenericRepository<Cart>, ICartReposistory
    {
        private readonly DataContext _context;

        public CartReposistory(DataContext context) : base(context)
        {
            _context = context;
        }

        // ✅ Get cart by user including product details
        public async Task<Cart?> GetCartByUserIdIncludeProductAsync(int userId)
        {
            return await _context.Cart
                .Include(c => c.CartItems)
                    .ThenInclude(i => i.Product)
                        .ThenInclude(p => p.Thumbnail)
                .FirstOrDefaultAsync(c => c.UserId == userId);
        }

        public async Task<Cart?> GetCartByUserIdAsync(int userId)
        {
            return await _context.Cart
                .Include(c => c.CartItems)
                .FirstOrDefaultAsync(c => c.UserId == userId);
        }

        public async Task<CartItems> AddToCartAsync(AddToCartReqDto item)
        {
            var userExists = await UserExistsAsync(item.UserId);
            if (!userExists) throw new Exception("User does not exist");

            var cart = await GetCartByUserIdAsync(item.UserId);
            if (cart == null)
            {
                cart = new Cart
                {
                    UserId = item.UserId,
                    CartItems = new List<CartItems>()
                };
                await _context.Cart.AddAsync(cart);
                await _context.SaveChangesAsync(); // Save to get Cart.Id
            }

            var product = await _context.Products.FindAsync(item.ProductId);
            if (product == null) throw new Exception("Product not found");

            var existing = cart.CartItems.FirstOrDefault(x => x.ProductId == item.ProductId);
            if (existing != null)
            {
                _context.CartItems.Update(existing);
                await _context.SaveChangesAsync();
                return existing;
            }

            var cartItem = new CartItems
            {
                ProductId = item.ProductId,
                CartId = cart.Id,
                //Quantity = item.Quantity
            };

            await _context.CartItems.AddAsync(cartItem);
            await _context.SaveChangesAsync();

            return cartItem;
        }

        public async Task RemoveFromCartAsync(int userId, int productId)
        {
            var cart = await GetCartByUserIdAsync(userId);
            if (cart == null) throw new Exception("Cart not found");

            var cartItem = cart.CartItems.FirstOrDefault(x => x.ProductId == productId);
            if (cartItem == null) throw new Exception("Item not found in cart");

            _context.CartItems.Remove(cartItem);
            await _context.SaveChangesAsync();
        }

        // ✅ Check if user exists
        public async Task<bool> UserExistsAsync(int userId)
        {
            return await _context.User.AnyAsync(u => u.UserId == userId);
        }
    }
}
