using System.Linq;
using E_Shop_Engine.Domain.DomainModel;

namespace E_Shop_Engine.Domain.Interfaces
{
    public interface IService<T> where T : DbEntity
    {
        IQueryable<T> GetAll();
        T GetById(int id);
        void Create(T entity);
        void Update(T entity);
        void Delete(int id);
    }
}
