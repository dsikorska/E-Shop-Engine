using System.Collections.Generic;
using E_Shop_Engine.Domain.DomainModel;

namespace E_Shop_Engine.Domain.Abstract
{
    public interface ISubcategoryRepository : IRepository<Subcategory>
    {
        /// <summary>
        /// Get all subcategories that name contains search term.
        /// </summary>
        /// <param name="searchTerm">Search term.</param>
        /// <returns>Subcategories that name contains search term.</returns>
        IEnumerable<Subcategory> GetSubcategoriesByName(string searchTerm);
    }
}
