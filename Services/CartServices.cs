using Microsoft.AspNetCore.Cors.Infrastructure;
using server.Dto;
using server.Entities;
using server.Interfaces.Repository;
using server.Interfaces.Services;

namespace server.Services
{
    public class CartService : ICartService
    {
        private readonly ICartReposistory _cartRepo;
        private readonly ICartItemReposistory _cartItemRepo;
        private readonly IProductRepository _productRepo;

        public CartService(
            ICartReposistory cartRepo,
            ICartItemReposistory cartItemRepo,
            IProductRepository productRepo
        )
        {
            _cartRepo = cartRepo;
            _cartItemRepo = cartItemRepo;
            _productRepo = productRepo;
        }

        // ✅ Get user's cart including product details
        public async Task<Cart?> GetCartIncludeProductAsync(int userId)
        {
            return await _cartRepo.GetCartByUserIdIncludeProductAsync(userId);
        }

        // ✅ Add item to cart
        public async Task<CartItems> AddToCartAsync(AddToCartReqDto item)
        {
            // 1️⃣ Check if user exists
            var userExists = await _cartRepo.UserExistsAsync(item.UserId);
            if (!userExists)
                throw new Exception("User does not exist");

            // 2️⃣ Load user's cart
            var cart = await _cartRepo.GetCartByUserIdAsync(item.UserId);
            if (cart == null)
            {
                cart = new Cart
                {
                    UserId = item.UserId
                };
                await _cartRepo.AddAsync(cart);
            }

            // 3️⃣ Check if product exists
            var product = await _productRepo.GetByIdAsync(item.ProductId);
            if (product == null)
                throw new Exception("Product does not exist");

            // 4️⃣ Check for duplicate
            var existing = cart.CartItems.FirstOrDefault(x => x.ProductId == item.ProductId);
            if (existing != null)
            {
                // Optional: increment quantity if already in cart
                //existing.Quantity += item.Quantity;
                await _cartItemRepo.UpdateAsync(existing);
                return existing;
            }

            // 5️⃣ Add new cart item
            var cartItem = new CartItems
            {
                ProductId = item.ProductId,
                CartId = cart.Id,
                //Quantity = item.Quantity
            };

            await _cartItemRepo.AddAsync(cartItem);
            return cartItem;
        }

        // ✅ Remove item from cart
        public async Task RemoveFromCartAsync(int userId, int productId)
        {
            var cart = await _cartRepo.GetCartByUserIdAsync(userId);
            if (cart == null)
                throw new Exception("Cart does not exist");

            var cartItem = cart.CartItems.FirstOrDefault(x => x.ProductId == productId);
            if (cartItem == null)
                throw new Exception("Item not found in cart");

            await _cartItemRepo.DeleteAsync(cartItem);
        }
    }
}
