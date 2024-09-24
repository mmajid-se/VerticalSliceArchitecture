using MeesageService.Data.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace MeesageService.InfraStructure.Implimentation
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected readonly DbContext Db;
        protected readonly DbSet<T> DbSet;

        public Repository(DbContext context)
        {
            Db = context;
            DbSet = Db.Set<T>();
        }

        public virtual async Task AddAsync(T entity)
        {
            try
            {
                await DbSet.AddAsync(entity).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                string error = ex.Message;
            }
        }

        public virtual async Task AddSync(T obj)
        {
            try
            {
                await DbSet.AddAsync(obj).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                // Consider logging the exception here
                string error = ex.Message;
            }
        }

        public void Dispose()
        {
            Db.Dispose();
            GC.SuppressFinalize(this);
        }

        public virtual IQueryable<T> Entity()
        {
            return DbSet;
        }

        public virtual async Task<T> GetByIdAsync(long id)
        {
            return await DbSet.FindAsync(id).ConfigureAwait(false);
        }

        public async Task<T> GetOne(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> result = Entity();
            if (includes.Any())
            {
                foreach (Expression<Func<T, object>> include in includes)
                {
                    result = result.Include(include);
                }
            }
            return await result.FirstOrDefaultAsync(predicate);
        }

        public virtual async Task<T> GetOneAsync(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = DbSet;
            foreach (var include in includes)
            {
                query = query.Include(include);
            }
            return await query.FirstOrDefaultAsync(predicate).ConfigureAwait(false);
        }

        public virtual async Task RemoveAsync(long id)
        {
            var entity = await DbSet.FindAsync(id).ConfigureAwait(false);
            if (entity != null)
            {
                DbSet.Remove(entity);
            }
        }
    }
}
