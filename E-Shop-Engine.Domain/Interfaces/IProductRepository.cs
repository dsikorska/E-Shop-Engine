using System.Collections.Generic;
using E_Shop_Engine.Domain.DomainModel;

namespace E_Shop_Engine.Domain.Interfaces
{
    public interface IProductRepository : IRepository<Product>
    {
        IEnumerable<Product> GetAllSpecialOffers();
        IEnumerable<Product> GetProductsByCategory(int id);

    }
}
