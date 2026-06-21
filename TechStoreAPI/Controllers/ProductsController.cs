using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Security.Claims;
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

        [HttpGet]
        public async Task<ApiResponse<PagedResult<ListItemProductModel>>> GetProducts([FromQuery]ProductSearchQuery query)
        {
            var serviceResult = await _productService.GetProductsAsync(query);

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

        [HttpGet("recommended")]
        public async Task<ApiResponse<IEnumerable<ListItemProductModel>>> GetRecommendedProducts()
        {
            var userId = User.FindFirstValue(AppClaims.UserId);

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
