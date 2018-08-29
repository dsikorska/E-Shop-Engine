using System;
using System.Collections.Generic;
using System.Data.Entity;
using E_Shop_Engine.Domain.DomainModel;
using E_Shop_Engine.Domain.Interfaces;
using E_Shop_Engine.Services.Data;

namespace E_Shop_Engine.Services.Repositories
{
    public class Repository<T> : IRepository<T> where T : DbEntity
    {
        protected AppDbContext _context;
        protected IDbSet<T> _dbSet;

        public Repository(AppDbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        public virtual void Create(T entity)
        {
            _context.Set<T>().Add(entity);
            Save();
        }

        public virtual void Delete(int id)
        {
            T entity = _dbSet.Find(id);
            _dbSet.Remove(entity);
            Save();
        }

        public virtual IEnumerable<T> GetAll()
        {
            return _dbSet;
        }

        public virtual T GetById(int id)
        {
            return _dbSet.Find(id);
        }

        public virtual void Update(T entity)
        {
            _context.Entry<T>(entity).State = EntityState.Modified;
            Save();
        }

        public virtual void Save()
        {
            _context.SaveChanges();
        }

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
