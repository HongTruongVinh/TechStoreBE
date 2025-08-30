//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;
//using System.Net;
//using System.Security.Claims;
//using TechStoreModel.Base;
//using TechStoreModel.Constants;
//using TechStoreModel.Enums;
//using TechStoreModel.Model.Cart;
//using TechStoreModel.Model.Comment;
//using TechStoreServices.IServices;
//using TechStoreServices.Services;

//namespace TechStoreAPI.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class CommentsController : ControllerBase
//    {
//        private readonly ICommentService _commentService;

//        public CommentsController (ICommentService commentService)
//        {
//            _commentService = commentService;
//        }

//        [HttpGet("product/{productId}")]
//        public async Task<ApiResponse<List<CommentResponseModel>>> GetProductComment(string productId)
//        {
//            var serviceResult = await _commentService.GetProductComments(productId);

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

//        [HttpPost]
//        public async Task<ApiResponse<string>> AddProductComment(CommentCreateModel model)
//        {
//            var userId = User.FindFirstValue(AppClaims.UserId);

//            if (userId != null)
//            {
//                var serviceResult = await _commentService.AddProductComment(userId, model);

//                if (serviceResult.IsSuccess)
//                {
//                    return new()
//                    {
//                        PartnerCode = Messenger.SuccessFull,
//                        RetCode = ERetCode.Successfull,
//                        Data = serviceResult.Data,
//                        SystemMessage = serviceResult.Message,
//                        StatusCode = (int)HttpStatusCode.OK
//                    };
//                }
//                else
//                {
//                    return new()
//                    {
//                        PartnerCode = Messenger.NoExitData,
//                        RetCode = ERetCode.NoExitData,
//                        Data = serviceResult.Data,
//                        SystemMessage = serviceResult.Message,
//                        StatusCode = (int)HttpStatusCode.OK
//                    };
//                }
//            }
//            else
//            {
//                return new()
//                {
//                    PartnerCode = Messenger.SuccessFull,
//                    RetCode = ERetCode.Successfull,
//                    Data = null,
//                    SystemMessage = Messenger.LoginError,
//                    StatusCode = (int)HttpStatusCode.ExpectationFailed
//                };
//            }
//        }

//        [HttpPut("{id}")]
//        public async Task<ApiResponse<bool>> UpdateProductComment(string id, CommentUpdateModel model)
//        {
//            var userId = User.FindFirstValue(AppClaims.UserId);

//            if (userId != null)
//            {
//                var serviceResult = await _commentService.UpdateProductComment(userId, id, model);

//                if (serviceResult.IsSuccess)
//                {
//                    return new()
//                    {
//                        PartnerCode = Messenger.SuccessFull,
//                        RetCode = ERetCode.Successfull,
//                        Data = serviceResult.Data,
//                        SystemMessage = serviceResult.Message,
//                        StatusCode = (int)HttpStatusCode.OK
//                    };
//                }
//                else
//                {
//                    return new()
//                    {
//                        PartnerCode = Messenger.NoExitData,
//                        RetCode = ERetCode.NoExitData,
//                        Data = serviceResult.Data,
//                        SystemMessage = serviceResult.Message,
//                        StatusCode = (int)HttpStatusCode.OK
//                    };
//                }
//            }
//            else
//            {
//                return new()
//                {
//                    PartnerCode = Messenger.SuccessFull,
//                    RetCode = ERetCode.Successfull,
//                    Data = false,
//                    SystemMessage = Messenger.LoginError,
//                    StatusCode = (int)HttpStatusCode.ExpectationFailed
//                };
//            }
//        }

//        [HttpDelete("{id}")]
//        public async Task<ApiResponse<bool>> DeleteProductComment(string id)
//        {
//            var userId = User.FindFirstValue(AppClaims.UserId);

//            if (userId != null)
//            {
//                var serviceResult = await _commentService.DeleteProductComment(id);

//                if (serviceResult.IsSuccess)
//                {
//                    return new()
//                    {
//                        PartnerCode = Messenger.SuccessFull,
//                        RetCode = ERetCode.Successfull,
//                        Data = serviceResult.Data,
//                        SystemMessage = serviceResult.Message,
//                        StatusCode = (int)HttpStatusCode.OK
//                    };
//                }
//                else
//                {
//                    return new()
//                    {
//                        PartnerCode = Messenger.NoExitData,
//                        RetCode = ERetCode.NoExitData,
//                        Data = serviceResult.Data,
//                        SystemMessage = serviceResult.Message,
//                        StatusCode = (int)HttpStatusCode.OK
//                    };
//                }
//            }
//            else
//            {
//                return new()
//                {
//                    PartnerCode = Messenger.SuccessFull,
//                    RetCode = ERetCode.Successfull,
//                    Data = false,
//                    SystemMessage = Messenger.LoginError,
//                    StatusCode = (int)HttpStatusCode.ExpectationFailed
//                };
//            }
//        }
//    }
//}
