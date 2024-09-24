using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MeesageService.Data.Interfaces
{
    public interface IRepository<T> where T : class
    {
        Task AddSync(T obj);
        Task<T> GetByIdAsync(long id);
        Task<T> GetOne(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes);
        IQueryable<T> Entity();
        Task RemoveAsync(long id);

    }
}
