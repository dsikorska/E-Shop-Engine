using System.Collections.Generic;
using System.Linq;
using E_Shop_Engine.Domain.DomainModel;
using E_Shop_Engine.Domain.Interfaces;

namespace E_Shop_Engine.Services.Repositories
{
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        public CategoryRepository(IAppDbContext context) : base(context)
        {
            _dbSet = _context.Categories;
        }

        public IEnumerable<Category> GetCategoriesByName(string searchTerm)
        {
            return _dbSet.Where(x => x.Name.Contains(searchTerm)).Select(x => x).ToList();
        }
    }
}
