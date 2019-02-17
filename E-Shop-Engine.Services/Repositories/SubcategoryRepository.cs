using System.Collections.Generic;
using System.Linq;
using E_Shop_Engine.Domain.Abstract;
using E_Shop_Engine.Domain.DomainModel;

namespace E_Shop_Engine.Services.Repositories
{
    public class SubategoryRepository : Repository<Subcategory>, ISubcategoryRepository
    {
        public SubategoryRepository(IAppDbContext context) : base(context)
        {
            _dbSet = _context.Subcategories;
        }

        /// <summary>
        /// Get all subcategories that name contains search term.
        /// </summary>
        /// <param name="searchTerm">Search term.</param>
        /// <returns>Subcategories that name contains search term.</returns>
        public IEnumerable<Subcategory> GetSubcategoriesByName(string searchTerm)
        {
            return _dbSet.Where(x => x.Name.Contains(searchTerm)).Select(x => x).ToList();
        }
    }
}
