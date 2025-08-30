using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechStore.Data.Entities;

namespace TechStore.Data.Repositories.Interfaces
{
    public interface IProductRepository : IRepository<Product>
    {
        Task<List<Product>> GetProductsFilteredAsync(
            int page,
            int pageSize,
            string? categoryId = null,
            decimal? minPrice = null,
            decimal? maxPrice = null,
            string? brand = null);

        Task<List<Product>> GetTopNewestProductsAsync(int count);

        Task<List<Product>> SearchByNameAsync(string keyword, int pageNumber, int pageSize);

        Task<List<Product>> GetProductsAsync(int pageNumber, int pageSize);
    }
}
