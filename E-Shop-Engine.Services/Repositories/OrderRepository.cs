using System.Linq;
using E_Shop_Engine.Domain.DomainModel;
using E_Shop_Engine.Domain.Enumerables;
using E_Shop_Engine.Domain.Interfaces;

namespace E_Shop_Engine.Services.Repositories
{
    public class OrderRepository : Repository<Order>, IOrderRepository
    {
        public OrderRepository(IAppDbContext context) : base(context)
        {
            _dbSet = _context.Orders;
        }

        public Order GetByOrderNumber(string orderNumber)
        {
            return _dbSet.Where(x => x.OrderNumber == orderNumber).FirstOrDefault();
        }

        private void SetOrderStatus(Order order, OrderStatus status)
        {
            order.OrderStatus = status;
        }

        public void OrderPaymentSuccess(Order order, string transactionNumber)
        {
            order.TransactionNumber = transactionNumber;
            SetOrderStatus(order, OrderStatus.Processing);
            order.IsPaid = true;
            Update(order);
        }

        public void OrderPaymentFailed(Order order)
        {
            SetOrderStatus(order, OrderStatus.Pending);
            Update(order);
        }

        public decimal GetCurrentValue(Order order)
        {
            return order.OrderedCart.CartLines.Sum(x => x.Quantity * x.Product.Price);
        }
    }
}
