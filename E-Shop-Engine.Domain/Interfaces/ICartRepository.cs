using E_Shop_Engine.Domain.DomainModel;
using E_Shop_Engine.Domain.DomainModel.IdentityModel;

namespace E_Shop_Engine.Domain.Interfaces
{
    public interface ICartRepository : IRepository<Cart>
    {
        void AddItem(Cart cart, Product product, int quantity);
        void RemoveItem(Cart cart, Product product, int quantity);
        void RemoveLine(Cart cart, Product product);
        decimal GetTotalValue(Cart cart);
        int CountItems(Cart cart);
        void NewCart(AppUser user);
        void SetCartOrdered(Cart cart);
        Cart GetCurrentCart(AppUser user);
    }
}
