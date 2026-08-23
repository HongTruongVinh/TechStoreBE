using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechStore.Data.Entities;

namespace TechStore.Data.Repositories.Interfaces
{
    public interface IProductVariantOptionRepository : IRepository<ProductVariantOption>
    {
        Task<ProductVariantOption?> GetProductVariantOptionDetailByPublicIdAsync(string publicId);
        Task<ProductVariantOption?> GetOrderItemDetailAsync(string publicId);
        Task<ProductVariantOption?> GetProductVariantOptionDetailByInternalIdAsync(Guid publicId);
        Task<ProductVariantOption?> GetByIdAsync(Guid id, CancellationToken cancellationToken);

        /// <summary>
        /// Function for postgreSQL
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ProductVariantOption?> GetForUpdateAsync_PostgreSQL(string publicId);

        /// <summary>
        /// Function for SQLServer
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ProductVariantOption?> GetForUpdateAsync_SQLServer(Guid id);
    }
}
