using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using server.Dto;
using server.Interfaces.Services;

namespace server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WishListController : ControllerBase
    {
        private readonly IWishListService _wishListService;
        private readonly IMapper _mapper;

        public WishListController(IWishListService wishListService, IMapper mapper)
        {
            _wishListService = wishListService;
            _mapper = mapper;
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetWishlist(int userId)
        {
            var wishlist = await _wishListService.GetWishlistIncludeProductAsync(userId);
            if (wishlist == null) return NotFound("Wishlist not found");
            return Ok(wishlist);
        }
        [HttpPost("Add")]
        public async Task<ActionResult<ResponceDto>> AddToWishlist(
            [FromBody] AddWishListitemDto wishlistItemDto)
        {
            ResponceDto res = new();

            try
            {
                var addedItem = await _wishListService.AddToWishlistAsync(wishlistItemDto);

                res.IsSuccessed = true;
                res.Message = "Product added to wishlist.";
                res.Data = addedItem;

                return Ok(res);
            }
            catch (InvalidOperationException ex)
            {
                res.IsSuccessed = false;
                res.Message = ex.Message;

                return Conflict(res); // 👈 409
            }
            catch (Exception ex)
            {
                res.IsSuccessed = false;
                res.Message = "Something went wrong";

                return StatusCode(500, res);
            }
        }


        [HttpDelete("Remove/{userId}/{productId}")]
        public async Task<ActionResult<ResponceDto>> RemoveFromWishlist(int userId, int productId)
        {
            ResponceDto res = new ResponceDto();
            try
            {
                await _wishListService.RemoveFromWishlistAsync(userId, productId);
                res.IsSuccessed = true;
                res.Message = "Product removed from wishlist.";
                return Ok(res);
            }
            catch (Exception ex)
            {
                res.IsSuccessed = false;
                res.Message = ex.Message;
                return StatusCode(500, res);
            }
        }
    }
}
