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

        public IList<Product> GetAllSpecialOffers()
        {
            return _dbSet.Where(p => p.ShowAsSpecialOffer == true).Select(p => p).ToList();
        }

        public IList<Product> GetAllShowingInDeck()
        {
            return _dbSet.Where(p => p.ShowAtMainPage == true).Select(p => p).ToList();
        }

        public IEnumerable<Product> GetProductsByCategory(int id)
        {
            return _dbSet.Where(p => p.CategoryID == id).Select(p => p);
        }
    }
}
