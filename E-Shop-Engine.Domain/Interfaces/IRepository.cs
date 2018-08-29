using System.Collections.Generic;
using E_Shop_Engine.Domain.DomainModel;

namespace E_Shop_Engine.Domain.Interfaces
{
    public interface IRepository<T> where T : DbEntity
    {
        IEnumerable<T> GetAll();
        T GetById(int id);
        void Create(T entity);
        void Update(T entity);
        void Delete(int id);
        void Save();
    }
}
