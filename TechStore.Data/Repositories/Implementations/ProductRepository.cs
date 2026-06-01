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
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        public ProductRepository(AppDbContext context) : base(context) { }

        public async Task<List<Product>?> GetProductsAsync(Expression<Func<Product, bool>> predicate, int pageNumber, int pageSize)
        {
            var products =  await _dbSet
                                .Where(predicate)
                                .Include(p => p.Category)
                                .Include(p => p.Brand)
                                .Include(p => p.Variants).ThenInclude(vo => vo.Options)
                                .Skip((pageNumber - 1) * pageSize)
                                .Take(pageSize)
                                .ToListAsync();
            return products;
        }

        public async Task<List<Product>?> GetProductsFilteredAsync(
    int pageNumber,
    int pageSize,
    string? categoryId = null,
    decimal? minPrice = null,
    decimal? maxPrice = null,
    string? brandId = null)
        {
            var query = _dbSet.AsQueryable();

            if (!string.IsNullOrEmpty(categoryId))
            {
                var category = await _context.Categories
                    .FirstOrDefaultAsync(c => c.PublicId == categoryId);

                if (category != null)
                {
                    query = query.Where(p => p.CategoryId == category.Id);
                }
            }

            //if (minPrice.HasValue)
            //    query = query.Where(p => p.Price >= minPrice.Value);

            //if (maxPrice.HasValue)
            //    query = query.Where(p => p.Price <= maxPrice.Value);

            if (!string.IsNullOrEmpty(brandId))
            {
                var brand = await _context.Brands
                    .FirstOrDefaultAsync(c => c.PublicId == brandId);

                if (brand != null)
                {
                    query = query.Where(p => p.BrandId == brand.Id);
                }
            }

            return await query
                .OrderByDescending(p => p.StartSellingDate)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<List<Product>?> GetTopNewestProductsAsync(int count)
        {
            return await _dbSet.OrderByDescending(p => p.StartSellingDate)
                 .Take(count).ToListAsync();
        }

        public async Task<List<Product>?> GetTopProductsAsync(int count)
        {
            return await _dbSet.OrderBy(p => p.StartSellingDate).Take(count).ToListAsync();
        }

        public async Task<List<Product>?> SearchByNameAsync(string keyword, int pageNumber, int pageSize)
        {
            return await _dbSet.Where(p => p.Name.Contains(keyword))
                .OrderByDescending(p => p.StartSellingDate)
                .Include(p => p.Variants)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<List<Product>?> GetProductsByCategoryAsync(Guid categoryId, int pageNumber, int pageSize)
        {
            return await _dbSet.Where(p => p.CategoryId == categoryId)
                .OrderByDescending(p => p.StartSellingDate)
                .Include(p => p.Variants)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<List<Product>?> GetProductsByCategoryAndBrandAsync(Guid categoryId, Guid brandId, int pageNumber, int pageSize)
        {
            return await _dbSet.Where(p => p.CategoryId == categoryId && p.BrandId == brandId)
                .OrderByDescending(p => p.StartSellingDate)
                .Include(p => p.Variants)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<Product?> GetProductWithDetailsByIdAsync(string publicId)
        {
            return await _dbSet
                .Include(p => p.Category)
                .Include(p => p.Brand)
                .Include(p => p.Variants).ThenInclude(vo => vo.Options)
                .FirstOrDefaultAsync(p => p.PublicId == publicId);
        }

        public async Task<List<Product>?> GetFeatureProductsWithDetailsAsync(int pageNumber, int pageSize)
        {
            return await _dbSet
                .Where(p => p.IsFeatured == true)
                .Include(p => p.Category)
                .Include(p => p.Brand)
                .Include(p => p.Variants).ThenInclude(vo => vo.Options)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<List<Product>> GetTopSoldProductsAsync(int count)
        {
            return await _dbSet
                .AsNoTracking()
                .OrderByDescending(p => p.SoldCount)
                .Take(count)
                .ToListAsync();
        }

        public async Task<List<Product>> GetTopRatedProductsAsync(int count)
        {
            return await _dbSet
                .AsNoTracking()
                .OrderByDescending(p => p.AverageRating)
                .Take(count)
                .ToListAsync();
        }
    }
}
