using Microsoft.AspNetCore.Mvc;
using TechStore.Model.DTOs.Invoice;
using TechStore.Common.Models;
using TechStore.Service.Interfaces;
using TechStoreAPI.Extensions;

namespace TechStoreAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InvoicesController : ControllerBase
    {
        private readonly IInvoiceService _invoiceService;

        public InvoicesController(IInvoiceService invoiceService)
        {
            _invoiceService = invoiceService;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<List<ListItemInvoiceModel>>>> GetItems()
        {
            var serviceResult = await _invoiceService.GetAllItems();

            return serviceResult.ToActionResult(this);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<ListItemInvoiceModel>>> GetItem(string id)
        {
            var serviceResult = await _invoiceService.GetItemById(id);

            return serviceResult.ToActionResult(this);
        }
    }
}
