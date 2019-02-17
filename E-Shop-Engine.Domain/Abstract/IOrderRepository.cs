using System.Collections.Generic;
using E_Shop_Engine.Domain.DomainModel;

namespace E_Shop_Engine.Domain.Abstract
{
    public interface IOrderRepository : IRepository<Order>
    {
        /// <summary>
        /// Get order exactly by specified order number.
        /// </summary>
        /// <param name="number">Order number.</param>
        /// <returns>Order that matches specified order number.</returns>
        Order GetByOrderNumber(string number);

        /// <summary>
        /// Get order exactly by specified transaction number.
        /// </summary>
        /// <param name="number">Order number.</param>
        /// <returns>Order that matches specified transaction number.</returns>
        Order GetByTransactionNumber(string number);

        /// <summary>
        /// Get all orders that order number contains specified number.
        /// </summary>
        /// <param name="number">Order number or part of it.</param>
        /// <returns>All orders that order number contains specified number.</returns>
        IEnumerable<Order> FindByOrderNumber(string number);

        /// <summary>
        /// Get all orders that transaction number contains specified number.
        /// </summary>
        /// <param name="number">Transaction number or part of it.</param>
        /// <returns>All orders that transaction number contains specified number.</returns>
        IEnumerable<Order> FindByTransactionNumber(string number);

        /// <summary>
        /// Set order as paid.
        /// </summary>
        /// <param name="order">Order.</param>
        /// <param name="transactionNumber">Transaction number that matches the order.</param>
        void OrderPaymentSuccess(Order order, string transactionNumber);

        /// <summary>
        /// Set order as pending.
        /// </summary>
        /// <param name="order">Order.</param>
        void OrderPaymentFailed(Order order);

        /// <summary>
        /// Get current value of the order.
        /// </summary>
        /// <param name="order">Order.</param>
        /// <returns>Current value of order.</returns>
        decimal GetCurrentValue(Order order);
    }
}
