using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using server.Dto;
using server.Interfaces.Services;

namespace server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CartController : ControllerBase
    {
        private readonly ICartService _cartService;
        private readonly IMapper _mapper;

        public CartController(ICartService cartService, IMapper mapper)
        {
            _cartService = cartService;
            _mapper = mapper;
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetCart(int userId)
        {
            var cart = await _cartService.GetCartIncludeProductAsync(userId);
            if (cart == null) return NotFound("Cart not found");
            return Ok(cart);
        }

        [HttpPost("Add")]
        public async Task<ActionResult<ResponceDto>> AddToCart([FromBody] AddToCartReqDto cartItemDto)
        {
            var res = new ResponceDto();
            try
            {
                var addedItem = await _cartService.AddToCartAsync(cartItemDto);
                res.IsSuccessed = true;
                res.Message = "Product added to cart.";
                res.Data = addedItem;
                return Ok(res);
            }
            catch (Exception ex)
            {
                res.IsSuccessed = false;
                res.Message = ex.Message;
                return StatusCode(500, res);
            }
        }

        [HttpDelete("Remove/{userId}/{productId}")]
        public async Task<ActionResult<ResponceDto>> RemoveFromCart(int userId, int productId)
        {
            var res = new ResponceDto();
            try
            {
                await _cartService.RemoveFromCartAsync(userId, productId);
                res.IsSuccessed = true;
                res.Message = "Product removed from cart.";
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
