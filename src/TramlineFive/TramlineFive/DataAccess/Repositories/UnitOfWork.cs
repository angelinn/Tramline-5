using TramlineFive.DataAccess.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TramlineFive.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

using SettingsEntity = TramlineFive.DataAccess.Entities.Settings;
using TramlineFive.Common.Extensions;

namespace TramlineFive.DataAccess.Repositories
{
    public class UnitOfWork : IDisposable
    {
        public UnitOfWork(TramlineFiveContext context)
        {
            this.context = context;
            this.repositories = new Dictionary<Type, object>();
        }

        public UnitOfWork() : this(new TramlineFiveContext())
        {   }

        public void Migrate()
        {
            try
            {
                context.Database.Migrate();
            }
            catch (InvalidOperationException)
            {

            }
        }

        public void Clear()
        {
            context.Stops.Clear();
            context.Days.Clear();
            context.Directions.Clear();
            context.Lines.Clear();

            context.SaveChanges();
        }

        public IGenericRepository<SettingsEntity> Settings
        {
            get
            {
                return GetRepository<SettingsEntity>();
            }
        }

        public IGenericRepository<Line> Lines
        {
            get
            {
                return GetRepository<Line>();
            }
        }

        public IGenericRepository<Favourite> Favourites
        {
            get
            {
                return GetRepository<Favourite>();
            }
        }

        private IGenericRepository<T> GetRepository<T>() where T : class, BaseEntity
        {
            if (!repositories.ContainsKey(typeof(T)))
            {
                Type type = typeof(GenericRepository<T>);
                repositories.Add(typeof(T), Activator.CreateInstance(type, context));
            }
            return (IGenericRepository<T>)repositories[typeof(T)];
        }

        public void Save()
        {
            context.SaveChanges();
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
        private Dictionary<Type, object> repositories;
    }
}
