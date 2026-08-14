using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TechStore.Common.Models;
using TechStore.Model.DTOs.Order;
using TechStore.Model.DTOs.Voucher;
using TechStore.Service.Interfaces;
using TechStoreAPI.Extensions;

namespace TechStoreAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VouchersController : ControllerBase
    {
        private readonly IVoucherService _voucherService;

        public VouchersController(IVoucherService voucherService)
        {
            _voucherService = voucherService;
        }

        [HttpPost("{voucherCode}")]
        public async Task<ActionResult<ApiResponse<VoucherResponseModel>>> CheckVoucher(string voucherCode, [FromBody] List<OrderItemCreateModel> products)
        {
            var serviceResult = await _voucherService.CheckVoucherAsync(voucherCode, products);

            var result = serviceResult.ToActionResult(this);
            return result;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<List<VoucherResponseModel>>>> GetVouchers()
        {
            var serviceResult = await _voucherService.GetVouchersAsync(null);

            return serviceResult.ToActionResult(this);
        }
    }
}
