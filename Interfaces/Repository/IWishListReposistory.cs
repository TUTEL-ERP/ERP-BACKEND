using server.Dto;
using server.Entities;

namespace server.Interfaces.Repository
{
    public interface IWishListReposistory:IGenericReposistroy<WishList>
    {
        Task<WishList?> GetWishlistByUserIdIncludeProductAsync(int userId);
        Task<WishList?> GetWishlistByUserIdAsync(int userId);
        Task<WishListItems> AddToWishlistAsync(AddWishListitemDto item);  // <- return type changed
        Task RemoveFromWishlistAsync(int userId, int productId);
        Task<bool> UserExistsAsync(int userId); // ✅ ADD THIS
    }
}
