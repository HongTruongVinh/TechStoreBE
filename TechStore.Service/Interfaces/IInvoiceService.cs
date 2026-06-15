

using TechStore.Common.Models;
using TechStore.Model.DTOs.Invoice;

namespace TechStore.Service.Interfaces
{
    public interface IInvoiceService
    {
        Task<ServiceResult<ListItemInvoiceModel>> GetItemById(string id);
        Task<ServiceResult<List<ListItemInvoiceModel>>> GetAllItems();
    }
}
