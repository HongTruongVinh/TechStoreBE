

using TechStore.Common.Models;
using TechStore.Model.DTOs.Invoice;

namespace TechStore.Service.Interfaces
{
    public interface IInvoiceService
    {
        Task<ServiceResult<InvoiceListItemModel>> GetItemById(string id);
        Task<ServiceResult<List<InvoiceListItemModel>>> GetAllItems();
    }
}
