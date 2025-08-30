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
        Task<T?> GetByInternalIdAsync(Guid id);
        Task<T?> GetByIdAsync(string publicId);
        Task<List<T>> GetAllAsync();
        Task<List<T>> FindManyAsync(Expression<Func<T, bool>> predicate);
        Task<T?> FindOneAsync(Expression<Func<T, bool>> predicate);
        T? FindTracked(Func<T, bool> predicate);

        Task AddAsync(T entity);
        Task AddRangeAsync(IEnumerable<T> entities);
        void Update(T entity);
        void Remove(T entity);
        void RemoveRange(IEnumerable<T> entities);

        Task DeleteAllAsync();

        Task<int> CountAsync();
        Task<int> CountAsync(Expression<Func<T, bool>> predicate);
    }
}
