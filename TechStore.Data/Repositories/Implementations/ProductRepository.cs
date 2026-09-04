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
using TechStore.Data.Repositories.QueryModels;

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
                //case-insensitive search in SQLExpress
                //products = products.Where(p =>
                //    p.Name.Contains(query.Keyword));

                // Use ILike for case-insensitive search in PostgreSQL -- no need to use ToLower() or ToUpper()
                products = products.Where(p =>
                EF.Functions.ILike(p.Name, $"%{query.Keyword}%"));
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

        public async Task<List<AiProductContext>> SearchForAiAsync(ProductSearchCriteria criteria, CancellationToken cancellationToken = default)
        {
            var query = _context.Products
                .AsNoTracking()
                .Include(x => x.Brand)
                .Include(x => x.Category)
                .Include(x => x.Variants)
                .AsQueryable();

            // Category
            if (!string.IsNullOrWhiteSpace(criteria.Category))
            {
                query = query.Where(x =>
                    x.Category.Name.ToLower()
                        .Contains(criteria.Category.ToLower()));
            }

            // Brand
            if (!string.IsNullOrWhiteSpace(criteria.Brand))
            {
                query = query.Where(x =>
                    x.Brand.Name.ToLower()
                        .Contains(criteria.Brand.ToLower()));
            }

            //// Minimum price
            //if (criteria.MinPrice.HasValue)
            //{
            //    if (criteria.MinPrice.Value > 0)
            //    {
            //        query = query.Where(x =>
            //        x.Variants.Any(v =>
            //            v.Price >= criteria.MinPrice.Value));
            //    }
            //}

            //// Maximum price
            //if (criteria.MaxPrice.HasValue)
            //{
            //    if (criteria.MaxPrice.Value > 0)
            //    {
            //        query = query.Where(x =>
            //        x.Variants.Any(v =>
            //            v.Price <= criteria.MaxPrice.Value));
            //    }
            //}

            // Minimum price
            if (criteria.MinPrice.HasValue)
            {
                if (criteria.MinPrice.Value > 0)
                {
                    query = query.Where(x => x.MinPrice >= criteria.MinPrice.Value);
                }
            }

            // Maximum price
            if (criteria.MaxPrice.HasValue)
            {
                if (criteria.MaxPrice.Value > 0)
                {
                    query = query.Where(x => x.MaxPrice <= criteria.MaxPrice.Value);
                }
            }

            var products = await query
                .Take(20)
                .ToListAsync(cancellationToken);

            return products
                .Select(product =>
                {
                    var variants = product.Variants;

                    var price = variants
                        .Select(x => x.Price)
                        .OrderBy(x => x)
                        .FirstOrDefault();

                    return new AiProductContext
                    {
                        ProductId = product.PublicId,
                        Slug = product.Slug,
                        Name = product.Name,
                        ImgUrl = product.MainImageUrl,
                        Brand = product.Brand.Name,
                        Price = price,
                    };
                })
                .ToList();
        }
    }
}
