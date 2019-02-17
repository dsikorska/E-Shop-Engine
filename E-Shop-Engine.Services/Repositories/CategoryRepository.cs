using System.Collections.Generic;
using System.Linq;
using E_Shop_Engine.Domain.Abstract;
using E_Shop_Engine.Domain.DomainModel;

namespace E_Shop_Engine.Services.Repositories
{
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        public CategoryRepository(IAppDbContext context) : base(context)
        {
            _dbSet = _context.Categories;
        }

        /// <summary>
        /// Get all categories that name contains search term.
        /// </summary>
        /// <param name="searchTerm">The method will search by this parameter.</param>
        /// <returns>All categories that name contains search term.</returns>
        public IEnumerable<Category> GetCategoriesByName(string searchTerm)
        {
            return _dbSet.Where(x => x.Name.Contains(searchTerm)).Select(x => x).ToList();
        }
    }
}
