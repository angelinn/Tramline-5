using TramlineFive.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TramlineFive.DataAccess;

namespace TramlineFive.DataAccess.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class, BaseEntity
    {
        public GenericRepository(TramlineFiveContext context)
        {
            this.context = context;
        }

        public IQueryable<T> All()
        {
            return context.Set<T>().AsQueryable();
        }

        public void Add(T entity)
        {
            context.Set<T>().Add(entity);
        }

        public void Delete(int id)
        {
            T entity = FindById(id);
            context.Set<T>().Remove(entity);
        }

        public T FindById(int id)
        {
            return context.Set<T>().FirstOrDefault(i => i.GetId() == id);
        }

        public void Save()
        {
            context.SaveChanges();
        }

        public void Update(T entity)
        {
            context.Entry(entity).State = EntityState.Modified;
        }

        public IQueryable<T> Where(Expression<Func<T, bool>> filter)
        {
            return context.Set<T>().Where(filter);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private bool disposed = false;
        private TramlineFiveContext context;
    }
}
