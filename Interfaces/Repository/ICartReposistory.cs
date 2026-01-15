using server.Dto;
using server.Entities;

namespace server.Interfaces.Repository
{
    public interface ICartReposistory:IGenericReposistroy<Cart>    {
        Task<Cart?> GetCartByUserIdIncludeProductAsync(int userId);
        Task<Cart?> GetCartByUserIdAsync(int userId);
        Task<CartItems> AddToCartAsync(AddToCartReqDto item);  // <- return type changed
        Task RemoveFromCartAsync(int userId, int productId);
        Task<bool> UserExistsAsync(int userId); // ✅ ADD THIS
    }
}
