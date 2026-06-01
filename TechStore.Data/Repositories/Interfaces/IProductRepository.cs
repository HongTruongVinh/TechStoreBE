using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TechStore.Data.Entities;

namespace TechStore.Data.Repositories.Interfaces
{
    public interface IProductRepository : IRepository<Product>
    {
        Task<List<Product>?> GetProductsFilteredAsync(
            int page,
            int pageSize,
            string? categoryId = null,
            decimal? minPrice = null,
            decimal? maxPrice = null,
            string? brand = null);


        Task<List<Product>?> SearchByNameAsync(string keyword, int pageNumber, int pageSize);
        Task<List<Product>?> GetProductsByCategoryAsync(Guid categoryId, int pageNumber, int pageSize);
        Task<List<Product>?> GetProductsByCategoryAndBrandAsync(Guid categoryId, Guid brandId, int pageNumber, int pageSize);

        Task<List<Product>?> GetProductsAsync(Expression<Func<Product, bool>> predicate, int pageNumber, int pageSize);

        Task<Product?> GetProductWithDetailsByIdAsync(string publicId);
        Task<List<Product>?> GetFeatureProductsWithDetailsAsync(int pageNumber, int pageSize);

        Task<List<Product>?> GetTopNewestProductsAsync(int count);
        Task<List<Product>> GetTopSoldProductsAsync(int count);
        Task<List<Product>> GetTopRatedProductsAsync(int count);
    }
}

