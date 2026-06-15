using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TechStore.Common.Models;
using TechStore.Data.Entities;

namespace TechStore.Data.Repositories.Interfaces
{
    public interface IOrderRepository : IRepository<Order>
    {
        Task<PagedResult<Order>> SearchAsync(OrderSearchQuery query);
        Task<List<Order>> GetOrdersIncludeItemsAsync(Expression<Func<Order, bool>> predicate);
        Task<List<Order>> GetOrdersIncludeItemsDetailAsync(Expression<Func<Order, bool>> predicate, int page, int pageSize);
        Task<Order?> GetOrderIncludeItemsAsync(Expression<Func<Order, bool>> predicate);

        Task<Order?> GetWithItemsAsync(string publicId);
        Task<Order?> GetWithInvoiceAsync(string publicId);
        Task<Order?> GetWithItemsAndInvoiceAsync(string publicId);
    }
}
