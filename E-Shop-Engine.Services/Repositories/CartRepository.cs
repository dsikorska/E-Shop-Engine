using System.Data.Entity;
using System.Linq;
using E_Shop_Engine.Domain.Abstract;
using E_Shop_Engine.Domain.DomainModel;
using E_Shop_Engine.Domain.DomainModel.IdentityModel;

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

        /// <summary>
        /// Adds specified quantity of product to cart.
        /// </summary>
        /// <param name="cart">Cart</param>
        /// <param name="product">Add this product to cart.</param>
        /// <param name="quantity">Add specified quantity.</param>
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
                    Cart_Id = cart.ID
                };
                cart.CartLines.Add(line);
            }
            else
            {
                line.Quantity += quantity;
            }
            Update(cart);
        }

        /// <summary>
        /// Removes specified quantity of product from cart. 
        /// </summary>
        /// <param name="cart">Cart.</param>
        /// <param name="product">Remove this product from cart.</param>
        /// <param name="quantity">Remove specified quantity.</param>
        public void RemoveItem(Cart cart, Product product, int quantity = 1)
        {
            CartLine line = cart.CartLines
                ?.Where(p => p.Product.ID == product.ID)
                .DefaultIfEmpty()
                .FirstOrDefault();

            if (line != null && line.Quantity > 0)
            {
                line.Quantity -= quantity;
            }

            Update(cart);
        }

        /// <summary>
        /// Removes totally product from cart.
        /// </summary>
        /// <param name="cart">Cart</param>
        /// <param name="product">Remove this product.</param>
        public void RemoveLine(Cart cart, Product product)
        {
            CartLine line = cart.CartLines.Where(l => l.Product.ID == product.ID).FirstOrDefault();
            _cartLines.Remove(line);
        }

        /// <summary>
        /// Count quantity of products in cart.
        /// </summary>
        /// <param name="cart">Cart.</param>
        /// <returns>Quantity of products in cart.</returns>
        public int CountItems(Cart cart)
        {
            return cart.CartLines.Sum(x => x.Quantity);
        }

        /// <summary>
        /// Get total value of cart.
        /// </summary>
        /// <param name="cart">Cart.</param>
        /// <returns>Total value of cart.</returns>
        public decimal GetTotalValue(Cart cart)
        {
            return cart.CartLines.Sum(s => s.Product.Price * s.Quantity);
        }

        /// <summary>
        /// Set new cart instance for AppUser.
        /// </summary>
        /// <param name="user">AppUser.</param>
        public void NewCart(AppUser user)
        {
            user.Carts.Add(new Cart(user));
        }

        /// <summary>
        /// Specified cart is ordered.
        /// </summary>
        /// <param name="cart">Cart.</param>
        public void SetCartOrdered(Cart cart)
        {
            cart.IsOrdered = true;
        }

        /// <summary>
        /// Get current active instance of AppUser's cart.
        /// </summary>
        /// <param name="user">AppUser.</param>
        /// <returns>First active instance of AppUser's cart.</returns>
        public Cart GetCurrentCart(AppUser user)
        {
            return user.Carts.Select(x => x).Where(x => x.IsOrdered == false).FirstOrDefault();
        }
    }
}
