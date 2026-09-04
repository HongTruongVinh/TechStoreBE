using Microsoft.AspNetCore.Mvc;
using TechStore.Model.DTOs.Home;
using TechStore.Model.DTOs.Product;
using TechStore.Common.Models;
using TechStore.Service.Interfaces;
using TechStoreAPI.Extensions;

namespace TechStoreAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        private readonly IHomeService _homeService;

        public HomeController(IHomeService homeService)
        {
            _homeService = homeService;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<HomeResponseModel>>> GetData()
        {
            var serviceResult = await _homeService.GetHomeProduct();

            return serviceResult.ToActionResult(this);
        }

        [HttpGet("feature")]
        public async Task<ActionResult<ApiResponse<List<ListItemProductModel>>>> GetFeaturedProducts()
        {
            var serviceResult = await _homeService.GetFeaturedProducts();

            return serviceResult.ToActionResult(this);
        }

        [HttpGet("products")]
        public async Task<ActionResult<ApiResponse<List<ListItemProductModel>>>> GetProductsByBrandName(string brandName, int page = 1, int pageSize = 16)
        {
            var serviceResult = await _homeService.GetProductsByBrandName(brandName, page, pageSize);

            return serviceResult.ToActionResult(this);
        }

        [HttpGet("system-configs")]
        public async Task<ActionResult<ApiResponse<SystemConfigsModel>>> GetSystemConfigs()
        {
            var serviceResult = await _homeService.GetSystemConfigs();

            return serviceResult.ToActionResult(this);
        }
    }
}
