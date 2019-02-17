using E_Shop_Engine.Domain.DomainModel;
using E_Shop_Engine.Domain.DomainModel.IdentityModel;

namespace E_Shop_Engine.Domain.Abstract
{
    public interface ICartRepository : IRepository<Cart>
    {
        /// <summary>
        /// Adds specified quantity of product to cart.
        /// </summary>
        /// <param name="cart">Cart</param>
        /// <param name="product">Add this product to cart.</param>
        /// <param name="quantity">Add specified quantity.</param>
        void AddItem(Cart cart, Product product, int quantity);

        /// <summary>
        /// Removes specified quantity of product from cart. 
        /// </summary>
        /// <param name="cart">Cart.</param>
        /// <param name="product">Remove this product from cart.</param>
        /// <param name="quantity">Remove specified quantity.</param>
        void RemoveItem(Cart cart, Product product, int quantity);

        /// <summary>
        /// Removes totally product from cart.
        /// </summary>
        /// <param name="cart">Cart</param>
        /// <param name="product">Remove this product.</param>
        void RemoveLine(Cart cart, Product product);

        /// <summary>
        /// Get total value of cart.
        /// </summary>
        /// <param name="cart">Cart.</param>
        /// <returns>Total value of cart.</returns>
        decimal GetTotalValue(Cart cart);

        /// <summary>
        /// Count quantity of products in cart.
        /// </summary>
        /// <param name="cart">Cart.</param>
        /// <returns>Quantity of products in cart.</returns>
        int CountItems(Cart cart);

        /// <summary>
        /// Set new cart instance for AppUser.
        /// </summary>
        /// <param name="user">AppUser.</param>
        void NewCart(AppUser user);

        /// <summary>
        /// Specified cart is ordered.
        /// </summary>
        /// <param name="cart">Cart.</param>
        void SetCartOrdered(Cart cart);

        /// <summary>
        /// Get current active instance of AppUser's cart.
        /// </summary>
        /// <param name="user">AppUser.</param>
        /// <returns>First active instance of AppUser's cart.</returns>
        Cart GetCurrentCart(AppUser user);
    }
}
