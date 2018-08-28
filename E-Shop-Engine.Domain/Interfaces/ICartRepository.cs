using E_Shop_Engine.Domain.DomainModel;

namespace E_Shop_Engine.Domain.Interfaces
{
    public interface ICartRepository : IRepository<Cart>
    {
        void AddItem(Cart cart, Product product, int quantity);
        void RemoveLine(Cart cart, Product product);
        decimal ComputeTotalValue(Cart cart);
        void Clear(Cart cart);
    }
}
