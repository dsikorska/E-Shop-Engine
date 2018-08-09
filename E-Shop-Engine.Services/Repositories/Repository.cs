using System.Data.Entity;
using System.Linq;
using E_Shop_Engine.Domain.DomainModel;
using E_Shop_Engine.Domain.Interfaces;
using E_Shop_Engine.Services.Data;

namespace E_Shop_Engine.Services.Repositories
{
    public abstract class Repository<T> : IRepository<T> where T : DbEntity
    {
        protected AppDbContext _context;
        protected readonly IDbSet<T> _dbSet;

        public Repository(AppDbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        public virtual void Create(T entity)
        {
            _context.Set<T>().Add(entity);
        }

        public virtual void Delete(int id)
        {
            T entity = _dbSet.Find(id);
            _dbSet.Remove(entity);
        }

        public virtual IQueryable<T> GetAll()
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
        }

        public virtual void Save()
        {
            _context.SaveChanges();
        }
    }
}
