using E_Shop_Engine.Domain.DomainModel;

namespace E_Shop_Engine.Domain.Interfaces
{
    public interface IOrderRepository : IRepository<Order>
    {
        Order GetByOrderNumber(string orderNumber);
        void OrderPaymentSuccess(Order order, string transactionNumber);
        void OrderPaymentFailed(Order order);
        decimal GetCurrentValue(Order order);
    }
}
