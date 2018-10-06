using System.Collections.Generic;
using System.Linq;
using E_Shop_Engine.Domain.DomainModel;

namespace E_Shop_Engine.Domain.Interfaces
{
    public interface IProductRepository : IRepository<Product>
    {
        IQueryable<Product> GetAllSpecialOffers();
        IQueryable<Product> GetAllShowingInDeck();
        IEnumerable<Product> GetProductsByCategory(int id);
        IEnumerable<Product> GetProductsByName(string name);
        IEnumerable<Product> GetProductsByCatalogNumber(string catalogNumber);
    }
}
