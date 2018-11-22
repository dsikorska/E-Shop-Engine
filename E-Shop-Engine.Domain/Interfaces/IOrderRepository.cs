using System.Collections.Generic;
using E_Shop_Engine.Domain.DomainModel;

namespace E_Shop_Engine.Domain.Interfaces
{
    public interface IOrderRepository : IRepository<Order>
    {
        Order GetByOrderNumber(string number);
        IEnumerable<Order> FindByOrderNumber(string number);
        IEnumerable<Order> FindByTransactionNumber(string number);
        void OrderPaymentSuccess(Order order, string transactionNumber);
        void OrderPaymentFailed(Order order);
        decimal GetCurrentValue(Order order);
    }
}
