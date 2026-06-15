using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TechStore.Common.Models;
using TechStore.Data.Context;
using TechStore.Data.Entities;
using TechStore.Data.Repositories.Interfaces;

namespace TechStore.Data.Repositories.Implementations
{
    public class OrderRepository : Repository<Order>, IOrderRepository
    {
        public OrderRepository(AppDbContext context) : base(context) { }

        public async Task<PagedResult<Order>> SearchAsync(OrderSearchQuery query)
        {
            IQueryable<Order> orders = _dbSet;

            // Filter User
            if (!string.IsNullOrWhiteSpace(query.UserId))
            {
                orders = orders.Where(o => o.CustomerPublicId == query.UserId);
            }

            // Filter Status
            if (query.OrderStatus != null)
            {
                orders = orders.Where(o => o.OrderStatus == query.OrderStatus);
            }

            // Filter Date
            if (query.FromDate.HasValue)
            {
                orders = orders.Where(o => o.CreatedAt >= query.FromDate.Value);
            }

            if (query.ToDate.HasValue)
            {
                orders = orders.Where(o => o.CreatedAt <= query.ToDate.Value);
            }

            // Search
            if (!string.IsNullOrWhiteSpace(query.Keyword))
            {
                orders = orders.Where(o =>
                    o.PublicId.Contains(query.Keyword));
            }

            // Sort
            orders = query.SortBy?.ToLower() switch
            {
                "totalprice" => query.Descending
                    ? orders.OrderByDescending(o => o.TotalPrice)
                    : orders.OrderBy(o => o.TotalPrice),

                "createdat" => query.Descending
                    ? orders.OrderByDescending(o => o.CreatedAt)
                    : orders.OrderBy(o => o.CreatedAt),

                _ => orders.OrderByDescending(o => o.CreatedAt)
            };

            var totalItems = await orders.CountAsync();

            var items = await orders
                .Skip((query.Page - 1) * query.PageSize)
                .Take(query.PageSize)
                .ToListAsync();

            return new PagedResult<Order>
            {
                CurrentPage = query.Page,
                PageSize = query.PageSize,
                TotalItems = totalItems,
                Items = items
            };
        }

        public async Task<List<Order>> GetOrdersIncludeItemsAsync(Expression<Func<Order, bool>> predicate)
            => await _dbSet.Where(predicate).Include(o => o.OrderItems).ToListAsync();

        public async Task<List<Order>> GetOrdersIncludeItemsDetailAsync(Expression<Func<Order, bool>> predicate, int page, int pageSize)
        {
            var orders = await _dbSet.AsNoTracking()
                                .Where(predicate)
                                .Include(o => o.OrderItems)
                                    .ThenInclude(oi => oi.ProductVariantOption)
                                        .ThenInclude(pvo => pvo.ProductVariant)
                                            .ThenInclude(pv => pv.Product)
                                                .ThenInclude(p => p.Category)
                                .Include(o => o.OrderItems)
                                    .ThenInclude(oi => oi.ProductVariantOption)
                                        .ThenInclude(pvo => pvo.ProductVariant)
                                            .ThenInclude(pv => pv.Product)
                                                .ThenInclude(p => p.Brand)
                                .OrderByDescending(o => o.CreatedAt)
                                .Skip((page - 1) * pageSize)
                                .Take(pageSize)
                                .ToListAsync();

            return orders;
        }

        public async Task<Order?> GetOrderIncludeItemsAsync(Expression<Func<Order, bool>> predicate)
        {
            var order = await _dbSet
                                .Where(predicate)
                                .Include(o => o.OrderItems)
                                    .ThenInclude(oi => oi.ProductVariantOption)
                                        .ThenInclude(pvo => pvo.ProductVariant)
                                            .ThenInclude(pv => pv.Product)
                                                .ThenInclude(p => p.Category)
                                .Include(o => o.Invoice)
                                .FirstOrDefaultAsync();
            return order;
        }

        public async Task<Order?> GetWithItemsAsync(string publicId)
        {
            var order = await _dbSet
                                .Where(o => o.PublicId == publicId)
                                .Include(o => o.OrderItems)
                                .FirstOrDefaultAsync();
            return order;
        }

        public async Task<Order?> GetWithInvoiceAsync(string publicId)
        {
            var order = await _dbSet
                                .Where(o => o.PublicId == publicId)
                                .Include(o => o.Invoice)
                                    .ThenInclude(i => i.Payments)
                                .FirstOrDefaultAsync();
            return order;
        }

        public async Task<Order?> GetWithItemsAndInvoiceAsync(string publicId)
        {
            var order = await _dbSet
                                .Where(o => o.PublicId == publicId)
                                .Include(o => o.OrderItems)
                                .Include(o => o.Invoice)
                                    .ThenInclude(i => i.Payments)
                                .FirstOrDefaultAsync();
            return order;
        }
    }
}
