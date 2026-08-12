using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TechStore.Common.Models;
using TechStore.Model.DTOs.Cart;
using TechStore.Service.Interfaces;
using TechStoreAPI.Extensions;

namespace TechStoreAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartsController : ControllerBase
    {
        private readonly ICartService _cartService;

        public CartsController(ICartService cartService)
        {
            _cartService = cartService;
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<ApiResponse<List<CartItemResponseModel>>>> GetCartItems(int pageNumber = 1, int pageSize = 100)
        {
            var userId = User.GetRequiredUserId();

            var serviceResult = await _cartService.GetCartItems(userId, pageNumber, pageSize);

            return serviceResult.ToActionResult(this);
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse<CartItemResponseModel>>> AddProductToCart(CartItemUpdateModel model)
        {
            var userId = User.GetRequiredUserId();

            var serviceResult = await _cartService.AddToCart(userId, model);

            return serviceResult.ToActionResult(this);
        }

        [HttpPut("clear")]
        public async Task<ActionResult<ApiResponse<bool>>> ClearCart()
        {
            var userId = User.GetRequiredUserId();

            var serviceResult = await _cartService.ClearCart(userId);

            return serviceResult.ToActionResult(this);
        }

        [HttpPut("remove")]
        public async Task<ActionResult<ApiResponse<List<CartItemResponseModel>>>> RemoveCartItems(List<string> listProductId)
        {
            var userId = User.GetRequiredUserId();

            var serviceResult = await _cartService.RemoveCartItems(userId, listProductId);

            return serviceResult.ToActionResult(this);
        }

        [HttpPut("{userId}")]
        public async Task<ActionResult<ApiResponse<bool>>> UpdateCart(CartItemUpdateModel model)
        {
            var userId = User.GetRequiredUserId();

            var serviceResult = await _cartService.UpdateCart(userId, model);

            return serviceResult.ToActionResult(this);
        }
    }
}
