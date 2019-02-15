using System.Collections.Generic;
using E_Shop_Engine.Domain.DomainModel;

namespace E_Shop_Engine.Domain.Abstract
{
    public interface ICategoryRepository : IRepository<Category>
    {
        /// <summary>
        /// Get all categories that name contains search term.
        /// </summary>
        /// <param name="searchTerm">The method will search by this parameter.</param>
        /// <returns>All categories that name contains search term.</returns>
        IEnumerable<Category> GetCategoriesByName(string searchTerm);
    }
}
