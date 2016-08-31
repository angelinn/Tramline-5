using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace TramlineFive.DataAccess.Repositories
{
    public interface IGenericRepository<T> : IDisposable
    {
        IQueryable<T> All();
        IQueryable<T> Where(Expression<Func<T, bool>> filter);
        T FindById(int id);
        void Clear();

        void Add(T entity);
        void Delete(int id);
        void Update(T entity);
        void Save();
    }
}
