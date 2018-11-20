using System.Collections.Generic;
using E_Shop_Engine.Domain.DomainModel;

namespace E_Shop_Engine.Domain.Interfaces
{
    public interface ISubcategoryRepository : IRepository<Subcategory>
    {
        IEnumerable<Subcategory> GetSubcategoriesByName(string searchTerm);
    }
}
