using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using E_Shop_Engine.Domain.Abstract;
using E_Shop_Engine.Domain.DomainModel;

namespace E_Shop_Engine.Services.Repositories
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        public ProductRepository(IAppDbContext context) : base(context)
        {
            _dbSet = _context.Products;
        }

        /// <summary>
        /// Get all products that are showing at main page at the first place.
        /// </summary>
        /// <returns>Products that are showing at main page at the first plac</returns>
        public IEnumerable<Product> GetAllSpecialOffers()
        {
            return _dbSet.Where(p => p.ShowAsSpecialOffer == true).Select(p => p);
        }

        /// <summary>
        /// Get all products that are showing at main page below.
        /// </summary>
        /// <returns>Products that are showing at main page below.</returns>
        public IEnumerable<Product> GetAllShowingInDeck()
        {
            return _dbSet.Where(p => p.ShowAtMainPage == true).Select(p => p);
        }

        /// <summary>
        /// Get all products that belong to specified category id.
        /// </summary>
        /// <param name="id">Category id.</param>
        /// <returns>Products that belong to specified category id.</returns>
        public IEnumerable<Product> GetProductsByCategory(int id)
        {
            return _dbSet.Where(p => p.CategoryID == id).Select(p => p);
        }

        /// <summary>
        /// Get all products that name contains search term.
        /// </summary>
        /// <param name="name">Search term.</param>
        /// <returns>Products that name contains search term.</returns>
        public IEnumerable<Product> GetProductsByName(string name)
        {
            return _dbSet.Where(p => p.Name.Contains(name)).Select(p => p);
        }

        /// <summary>
        /// Get all products that catalog number contains search term.
        /// </summary>
        /// <param name="name">Search term.</param>
        /// <returns>Products that catalog number contains search term.</returns>
        public IEnumerable<Product> GetProductsByCatalogNumber(string catalogNumber)
        {
            return _dbSet.Where(p => p.CatalogNumber.Contains(catalogNumber)).Select(p => p);
        }

        /// <summary>
        /// Overrided method. Create new product and set it's creation time.
        /// </summary>
        /// <param name="entity">Product.</param>
        public override void Create(Product entity)
        {
            entity.Created = DateTime.UtcNow;
            base.Create(entity);
        }

        /// <summary>
        /// Overrided method. Set product's edited date and time.
        /// </summary>
        /// <param name="entity">Product.</param>
        public override void Update(Product entity)
        {
            entity.Edited = DateTime.UtcNow;
            base.Update(entity);
        }

        /// <summary>
        /// Overrided method. Search product by id then include all child entities and delete.
        /// </summary>
        /// <param name="id">Product id.</param>
        public override void Delete(int id)
        {
            Product entity = _dbSet
                .Include(x => x.CartLines)
                .FirstOrDefault(x => x.ID == id);

            _dbSet.Remove(entity);
        }
    }
}
