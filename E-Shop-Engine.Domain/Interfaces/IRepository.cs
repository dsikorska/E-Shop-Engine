using System.Linq;

namespace E_Shop_Engine.Domain.Interfaces
{
    public interface IRepository<T> where T : class
    {
        /// <summary>
        /// Get all entities from table.
        /// </summary>
        /// <returns>Entities from table.</returns>
        IQueryable<T> GetAll();

        /// <summary>
        /// Get entity that id matches.
        /// </summary>
        /// <param name="id">Search by this id.</param>
        /// <returns>Entity that id matches searching.</returns>
        T GetById(int id);

        /// <summary>
        /// Create new entity.
        /// </summary>
        /// <param name="entity">New entity.</param>
        void Create(T entity);

        /// <summary>
        /// Update specified entity.
        /// </summary>
        /// <param name="entity">Entity.</param>
        void Update(T entity);

        /// <summary>
        /// Delete entity that id matches.
        /// </summary>
        /// <param name="id">Search by this id.</param>
        void Delete(int id);

        /// <summary>
        /// Save changes at database.
        /// </summary>
        void Save();
    }
}
