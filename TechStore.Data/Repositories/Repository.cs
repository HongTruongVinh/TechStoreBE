using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TechStore.Data.Context;

namespace TechStore.Data.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected readonly AppDbContext _context;
        protected readonly DbSet<T> _dbSet;

        public Repository(AppDbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        public async Task<T?> GetByInternalIdAsync(Guid id) => await _dbSet.FindAsync(id);
        public async Task<T?> GetByIdAsync(string publicId)
            => await _dbSet.FirstOrDefaultAsync(e => EF.Property<string>(e, "PublicId") == publicId);

        public async Task<List<T>> GetAllAsync() => await _dbSet.ToListAsync();

        public async Task<List<T>> FindManyAsync(Expression<Func<T, bool>> predicate)
            => await _dbSet.Where(predicate).ToListAsync();

        public async Task<T?> FindOneAsync(Expression<Func<T, bool>> predicate)
            => await _dbSet.FirstOrDefaultAsync(predicate);

        /// <summary>
        /// Finds an entity in the local change tracker that matches the specified predicate.
        /// </summary>
        /// <remarks>This method searches only the local change tracker for entities that have already
        /// been loaded into memory. It does not query the database.</remarks>
        /// <param name="predicate">A function to test each entity for a condition. The function should return <see langword="true"/> for the
        /// entity to be selected.</param>
        /// <returns>The first entity that matches the specified predicate, or <see langword="null"/> if no such entity is found.</returns>
        public T? FindTracked(Func<T, bool> predicate)
        {
            return _dbSet.Local.FirstOrDefault(predicate);
        }

        public async Task AddAsync(T entity) => await _dbSet.AddAsync(entity);

        public async Task AddRangeAsync(IEnumerable<T> entities)
            => await _dbSet.AddRangeAsync(entities);

        public void Update(T entity) => _dbSet.Update(entity);

        public void Remove(T entity) => _dbSet.Remove(entity);

        public async Task DeleteAllAsync()
        {
            var allRecords = await _dbSet.ToListAsync();
            _dbSet.RemoveRange(allRecords);
        }

        public void RemoveRange(IEnumerable<T> entities)
        {
            _dbSet.RemoveRange(entities);
        }

        public async Task<int> CountAsync()
        {
            return await _dbSet.CountAsync();
        }

        public async Task<int> CountAsync(Expression<Func<T, bool>> predicate)
        {
            return await _dbSet.CountAsync(predicate);
        }
    }
}
