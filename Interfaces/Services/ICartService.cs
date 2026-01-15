using server.Dto;
using server.Entities;

namespace server.Interfaces.Services
{
    public interface ICartService
    {
        Task<Cart?> GetCartIncludeProductAsync(int userId);
        Task<CartItems> AddToCartAsync(AddToCartReqDto item);
        Task RemoveFromCartAsync(int userId, int productId);

    }
}
