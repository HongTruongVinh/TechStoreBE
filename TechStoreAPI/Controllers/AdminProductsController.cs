using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using TechStore.Common.Constants;
using TechStore.Common.Enums;
using TechStore.Common.Models;
using TechStore.Service.Interfaces;
using TechStore.Model.DTOs.Product;

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

        [HttpGet]
        public async Task<ApiResponse<List<AdminProductDetailModel>>> GetProducts(int pageNumber = 1, int pageSize = 20)
        {
            var serviceResult = await _productService.GetAdminProducts(pageNumber, pageSize);

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

        [HttpGet("filter")]
        public async Task<ApiResponse<IEnumerable<ProductListItemModel>>> GetProductsFiltered(
            int page = 1,
            int pageSize = 20,
            string? categoryId = null,
            decimal? minPrice = null,
            decimal? maxPrice = null,
            string? brandId = null
            )
        {
            var serviceResult = await _productService.GetProductsFilteredAsync(
                page = 1,
                pageSize = 20,
                categoryId,
                minPrice,
                maxPrice,
                brandId
                );

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

        //[Authorize(Roles = AppRoles.Admin)]
        [HttpGet("{id}")]
        public async Task<ApiResponse<AdminProductDetailModel>> Get(string id)
        {
            var result = await _productService.GetAdminProductById(id);

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
        [HttpPost]
        public async Task<ApiResponse<string>> Post(ProductCreateModel model)
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

        //[Authorize(Roles = AppRoles.Admin)]
        [HttpPut("{id}")]
        public async Task<ApiResponse<bool>> Put(string id, ProductUpdateModel model)
        {
            var result = await _productService.UpdateProductInformation(id, model);

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

        //[Authorize(Roles = AppRoles.Admin)]
        [HttpDelete("{id}")]
        public async Task<ApiResponse<bool>> Delete(string id)
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

        //[Authorize(Roles = AppRoles.Admin)]
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
                    Data = serviceResult.Data,
                    SystemMessage = serviceResult.Message,
                    StatusCode = (int)HttpStatusCode.BadRequest
                };
            }
        }
    }
}
