using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TechStore.Common.Constants;
using TechStore.Model.DTOs.Product;
using TechStore.Model.DTOs.ProductVariant;
using TechStore.Model.DTOs.ProductVariantOption;
using TechStore.Common.Models;
using TechStore.Service.Interfaces;
using TechStoreAPI.Extensions;

namespace TechStoreAPI.Controllers
{
    [Route(RouterControllerName.AdminProducts)]
    [ApiController]
    public class AdminProductsController : ControllerBase
    {
        private readonly IProductService _productService;

        public AdminProductsController(IProductService productService)
        {
            _productService = productService;
        }

        [Authorize(Roles = AppRoles.Admin)]
        [HttpGet]
        public async Task<ActionResult<ApiResponse<PagedResult<AdminListItemProduct>>>> GetProducts([FromQuery] ProductSearchQuery query)
        {
            var serviceResult = await _productService.GetAdminProductsAsync(query);

            return serviceResult.ToActionResult(this);
        }

        [Authorize(Roles = AppRoles.Admin)]
        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<AdminProductDetailModel>>> GetAdminProduct(string id)
        {
            var serviceResult = await _productService.GetAdminProductByIdAsync(id);

            return serviceResult.ToActionResult(this);
        }

        [Authorize(Roles = AppRoles.Admin)]
        [HttpPost]
        public async Task<ActionResult<ApiResponse<string>>> AddProduct(ProductCreateModel model)
        {
            var serviceResult = await _productService.AddProduct(model);

            return serviceResult.ToActionResult(this);
        }

        [Authorize(Roles = AppRoles.Admin)]
        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse<bool>>> UpdateProduct(string id, ProductUpdateModel model)
        {
            var serviceResult = await _productService.UpdateProduct(id, model);

            return serviceResult.ToActionResult(this);
        }

        [Authorize(Roles = AppRoles.Admin)]
        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse<bool>>> DeleteProduct(string id)
        {
            var serviceResult = await _productService.DeleteProduct(id);

            return serviceResult.ToActionResult(this);
        }

        [Authorize(Roles = AppRoles.Admin)]
        [HttpPut("update-count/{id}")]
        public async Task<ActionResult<ApiResponse<bool>>> UpdateProductCount(string id, ProductCountUpdateModel model)
        {
            var serviceResult = await _productService.UpdateProductCount(id, model);

            return serviceResult.ToActionResult(this);
        }

        [Authorize(Roles = AppRoles.Admin)]
        [HttpPost("{productId}/variants")]
        public async Task<ActionResult<ApiResponse<string>>> AddProductVariant(string productId, ProductVariantCreateModel model)
        {
            var serviceResult = await _productService.AddProductVariantAsync(productId, model);

            return serviceResult.ToActionResult(this);
        }

        [Authorize(Roles = AppRoles.Admin)]
        [HttpPut("{productId}/variants/{variantId}")]
        public async Task<ActionResult<ApiResponse<bool>>> UpdateProductVariant(string productId, string variantId, ProductVariantUpdateModel model)
        {
            var serviceResult = await _productService.UpdateProductVariantAsync(productId, variantId, model);

            return serviceResult.ToActionResult(this);
        }

        [Authorize(Roles = AppRoles.Admin)]
        [HttpDelete("{productId}/variants/{variantId}")]
        public async Task<ActionResult<ApiResponse<bool>>> DeleteProductVariant(string productId, string variantId)
        {
            var serviceResult = await _productService.DeleteProductVariantAsync(productId, variantId);

            return serviceResult.ToActionResult(this);
        }

        [Authorize(Roles = AppRoles.Admin)]
        [HttpPost("{productId}/variants/{variantId}/options")]
        public async Task<ActionResult<ApiResponse<string>>> AddProductVariantOption(string productId, string variantId, ProductVariantOptionCreateModel model)
        {
            var serviceResult = await _productService.AddProductVariantOptionAsync(productId, variantId, model);

            return serviceResult.ToActionResult(this);
        }

        [Authorize(Roles = AppRoles.Admin)]
        [HttpPut("{productId}/variants/{variantId}/options/{optionId}")]
        public async Task<ActionResult<ApiResponse<bool>>> UpdateProductVariantOption(string productId, string variantId, string optionId, ProductVariantOptionUpdateModel model)
        {
            var serviceResult = await _productService.UpdateProductVariantOptionAsync(productId, variantId, optionId, model);

            return serviceResult.ToActionResult(this);
        }

        [Authorize(Roles = AppRoles.Admin)]
        [HttpDelete("{productId}/variants/{variantId}/options/{optionId}")]
        public async Task<ActionResult<ApiResponse<bool>>> DeleteProductVariantOption(string productId, string variantId, string optionId)
        {
            var serviceResult = await _productService.DeleteProductVariantOptionAsync(productId, variantId, optionId);

            return serviceResult.ToActionResult(this);
        }
    }
}
