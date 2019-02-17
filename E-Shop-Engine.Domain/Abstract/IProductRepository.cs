using System.Collections.Generic;
using E_Shop_Engine.Domain.DomainModel;

namespace E_Shop_Engine.Domain.Abstract
{
    public interface IProductRepository : IRepository<Product>
    {
        /// <summary>
        /// Get all products that are showing at main page at the first place.
        /// </summary>
        /// <returns>Products that are showing at main page at the first plac</returns>
        IEnumerable<Product> GetAllSpecialOffers();

        /// <summary>
        /// Get all products that are showing at main page below.
        /// </summary>
        /// <returns>Products that are showing at main page below.</returns>
        IEnumerable<Product> GetAllShowingInDeck();

        /// <summary>
        /// Get all products that belong to specified category id.
        /// </summary>
        /// <param name="id">Category id.</param>
        /// <returns>Products that belong to specified category id.</returns>
        IEnumerable<Product> GetProductsByCategory(int id);

        /// <summary>
        /// Get all products that name contains search term.
        /// </summary>
        /// <param name="name">Search term.</param>
        /// <returns>Products that name contains search term.</returns>
        IEnumerable<Product> GetProductsByName(string name);

        /// <summary>
        /// Get all products that catalog number contains search term.
        /// </summary>
        /// <param name="name">Search term.</param>
        /// <returns>Products that catalog number contains search term.</returns>
        IEnumerable<Product> GetProductsByCatalogNumber(string catalogNumber);
    }
}
