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

        public async Task<List<Order>> GetOrdersIncludeItemsDetailAsync(Expression<Func<Order, bool>> predicate)
        {
            var orders = await _dbSet
                                .Where(predicate)
                                .Include(o => o.OrderItems)
                                    .ThenInclude(oi => oi.Product)
                                        .ThenInclude(p => p.Category)
                                .Include(o => o.OrderItems)
                                    .ThenInclude(oi => oi.Product)
                                        .ThenInclude(p => p.Brand)
                                .ToListAsync();

            return orders;
        }

        public async Task<Order?> GetOrderIncludeItemsAsync(Expression<Func<Order, bool>> predicate)
        {
            var order = await _dbSet
                                .Where(predicate)
                                .Include(o => o.OrderItems)
                                    .ThenInclude(oi => oi.Product)
                                        .ThenInclude(p => p.Category)
                                .Include(o => o.Payment)
                                .FirstOrDefaultAsync();
            return order;
        }
    }
}
