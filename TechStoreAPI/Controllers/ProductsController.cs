using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Threading.Tasks;
using TechStore.Common.Constants;
using TechStore.Common.Enums;
using TechStore.Common.Models;
using TechStore.Model.DTOs.Product;
using TechStore.Service.Interfaces;

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

        [HttpGet("filter")]
        public async Task<ApiResponse<PagedResult<ListItemProductModel>>> GetProductsFiltered([FromQuery]ProductSearchQuery query)
        {
            var serviceResult = await _productService.GetProductsFilteredAsync(query);

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

        //[HttpGet]
        //public async Task<ApiResponse<IEnumerable<ListItemProductModel>>> GetProducts(int page = 1, int pageSize = 20)
        //{
        //    var serviceResult = await _productService.GetProductsAsync(page, pageSize);

        //    if (serviceResult.IsSuccess)
        //    {
        //        return new()
        //        {
        //            PartnerCode = Messenger.SuccessFull,
        //            RetCode = ERetCode.Successfull,
        //            Data = serviceResult.Data,
        //            SystemMessage = serviceResult.Message,
        //            StatusCode = (int)HttpStatusCode.OK
        //        };
        //    }
        //    else
        //    {
        //        return new()
        //        {
        //            PartnerCode = Messenger.NoExitData,
        //            RetCode = ERetCode.NoExitData,
        //            Data = serviceResult.Data,
        //            SystemMessage = serviceResult.Message,
        //            StatusCode = (int)HttpStatusCode.OK
        //        };
        //    }
        //}

        [HttpGet("{id}")]
        public async Task<ApiResponse<ProductDetailModel>> GetProductDetail(string id)
        {
            var serviceResult = await _productService.GetProductById(id);

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

        [HttpGet("recommended/{userId}")]
        public async Task<ApiResponse<IEnumerable<ListItemProductModel>>> GetRecommendedProducts(string userId)
        {
            //var userId = User.FindFirstValue(AppClaims.UserId);

            if (userId != null)
            {
                var serviceResult = await _productService.GetUserRecommendedProducts(userId);

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
            else
            {
                return new()
                {
                    PartnerCode = Messenger.SuccessFull,
                    RetCode = ERetCode.Successfull,
                    Data = null,
                    SystemMessage = Messenger.LoginError,
                    StatusCode = (int)HttpStatusCode.ExpectationFailed
                };
            }
        }

        [HttpGet("search")]
        public async Task<ApiResponse<IEnumerable<ListItemProductModel>>> SearchByName(string keyword, int pageNumber = 1, int pageSize = 12)
        {
            var serviceResult = await _productService.SearchByNameAsync(keyword, pageNumber, pageSize);

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

        [HttpGet("category")]
        public async Task<ApiResponse<IEnumerable<ListItemProductModel>>> GetProductsByCategory(string categorySlug, int pageNumber = 1, int pageSize = 12)
        {

            var serviceResult = await _productService.GetProductsByCategory(categorySlug, pageNumber, pageSize);

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

        [HttpGet("category-brand")]
        public async Task<ApiResponse<IEnumerable<ListItemProductModel>>> GetProductsByCategoryAndBrand(string categorySlug, string brandSlug, int pageNumber = 1, int pageSize = 12)
        {

            var serviceResult = await _productService.GetProductsByCategoryAndBrand(categorySlug, brandSlug, pageNumber, pageSize);

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

        [HttpGet("featured")]
        public async Task<ApiResponse<IEnumerable<ListItemProductModel>>> GetFeaturedProducts()
        {
            var serviceResult = await _productService.GetFeaturedProducts();

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
    }
}
