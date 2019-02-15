using System.Collections.Generic;
using System.Linq;
using E_Shop_Engine.Domain.Abstract;
using E_Shop_Engine.Domain.DomainModel;
using E_Shop_Engine.Domain.Enumerables;

namespace E_Shop_Engine.Services.Repositories
{
    public class OrderRepository : Repository<Order>, IOrderRepository
    {
        public OrderRepository(IAppDbContext context) : base(context)
        {
            _dbSet = _context.Orders;
        }

        /// <summary>
        /// Get order exactly by specified order number.
        /// </summary>
        /// <param name="number">Order number.</param>
        /// <returns>Order that matches specified order number.</returns>
        public Order GetByOrderNumber(string number)
        {
            return _dbSet.Where(x => x.OrderNumber == number).FirstOrDefault();
        }

        /// <summary>
        /// Get order exactly by specified transaction number.
        /// </summary>
        /// <param name="number">Order number.</param>
        /// <returns>Order that matches specified transaction number.</returns>
        public Order GetByTransactionNumber(string number)
        {
            return _dbSet.Where(x => x.TransactionNumber == number).FirstOrDefault();
        }

        /// <summary>
        /// Get all orders that order number contains specified number.
        /// </summary>
        /// <param name="number">Order number or part of it.</param>
        /// <returns>All orders that order number contains specified number.</returns>
        public IEnumerable<Order> FindByOrderNumber(string number)
        {
            return _dbSet.Where(x => x.OrderNumber.Contains(number)).Select(x => x);
        }

        /// <summary>
        /// Get all orders that transaction number contains specified number.
        /// </summary>
        /// <param name="number">Transaction number or part of it.</param>
        /// <returns>All orders that transaction number contains specified number.</returns
        public IEnumerable<Order> FindByTransactionNumber(string number)
        {
            return _dbSet.Where(x => x.TransactionNumber.Contains(number)).Select(x => x);
        }

        /// <summary>
        /// Set order status.
        /// </summary>
        /// <param name="order">Order.</param>
        /// <param name="status">Status.</param>
        private void SetOrderStatus(Order order, OrderStatus status)
        {
            order.OrderStatus = status;
        }

        /// <summary>
        /// Set order as paid.
        /// </summary>
        /// <param name="order">Order.</param>
        /// <param name="transactionNumber">Transaction number that matches the order.</param>
        public void OrderPaymentSuccess(Order order, string transactionNumber)
        {
            order.TransactionNumber = transactionNumber;
            SetOrderStatus(order, OrderStatus.Processing);
            order.IsPaid = true;
            Update(order);
        }

        /// <summary>
        /// Set order as pending.
        /// </summary>
        /// <param name="order">Order.</param>
        public void OrderPaymentFailed(Order order)
        {
            SetOrderStatus(order, OrderStatus.Pending);
            Update(order);
        }

        /// <summary>
        /// Get current value of the order.
        /// </summary>
        /// <param name="order">Order.</param>
        /// <returns>Current value of order.</returns>
        public decimal GetCurrentValue(Order order)
        {
            return order.Cart.CartLines.Sum(x => x.Quantity * x.Product.Price);
        }
    }
}
