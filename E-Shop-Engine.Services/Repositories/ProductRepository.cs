using System.Collections.Generic;
using System.Linq;
using E_Shop_Engine.Domain.DomainModel;
using E_Shop_Engine.Domain.Interfaces;
using E_Shop_Engine.Services.Data;

namespace E_Shop_Engine.Services.Repositories
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        public ProductRepository(AppDbContext context) : base(context)
        {
            _context = context;
            _dbSet = context.Products;
        }

        public IEnumerable<Product> GetAllSpecialOffers()
        {
            return _dbSet.Where(p => p.IsSpecialOffer == true).Select(p => p);
        }

        public IEnumerable<Product> GetProductsByCategory(int id)
        {
            return _dbSet.Where(p => p.CategoryID == id).Select(p => p);
        }
    }
}
