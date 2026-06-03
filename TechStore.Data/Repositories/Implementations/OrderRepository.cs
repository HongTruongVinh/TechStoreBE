using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TechStore.Data.Context;
using TechStore.Data.Entities;
using TechStore.Data.Repositories.Interfaces;

namespace TechStore.Data.Repositories.Implementations
{
    public class OrderRepository : Repository<Order>, IOrderRepository
    {
        public OrderRepository(AppDbContext context) : base(context) { }

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
                                .Include(o => o.Payments)
                                .FirstOrDefaultAsync();
            return order;
        }
    }
}
