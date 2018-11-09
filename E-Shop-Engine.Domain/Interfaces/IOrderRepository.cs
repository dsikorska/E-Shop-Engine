using E_Shop_Engine.Domain.DomainModel;

namespace E_Shop_Engine.Domain.Interfaces
{
    public interface IOrderRepository : IRepository<Order>
    {
        Order GetByOrderNumber(string orderNumber);
    }
}
