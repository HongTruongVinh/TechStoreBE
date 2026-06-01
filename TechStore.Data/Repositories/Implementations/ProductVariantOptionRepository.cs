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
    public class ProductVariantOptionRepository : Repository<ProductVariantOption>, IProductVariantOptionRepository
    {
        public ProductVariantOptionRepository(AppDbContext context) : base(context) { }

        public async Task<ProductVariantOption?> GetProductVariantOptionDetailByPublicIdAsync(string publicId)
        {
            return await _dbSet
                .Include(p => p.ProductVariant)
                .ThenInclude(p => p.Product)
                .FirstOrDefaultAsync(p => p.PublicId == publicId);
        }

        public async Task<ProductVariantOption?> GetProductVariantOptionDetailByInternalIdAsync(Guid id)
        {
            return await _dbSet
                .Include(p => p.ProductVariant)
                .ThenInclude(p => p.Product)
                .FirstOrDefaultAsync(p => p.Id == id);
        }
    }
}
