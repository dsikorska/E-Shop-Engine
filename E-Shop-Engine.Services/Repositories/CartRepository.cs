using System.Data.Entity;
using System.Linq;
using E_Shop_Engine.Domain.DomainModel;
using E_Shop_Engine.Domain.Interfaces;

namespace E_Shop_Engine.Services.Repositories
{
    public class CartRepository : Repository<Cart>, ICartRepository
    {
        private DbSet<CartLine> _cartLines;

        public CartRepository(IAppDbContext context) : base(context)
        {
            _dbSet = _context.Carts;
            _cartLines = _context.CartLines;
        }

        public void AddItem(Cart cart, Product product, int quantity = 1)
        {
            CartLine line = cart.CartLines
                ?.Where(p => p.Product.ID == product.ID)
                .DefaultIfEmpty()
                .FirstOrDefault();

            if (line == null)
            {
                line = new CartLine
                {
                    Product = product,
                    Quantity = quantity,
                };
                cart.CartLines.Add(line);
            }
            else
            {
                line.Quantity += quantity;
            }
            Update(cart);
        }

        public void RemoveLine(Cart cart, Product product)
        {
            CartLine line = cart.CartLines.Where(l => l.Product.ID == product.ID).FirstOrDefault();
            _cartLines.Remove(line);
            Save();
        }

        public int CountItems(Cart cart)
        {
            return cart.CartLines.Sum(x => x.Quantity);
        }

        public decimal ComputeTotalValue(Cart cart)
        {
            return cart.CartLines.Sum(s => s.Product.Price * s.Quantity);
        }

        public void Clear(Cart cart)
        {
            cart.CartLines.Clear();
        }
    }
}
