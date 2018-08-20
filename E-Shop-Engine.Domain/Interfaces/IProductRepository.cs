using System.Collections.Generic;
using E_Shop_Engine.Domain.DomainModel;

namespace E_Shop_Engine.Domain.Interfaces
{
    public interface IProductRepository : IRepository<Product>
    {
        IList<Product> GetAllSpecialOffers();
        IList<Product> GetAllShowingInDeck();
        IEnumerable<Product> GetProductsByCategory(int id);

    }
}
