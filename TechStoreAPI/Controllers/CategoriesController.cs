using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using TechStore.Common.Constants;
using TechStore.Common.Enums;
using TechStore.Common.Models;
using TechStore.Service.Interfaces;
using TechStore.Model.DTOs.Category;

namespace TechStoreAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        public CategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet]
        public async Task<ApiResponse<IEnumerable<CategoryResponseModel>>> GetAllCategories()
        {
            var serviceResult = await _categoryService.GetAllCategories();

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
        public async Task<ApiResponse<CategoryResponseModel>> GetCategoryById(string id)
        {
            var serviceResult = await _categoryService.GetCategoryById(id);

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
                    StatusCode = (int)HttpStatusCode.BadRequest
                };
            }
        }

        [HttpPost]
        public async Task<ApiResponse<string>> AddCategory(CategoryCreateModel categoryCreateModel)
        {
            var serviceResult = await _categoryService.AddCategory(categoryCreateModel);

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

        [HttpPut("{id}")]
        public async Task<ApiResponse<bool>> UpdateCategory(string id, CategoryUpdateModel categoryUpdateModel)
        {
            var serviceResult = await _categoryService.UpdateCategory(id, categoryUpdateModel);

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

        [HttpDelete("{id}")]
        public async Task<ApiResponse<bool>> DeleteCategory(string id)
        {
            var serviceResult = await _categoryService.DeleteCategory(id);

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
