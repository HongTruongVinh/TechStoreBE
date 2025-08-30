//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;
//using System.Net;
//using TechStoreModel.Base;
//using TechStoreModel.Constants;
//using TechStoreModel.Enums;
//using TechStoreModel.Model.Product;
//using TechStoreServices.IServices;
//using TechStoreServices.Services;

//namespace TechStoreAPI.Controllers
//{
//    [Route(RouterControllerName.AdminProducts)]
//    [ApiController]
//    public class AdminProductsController : ControllerBase
//    {
//        private readonly IProductService _productService;

//        public AdminProductsController(IProductService productService)
//        {
//            _productService = productService;
//        }

//        [HttpGet]
//        public async Task<ApiResponse<List<AdminProductResponseModel>>> GetProducts()
//        {
//            var serviceResult = await _productService.GetAdminProducts();

//            if (serviceResult.IsSuccess)
//            {
//                return new()
//                {
//                    PartnerCode = Messenger.SuccessFull,
//                    RetCode = ERetCode.Successfull,
//                    Data = serviceResult.Data,
//                    SystemMessage = serviceResult.Message,
//                    StatusCode = (int)HttpStatusCode.OK
//                };
//            }
//            else
//            {
//                return new()
//                {
//                    PartnerCode = Messenger.NoExitData,
//                    RetCode = ERetCode.NoExitData,
//                    Data = serviceResult.Data,
//                    SystemMessage = serviceResult.Message,
//                    StatusCode = (int)HttpStatusCode.OK
//                };
//            }
//        }

//        [HttpGet("category/{categoryId}")]
//        public async Task<ApiResponse<List<ProductListItemModel>>> GetProductsByCategoryId(string categoryId)
//        {
//            var serviceResult = await _productService.GetProductsByCategoryId(categoryId);

//            if (serviceResult.IsSuccess)
//            {
//                return new()
//                {
//                    PartnerCode = Messenger.SuccessFull,
//                    RetCode = ERetCode.Successfull,
//                    Data = serviceResult.Data,
//                    SystemMessage = serviceResult.Message,
//                    StatusCode = (int)HttpStatusCode.OK
//                };
//            }
//            else
//            {
//                return new()
//                {
//                    PartnerCode = Messenger.NoExitData,
//                    RetCode = ERetCode.NoExitData,
//                    Data = serviceResult.Data,
//                    SystemMessage = serviceResult.Message,
//                    StatusCode = (int)HttpStatusCode.OK
//                };
//            }
//        }

//        //[Authorize(Roles = AppRoles.Admin)]
//        [HttpGet("{id}")]
//        public async Task<ApiResponse<AdminProductResponseModel>> Get(string id)
//        {
//            var result = await _productService.GetAdminProductById(id);

//            if (result != null)
//            {
//                return new ApiResponse<AdminProductResponseModel>
//                {
//                    PartnerCode = Messenger.GetDataSuccessful,
//                    RetCode = ERetCode.Successfull,
//                    Data = await _productService.GetAdminProductById(id),
//                    SystemMessage = string.Empty,
//                    StatusCode = (int)HttpStatusCode.OK
//                };
//            }
//            else
//            {
//                return new ApiResponse<AdminProductResponseModel>
//                {
//                    PartnerCode = Messenger.GetDataSuccessful,
//                    RetCode = ERetCode.Successfull,
//                    Data = await _productService.GetAdminProductById(id),
//                    SystemMessage = string.Empty,
//                    StatusCode = (int)HttpStatusCode.NotFound
//                };
//            }
//        }

//        //[Authorize(Roles = $"{AppRoles.Admin},{AppRoles.Manager}")]
//        [HttpPost]
//        public async Task<ApiResponse<string>> Post(ProductCreateModel model)
//        {
//            var result = await _productService.AddProduct(model);

//            if (!String.IsNullOrEmpty(result))
//            {
//                return new ApiResponse<string>
//                {
//                    PartnerCode = Messenger.SuccessFull,
//                    RetCode = ERetCode.Successfull,
//                    Data = result,
//                    SystemMessage = Messenger.SuccessFull,
//                    StatusCode = (int)HttpStatusCode.Created
//                };
//            }
//            else
//            {
//                return new ApiResponse<string>
//                {
//                    PartnerCode = Messenger.CreateDataError,
//                    RetCode = ERetCode.BadRequest,
//                    Data = result,
//                    SystemMessage = Messenger.CreateDataError,
//                    StatusCode = (int)HttpStatusCode.BadRequest
//                };
//            }
//        }

//        //[Authorize(Roles = AppRoles.Admin)]
//        [HttpPut("{id}")]
//        public async Task<ApiResponse<bool>> Put(string id, ProductUpdateModel model)
//        {
//            var result = await _productService.UpdateProductInformation(id, model);

//            if (result)
//            {
//                return new ApiResponse<bool>
//                {
//                    PartnerCode = Messenger.SuccessFull,
//                    RetCode = ERetCode.Successfull,
//                    Data = result,
//                    SystemMessage = Messenger.SuccessFull,
//                    StatusCode = (int)HttpStatusCode.OK
//                };
//            }
//            else
//            {
//                return new ApiResponse<bool>
//                {
//                    PartnerCode = Messenger.UpdateDataError,
//                    RetCode = ERetCode.BadRequest,
//                    Data = result,
//                    SystemMessage = Messenger.UpdateDataError,
//                    StatusCode = (int)HttpStatusCode.BadRequest
//                };
//            }
//        }

//        //[Authorize(Roles = AppRoles.Admin)]
//        [HttpDelete("{id}")]
//        public async Task<ApiResponse<bool>> Delete(string id)
//        {
//            var result = await _productService.DeleteProduct(id);

//            if (result)
//            {
//                return new ApiResponse<bool>
//                {
//                    PartnerCode = Messenger.SuccessFull,
//                    RetCode = ERetCode.Successfull,
//                    Data = result,
//                    SystemMessage = Messenger.SuccessFull,
//                    StatusCode = (int)HttpStatusCode.OK
//                };
//            }
//            else
//            {
//                return new ApiResponse<bool>
//                {
//                    PartnerCode = Messenger.SystemError,
//                    RetCode = ERetCode.SystemError,
//                    Data = result,
//                    SystemMessage = Messenger.SystemError,
//                    StatusCode = (int)HttpStatusCode.BadRequest
//                };
//            }
//        }

//        //[Authorize(Roles = AppRoles.Admin)]
//        [HttpPut("update-count/{id}")]
//        public async Task<ApiResponse<bool>> UpdateProductCount(string id, ProductCountUpdateModel model)
//        {
//            var serviceResult = await _productService.UpdateProductCount(id, model);

//            if (serviceResult.IsSuccess)
//            {
//                return new ApiResponse<bool>
//                {
//                    PartnerCode = Messenger.SuccessFull,
//                    RetCode = ERetCode.Successfull,
//                    Data = serviceResult.Data,
//                    SystemMessage = Messenger.SuccessFull,
//                    StatusCode = (int)HttpStatusCode.OK
//                };
//            }
//            else
//            {
//                return new ApiResponse<bool>
//                {
//                    PartnerCode = Messenger.UpdateDataError,
//                    RetCode = ERetCode.BadRequest,
//                    Data = serviceResult.Data,
//                    SystemMessage = serviceResult.Message,
//                    StatusCode = (int)HttpStatusCode.BadRequest
//                };
//            }
//        }
//    }
//}
