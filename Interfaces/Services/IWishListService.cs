using server.Dto;
using server.Entities;

namespace server.Interfaces.Services
{
    public interface IWishListService
    {
        Task<WishList?> GetWishlistIncludeProductAsync(int userId);
        Task<WishListItems> AddToWishlistAsync(AddWishListitemDto item); // change from Task to Task<WishListItems>
        Task RemoveFromWishlistAsync(int userId, int productId);
        Task<bool> IsProductInWishlistAsync(int userId, int productId);

    }

}
    