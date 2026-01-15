using server.Dto;
using server.Entities;
using server.Interfaces.Repository;
using server.Interfaces.Services;

namespace server.Services
{
    public class WishListService : IWishListService
    {
        private readonly IWishListReposistory _repo;
        private readonly IWishListItemReposistory _wishListItemRepository;
        private readonly IProductRepository _productRepository;

        public WishListService(
            IWishListReposistory repo,
            IWishListItemReposistory wishListItemRepository,
            IProductRepository productRepository
        )
        {
            _repo = repo;
            _wishListItemRepository = wishListItemRepository;
            _productRepository = productRepository;
        }
        public async Task<bool> IsProductInWishlistAsync(int userId, int productId)
        {
            var wishlist = await _repo.GetWishlistByUserIdAsync(userId);
            if (wishlist == null) return false;

            return wishlist.WishListItems
                .Any(x => x.ProductId == productId);
        }

        public async Task<WishList?> GetWishlistIncludeProductAsync(int userId)
        {
            return await _repo.GetWishlistByUserIdIncludeProductAsync(userId);
        }

        public async Task<WishListItems> AddToWishlistAsync(AddWishListitemDto item)
        {
            var userExists = await _repo.UserExistsAsync(item.UserId);
            if (!userExists)
                throw new Exception("User does not exist");

            var wishlist = await _repo.GetWishlistByUserIdAsync(item.UserId);

            if (wishlist == null)
            {
                wishlist = new WishList
                {
                    UserId = item.UserId,
                    WishListItems = new List<WishListItems>()
                };

                await _repo.AddAsync(wishlist);
            }

            var product = await _productRepository.GetByIdAsync(item.ProductId);
            if (product == null)
                throw new Exception("Product does not exist");

            var alreadyExists = wishlist.WishListItems
                .Any(x => x.ProductId == item.ProductId);

            if (alreadyExists)
                throw new InvalidOperationException("Product already in wishlist");

            var wishListItem = new WishListItems
            {
                ProductId = item.ProductId,
                WishListId = wishlist.Id
            };

            await _wishListItemRepository.AddAsync(wishListItem);

            return wishListItem;
        }

        public async Task RemoveFromWishlistAsync(int userId, int productId)
        {
            var wishlist = await _repo.GetWishlistByUserIdAsync(userId);
            if (wishlist == null)
                throw new Exception("Wishlist does not exist");

            var wishListItem = wishlist.WishListItems
                .FirstOrDefault(w => w.ProductId == productId);

            if (wishListItem == null)
                throw new Exception("Item not found in wishlist");

            await _wishListItemRepository.DeleteAsync(wishListItem);
        }
    }
}
