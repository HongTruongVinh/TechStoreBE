using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TechStore.Common.Constants;
using TechStore.Common.Models;
using TechStore.Model.DTOs.Shipper;
using TechStore.Service.Interfaces;
using TechStoreAPI.Extensions;

namespace TechStoreAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShippersController : ControllerBase
    {
        private readonly IShipperService _shipperService;

        public ShippersController(IShipperService shipperService)
        {
            _shipperService = shipperService;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<List<ShipperResponseModel>>>> GetAllShippers()
        {
            var serviceResult = await _shipperService.GetAllShippers();

            return serviceResult.ToActionResult(this);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<ShipperResponseModel>>> GetShipperById(string id)
        {
            var serviceResult = await _shipperService.GetShipperById(id);

            return serviceResult.ToActionResult(this);
        }

        [HttpPost]
        [Authorize(Roles = AppRoles.Admin)]
        public async Task<ActionResult<ApiResponse<string>>> AddShipper(ShipperCreateModel model)
        {
            var serviceResult = await _shipperService.AddShipper(model);

            return serviceResult.ToActionResult(this);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = AppRoles.Admin)]
        public async Task<ActionResult<ApiResponse<bool>>> UpdateShipper(string id, ShipperUpdateModel model)
        {
            var serviceResult = await _shipperService.UpdateShipper(id, model);

            return serviceResult.ToActionResult(this);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = AppRoles.Admin)]
        public async Task<ActionResult<ApiResponse<bool>>> DeleteShipper(string id)
        {
            var serviceResult = await _shipperService.DeleteShipper(id);

            return serviceResult.ToActionResult(this);
        }

        [HttpGet("status/{statusId}")]
        public async Task<ActionResult<ApiResponse<List<ShipperResponseModel>>>> GetShipperByStatus(bool statusId)
        {
            var serviceResult = await _shipperService.GetShippersByStatus(statusId);

            return serviceResult.ToActionResult(this);
        }
    }
}
