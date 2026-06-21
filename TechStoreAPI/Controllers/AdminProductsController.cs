using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using TechStore.Common.Constants;
using TechStore.Common.Enums;
using TechStore.Common.Models;
using TechStore.Model.DTOs.Product;
using TechStore.Model.DTOs.ProductVariant;
using TechStore.Model.DTOs.ProductVariantOption;
using TechStore.Service.Interfaces;

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
        public async Task<ApiResponse<PagedResult<AdminListItemProduct>>> GetProducts([FromQuery] ProductSearchQuery query)
        {
            var serviceResult = await _productService.GetAdminProductsAsync(query);

            if (serviceResult.IsSuccess)
            {
                return new()
                {
                    PartnerCode = Messenger.SuccessFull,
                    RetCode = ERetCode.Successfull,
                    Data = serviceResult.Data,
                    SystemMessage = serviceResult.Message,
                    StatusCode = (int)HttpStatusCode.OK
                };
            }
            else
            {
                return new()
                {
                    PartnerCode = Messenger.NoExitData,
                    RetCode = ERetCode.NoExitData,
                    Data = serviceResult.Data,
                    SystemMessage = serviceResult.Message,
                    StatusCode = (int)HttpStatusCode.OK
                };
            }
        }

        [Authorize(Roles = AppRoles.Admin)]
        [HttpGet("{id}")]
        public async Task<ApiResponse<AdminProductDetailModel>> GetAdminProduct(string id)
        {
            var result = await _productService.GetAdminProductByIdAsync(id);

            if (result != null)
            {
                return new ApiResponse<AdminProductDetailModel>
                {
                    PartnerCode = Messenger.GetDataSuccessful,
                    RetCode = ERetCode.Successfull,
                    Data = result.Data,
                    SystemMessage = string.Empty,
                    StatusCode = (int)HttpStatusCode.OK
                };
            }
            else
            {
                return new ApiResponse<AdminProductDetailModel>
                {
                    PartnerCode = Messenger.GetDataSuccessful,
                    RetCode = ERetCode.Successfull,
                    Data = null,
                    SystemMessage = string.Empty,
                    StatusCode = (int)HttpStatusCode.NotFound
                };
            }
        }

        //[Authorize(Roles = $"{AppRoles.Admin},{AppRoles.Manager}")]
        [Authorize(Roles = AppRoles.Admin)]
        [HttpPost]
        public async Task<ApiResponse<string>> AddProduct(ProductCreateModel model)
        {
            var result = await _productService.AddProduct(model);

            if (result.IsSuccess == true)
            {
                return new ApiResponse<string>
                {
                    PartnerCode = Messenger.SuccessFull,
                    RetCode = ERetCode.Successfull,
                    Data = result.Data,
                    SystemMessage = Messenger.SuccessFull,
                    StatusCode = (int)HttpStatusCode.Created
                };
            }
            else
            {
                return new ApiResponse<string>
                {
                    PartnerCode = Messenger.CreateDataError,
                    RetCode = ERetCode.BadRequest,
                    Data = null,
                    SystemMessage = Messenger.CreateDataError,
                    StatusCode = (int)HttpStatusCode.BadRequest
                };
            }
        }

        [Authorize(Roles = AppRoles.Admin)]
        [HttpPut("{id}")]
        public async Task<ApiResponse<bool>> UpdateProduct(string id, ProductUpdateModel model)
        {
            var result = await _productService.UpdateProduct(id, model);

            if (result.IsSuccess == true)
            {
                return new ApiResponse<bool>
                {
                    PartnerCode = Messenger.SuccessFull,
                    RetCode = ERetCode.Successfull,
                    Data = result.Data,
                    SystemMessage = Messenger.SuccessFull,
                    StatusCode = (int)HttpStatusCode.OK
                };
            }
            else
            {
                return new ApiResponse<bool>
                {
                    PartnerCode = Messenger.UpdateDataError,
                    RetCode = ERetCode.BadRequest,
                    Data = false,
                    SystemMessage = Messenger.UpdateDataError,
                    StatusCode = (int)HttpStatusCode.BadRequest
                };
            }
        }

        [Authorize(Roles = AppRoles.Admin)]
        [HttpDelete("{id}")]
        public async Task<ApiResponse<bool>> DeleteProduct(string id)
        {
            var result = await _productService.DeleteProduct(id);

            if (result.IsSuccess == true)
            {
                return new ApiResponse<bool>
                {
                    PartnerCode = Messenger.SuccessFull,
                    RetCode = ERetCode.Successfull,
                    Data = result.Data,
                    SystemMessage = Messenger.SuccessFull,
                    StatusCode = (int)HttpStatusCode.OK
                };
            }
            else
            {
                return new ApiResponse<bool>
                {
                    PartnerCode = Messenger.SystemError,
                    RetCode = ERetCode.SystemError,
                    Data = false,
                    SystemMessage = Messenger.SystemError,
                    StatusCode = (int)HttpStatusCode.BadRequest
                };
            }
        }

        [Authorize(Roles = AppRoles.Admin)]
        [HttpPut("update-count/{id}")]
        public async Task<ApiResponse<bool>> UpdateProductCount(string id, ProductCountUpdateModel model)
        {
            var serviceResult = await _productService.UpdateProductCount(id, model);

            if (serviceResult.IsSuccess)
            {
                return new ApiResponse<bool>
                {
                    PartnerCode = Messenger.SuccessFull,
                    RetCode = ERetCode.Successfull,
                    Data = serviceResult.Data,
                    SystemMessage = serviceResult.Message,
                    StatusCode = (int)HttpStatusCode.OK
                };
            }
            else
            {
                return new ApiResponse<bool>
                {
                    PartnerCode = Messenger.UpdateDataError,
                    RetCode = ERetCode.BadRequest,
                    Data = serviceResult.Data,
                    SystemMessage = serviceResult.Message,
                    StatusCode = (int)HttpStatusCode.BadRequest
                };
            }
        }

        [Authorize(Roles = AppRoles.Admin)]
        [HttpPost("{productId}/variants")]
        public async Task<ApiResponse<string>> AddProductVariant(string productId, ProductVariantCreateModel model)
        {
            var serviceResult = await _productService.AddProductVariantAsync(productId, model);

            if (serviceResult.IsSuccess)
            {
                return new ApiResponse<string>
                {
                    PartnerCode = Messenger.SuccessFull,
                    RetCode = ERetCode.Successfull,
                    Data = serviceResult.Data,
                    SystemMessage = serviceResult.Message,
                    StatusCode = (int)HttpStatusCode.OK
                };
            }
            else
            {
                return new ApiResponse<string>
                {
                    PartnerCode = Messenger.UpdateDataError,
                    RetCode = ERetCode.BadRequest,
                    Data = serviceResult.Data,
                    SystemMessage = serviceResult.Message,
                    StatusCode = (int)HttpStatusCode.BadRequest
                };
            }
        }

        [Authorize(Roles = AppRoles.Admin)]
        [HttpPut("{productId}/variants/{variantId}")]
        public async Task<ApiResponse<bool>> UpdateProductVariant(string productId, string variantId, ProductVariantUpdateModel model)
        {
            var serviceResult = await _productService.UpdateProductVariantAsync(productId, variantId, model);

            if (serviceResult.IsSuccess)
            {
                return new ApiResponse<bool>
                {
                    PartnerCode = Messenger.SuccessFull,
                    RetCode = ERetCode.Successfull,
                    Data = serviceResult.Data,
                    SystemMessage = serviceResult.Message,
                    StatusCode = (int)HttpStatusCode.OK
                };
            }
            else
            {
                return new ApiResponse<bool>
                {
                    PartnerCode = Messenger.UpdateDataError,
                    RetCode = ERetCode.BadRequest,
                    Data = serviceResult.Data,
                    SystemMessage = serviceResult.Message,
                    StatusCode = (int)HttpStatusCode.BadRequest
                };
            }
        }

        [Authorize(Roles = AppRoles.Admin)]
        [HttpDelete("{productId}/variants/{variantId}")]
        public async Task<ApiResponse<bool>> DeleteProductVariant(string productId, string variantId)
        {
            var serviceResult = await _productService.DeleteProductVariantAsync(productId, variantId);

            if (serviceResult.IsSuccess)
            {
                return new ApiResponse<bool>
                {
                    PartnerCode = Messenger.SuccessFull,
                    RetCode = ERetCode.Successfull,
                    Data = serviceResult.Data,
                    SystemMessage = serviceResult.Message,
                    StatusCode = (int)HttpStatusCode.OK
                };
            }
            else
            {
                return new ApiResponse<bool>
                {
                    PartnerCode = Messenger.SystemError,
                    RetCode = ERetCode.BadRequest,
                    Data = serviceResult.Data,
                    SystemMessage = serviceResult.Message,
                    StatusCode = (int)HttpStatusCode.BadRequest
                };
            }
        }

        [Authorize(Roles = AppRoles.Admin)]
        [HttpPost("{productId}/variants/{variantId}/options")]
        public async Task<ApiResponse<string>> AddProductVariantOption(string productId, string variantId, ProductVariantOptionCreateModel model)
        {
            var serviceResult = await _productService.AddProductVariantOptionAsync(productId, variantId, model);

            if (serviceResult.IsSuccess)
            {
                return new ApiResponse<string>
                {
                    PartnerCode = Messenger.SuccessFull,
                    RetCode = ERetCode.Successfull,
                    Data = serviceResult.Data,
                    SystemMessage = serviceResult.Message,
                    StatusCode = (int)HttpStatusCode.OK
                };
            }
            else
            {
                return new ApiResponse<string>
                {
                    PartnerCode = Messenger.UpdateDataError,
                    RetCode = ERetCode.BadRequest,
                    Data = serviceResult.Data,
                    SystemMessage = serviceResult.Message,
                    StatusCode = (int)HttpStatusCode.BadRequest
                };
            }
        }

        [Authorize(Roles = AppRoles.Admin)]
        [HttpPut("{productId}/variants/{variantId}/options/{optionId}")]
        public async Task<ApiResponse<bool>> UpdateProductVariantOption(string productId, string variantId, string optionId, ProductVariantOptionUpdateModel model)
        {
            var serviceResult = await _productService.UpdateProductVariantOptionAsync(productId, variantId, optionId, model);

            if (serviceResult.IsSuccess)
            {
                return new ApiResponse<bool>
                {
                    PartnerCode = Messenger.SuccessFull,
                    RetCode = ERetCode.Successfull,
                    Data = serviceResult.Data,
                    SystemMessage = serviceResult.Message,
                    StatusCode = (int)HttpStatusCode.OK
                };
            }
            else
            {
                return new ApiResponse<bool>
                {
                    PartnerCode = Messenger.UpdateDataError,
                    RetCode = ERetCode.BadRequest,
                    Data = serviceResult.Data,
                    SystemMessage = serviceResult.Message,
                    StatusCode = (int)HttpStatusCode.BadRequest
                };
            }
        }

        [Authorize(Roles = AppRoles.Admin)]
        [HttpDelete("{productId}/variants/{variantId}/options/{optionId}")]
        public async Task<ApiResponse<bool>> DeleteProductVariantOption(string productId, string variantId, string optionId)
        {
            var serviceResult = await _productService.DeleteProductVariantOptionAsync(productId, variantId, optionId);

            if (serviceResult.IsSuccess)
            {
                return new ApiResponse<bool>
                {
                    PartnerCode = Messenger.SuccessFull,
                    RetCode = ERetCode.Successfull,
                    Data = serviceResult.Data,
                    SystemMessage = serviceResult.Message,
                    StatusCode = (int)HttpStatusCode.OK
                };
            }
            else
            {
                return new ApiResponse<bool>
                {
                    PartnerCode = Messenger.UpdateDataError,
                    RetCode = ERetCode.BadRequest,
                    Data = serviceResult.Data,
                    SystemMessage = serviceResult.Message,
                    StatusCode = (int)HttpStatusCode.BadRequest
                };
            }
        }
    }
}
