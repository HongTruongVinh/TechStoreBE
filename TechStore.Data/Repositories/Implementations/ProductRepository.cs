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

        public async Task<PagedResult<Product>> SearchAsync(ProductSearchQuery query)
        {
            IQueryable<Product> products = _dbSet;

            // Keyword
            if (!string.IsNullOrWhiteSpace(query.Keyword))
            {
                products = products.Where(p =>
                    p.Name.Contains(query.Keyword));
            }

            // Category
            if (!string.IsNullOrWhiteSpace(query.CategoryId))
            {
                products = products.Where(p =>
                    p.CategoryPublicId == query.CategoryId);
            }

            // Brand
            if (!string.IsNullOrWhiteSpace(query.BrandId))
            {
                products = products.Where(p =>
                    p.BrandPublicId == query.BrandId);
            }

            // Min Price
            if (query.MinPrice.HasValue)
            {
                products = products.Where(p =>
                    p.MinPrice >= query.MinPrice.Value);
            }

            // Max Price
            if (query.MaxPrice.HasValue)
            {
                products = products.Where(p =>
                    p.MaxPrice <= query.MaxPrice.Value);
            }

            // Sorting
            products = query.SortBy?.ToLower() switch
            {
                "name" => query.Descending
                    ? products.OrderByDescending(p => p.Name)
                    : products.OrderBy(p => p.Name),

                "price" => query.Descending
                    ? products.OrderByDescending(p => p.MinPrice)
                    : products.OrderBy(p => p.MinPrice),

                "popular" => query.Descending
                    ? products.OrderByDescending(p => p.SoldCount)
                    : products.OrderBy(p => p.SoldCount),

                _ => products.OrderByDescending(p => p.SoldCount)
            };

            var totalItems = await products.CountAsync();

            var items = await products
                .Skip((query.Page - 1) * query.PageSize)
                .Take(query.PageSize)
                .ToListAsync();

            return new PagedResult<Product>
            {
                CurrentPage = query.Page,
                PageSize = query.PageSize,
                TotalItems = totalItems,
                Items = items
            };
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
