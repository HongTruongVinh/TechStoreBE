using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace TechStore.Data.Repositories
{
    public interface IRepository<T> where T : class
    {
        Task<T?> GetByInternalIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<T?> GetByIdAsync(string publicId, CancellationToken cancellationToken = default);
        Task<List<T>> GetAllAsync();
        Task<List<T>> FindManyAsync(Expression<Func<T, bool>> predicate);
        Task<T?> FindOneAsync(Expression<Func<T, bool>> predicate);

        Task AddAsync(T entity, CancellationToken cancellationToken = default);
        Task AddRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default);
        void Update(T entity);
        void Remove(T entity);
        void RemoveRange(IEnumerable<T> entities);

        Task DeleteAllAsync();

        Task<int> CountAsync();
        Task<int> CountAsync(Expression<Func<T, bool>> predicate);

        Task<bool> ExistsByGuidAsync(string key, Guid guidValue);

        Task<List<T>> FindManyWithNumberAsync(Expression<Func<T, bool>> predicate, int pageNumber, int pageSize);

        IQueryable<T> Table { get; }
        IQueryable<T> TableNoTracking { get; }
    }
}
