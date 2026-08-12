using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TechStore.Common.Constants;
using TechStore.Model.DTOs.Product;
using TechStore.Common.Models;
using TechStore.Service.Interfaces;
using TechStoreAPI.Extensions;

namespace TechStoreAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<PagedResult<ListItemProductModel>>>> GetProducts([FromQuery] ProductSearchQuery query)
        {
            var serviceResult = await _productService.GetProductsAsync(query);

            return serviceResult.ToActionResult(this);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<ProductDetailModel>>> GetProductDetail(string id)
        {
            var serviceResult = await _productService.GetProductById(id);

            return serviceResult.ToActionResult(this);
        }

        [Authorize(Roles = AppRoles.Customer)]
        [HttpGet("recommended")]
        public async Task<ActionResult<ApiResponse<List<ListItemProductModel>>>> GetRecommendedProducts()
        {
            var userId = User.GetRequiredUserId();

            var serviceResult = await _productService.GetUserRecommendedProducts(userId);

            return serviceResult.ToActionResult(this);
        }

        [HttpGet("featured")]
        public async Task<ActionResult<ApiResponse<List<ListItemProductModel>>>> GetFeaturedProducts()
        {
            var serviceResult = await _productService.GetFeaturedProducts();

            return serviceResult.ToActionResult(this);
        }
    }
}
