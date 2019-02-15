using System;
using System.Collections.Generic;
using System.Data.Entity;
using E_Shop_Engine.Domain.Abstract;
using E_Shop_Engine.Services.Data;

namespace E_Shop_Engine.Services.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected AppDbContext _context;
        protected IDbSet<T> _dbSet;

        public Repository(IAppDbContext context)
        {
            _context = context as AppDbContext;
            _dbSet = _context.Set<T>();
        }

        /// <summary>
        /// Create new entity.
        /// </summary>
        /// <param name="entity">New entity.</param>
        public virtual void Create(T entity)
        {
            _context.Set<T>().Add(entity);
        }

        /// <summary>
        /// Delete entity that id matches.
        /// </summary>
        /// <param name="id">Search by this id.</param>
        public virtual void Delete(int id)
        {
            T entity = _dbSet.Find(id);
            _dbSet.Remove(entity);
        }

        /// <summary>
        /// Get all entities from table.
        /// </summary>
        /// <returns>Entities from table.</returns>
        public virtual IEnumerable<T> GetAll()
        {
            return _dbSet;
        }

        /// <summary>
        /// Get entity that id matches.
        /// </summary>
        /// <param name="id">Search by this id.</param>
        /// <returns>Entity that id matches searching.</returns>
        public virtual T GetById(int id)
        {
            return _dbSet.Find(id);
        }

        /// <summary>
        /// Update specified entity.
        /// </summary>
        /// <param name="entity">Entity.</param>
        public virtual void Update(T entity)
        {
            _context.Entry<T>(entity).State = EntityState.Modified;
        }

        /// <summary>
        /// Dispose the object.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_context != null)
                {
                    _context.Dispose();
                    _context = null;
                }
            }
        }
    }
}
