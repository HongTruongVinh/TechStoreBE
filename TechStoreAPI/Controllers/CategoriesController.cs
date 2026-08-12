using Microsoft.AspNetCore.Mvc;
using TechStore.Common.Models;
using TechStore.Model.DTOs.Category;
using TechStore.Service.Interfaces;
using TechStoreAPI.Extensions;

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
        public async Task<ActionResult<ApiResponse<List<CategoryResponseModel>>>> GetAllCategories()
        {
            var serviceResult = await _categoryService.GetAllCategories();

            return serviceResult.ToActionResult(this);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<CategoryResponseModel>>> GetCategoryById(string id)
        {
            var serviceResult = await _categoryService.GetCategoryById(id);

            return serviceResult.ToActionResult(this);
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse<string>>> AddCategory(CategoryCreateModel categoryCreateModel)
        {
            var serviceResult = await _categoryService.AddCategory(categoryCreateModel);

            return serviceResult.ToActionResult(this);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse<bool>>> UpdateCategory(string id, CategoryUpdateModel categoryUpdateModel)
        {
            var serviceResult = await _categoryService.UpdateCategory(id, categoryUpdateModel);

            return serviceResult.ToActionResult(this);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse<bool>>> DeleteCategory(string id)
        {
            var serviceResult = await _categoryService.DeleteCategory(id);

            return serviceResult.ToActionResult(this);
        }
    }
}
