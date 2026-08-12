using Microsoft.AspNetCore.Mvc;
using TechStore.Common.Models;
using TechStore.Model.DTOs.Brand;
using TechStore.Service.Interfaces;
using TechStoreAPI.Extensions;

namespace TechStoreAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BrandsController : ControllerBase
    {
        private readonly IBrandService _brandService;

        public BrandsController(IBrandService brandService)
        {
            _brandService = brandService;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<List<BrandResponseModel>>>> GetBrands()
        {
            var serviceResult = await _brandService.GetAllBrands();

            return serviceResult.ToActionResult(this);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<BrandResponseModel>>> Get(string id)
        {
            var serviceResult = await _brandService.GetBrandById(id);

            return serviceResult.ToActionResult(this);
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse<string>>> Post(BrandCreateModel model)
        {
            var serviceResult = await _brandService.AddBrand(model);

            return serviceResult.ToActionResult(this);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse<bool>>> Put(string id, BrandUpdateModel model)
        {
            var serviceResult = await _brandService.UpdateBrand(id, model);

            return serviceResult.ToActionResult(this);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse<bool>>> Delete(string id)
        {
            var serviceResult = await _brandService.DeleteBrand(id);

            return serviceResult.ToActionResult(this);
        }
    }
}
