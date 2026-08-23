using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechStore.Data.Context;
using TechStore.Data.Entities;
using TechStore.Data.Repositories.Interfaces;

namespace TechStore.Data.Repositories.Implementations
{
    public class ProductVariantRepository : Repository<ProductVariant>, IProductVariantRepository
    {
        public ProductVariantRepository(AppDbContext context) : base(context) { }

        public async Task<ProductVariant?> GetProductVariantOptionDetailAsync(string publicId)
        {
            return await _dbSet
                .Include(p => p.Product)
                .ThenInclude(p => p.Category)
                .FirstOrDefaultAsync(p => p.PublicId == publicId);
        }
    }
}
