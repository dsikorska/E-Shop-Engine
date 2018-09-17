using System;
using System.Collections.Generic;
using System.Linq;
using E_Shop_Engine.Domain.DomainModel;
using E_Shop_Engine.Domain.Interfaces;

namespace E_Shop_Engine.Services.Repositories
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        public ProductRepository(IAppDbContext context) : base(context)
        {
            _dbSet = _context.Products;
        }

        public IQueryable<Product> GetAllSpecialOffers()
        {
            return _dbSet.Where(p => p.ShowAsSpecialOffer == true).Select(p => p);
        }

        public IQueryable<Product> GetAllShowingInDeck()
        {
            return _dbSet.Where(p => p.ShowAtMainPage == true).Select(p => p);
        }

        public IEnumerable<Product> GetProductsByCategory(int id)
        {
            return _dbSet.Where(p => p.CategoryID == id).Select(p => p);
        }

        public override void Create(Product entity)
        {
            entity.Created = DateTime.UtcNow;
            base.Create(entity);
        }

        public override void Update(Product entity)
        {
            entity.Edited = DateTime.UtcNow;
            base.Update(entity);
        }
    }
}
