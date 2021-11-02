using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ChatApp.DAL.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected readonly DbSet<T> _dbSet;
        protected readonly DbContext _dbContext;

        public Repository(DbSet<T> dbSet, DbContext DbContext)
        {
            _dbSet = dbSet;
            _dbContext = DbContext;
        }

        public async Task AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
        }

        public async Task UpdateAsync(T entity)
        {
            _dbSet.Update(entity);
        }

        public async Task<T> GetAsync(Expression<Func<T, bool>> where)
        {
            return await _dbSet.FirstOrDefaultAsync(@where);
        }

        public void Remove(T entity)
        {
            _dbSet.Remove(entity);
        }


        public async Task<IQueryable<T>> GetAllAsync(Expression<Func<T, bool>> predicate = null)
        {
            var query = _dbSet.AsQueryable();
            if (predicate != null)
            {
                query = query.Where(predicate);
            }

            return query;
        }

        public async Task<T> GetIncludingAll(Expression<Func<T, bool>> where)
        {
            var query = _dbSet.AsQueryable();
            query = _dbContext.Model.FindEntityType(typeof(T)).GetNavigations().Aggregate(query, (current, property) => current.Include(property.Name));
            return query.FirstOrDefault(where);
        }
    }
}
