using E_Shop_Engine.Domain.DomainModel;
using E_Shop_Engine.Domain.Models;

namespace E_Shop_Engine.Domain.Abstract
{
    public interface IPaymentService
    {
        /// <summary>
        /// Make validation on the same currency transaction. Compare data sent by external server with data saved at Order instance.
        /// </summary>
        /// <param name="operation_amount">Send by external server operation amount.</param>
        /// <param name="operation_currency">Send by external server operation currency.</param>
        /// <param name="control">Send by external server control sum.</param>
        /// <param name="order">The order instance.</param>
        /// <returns>True if valid. False if no valid.</returns>
        bool ValidateSameCurrencyTransaction(string operation_amount, string operation_currency, string control, Order order);

        /// <summary>
        /// If transaction is made with currency conversion, validate data saved on external server.
        /// </summary>
        /// <param name="order">The order instance.</param>
        /// <param name="data">Data sent by external server.</param>
        /// <returns>True if valid. False if no valid.</returns>
        bool ValidateDataSavedAtExternalServer(Order order, PaymentDetails data);

        /// <summary>
        /// Check if transaction is without conversion.
        /// </summary>
        /// <param name="operation_amount">Transaction amount at external server.</param>
        /// <param name="operation_currency">Transaction currency at external server.</param>
        /// <param name="operation_original_amount">Transaction amount sent to external server.</param>
        /// <param name="operation_original_currency">Transaction currency sent to external server.</param>
        /// <returns>True if the same currency otherwise false.</returns>
        bool IsTransactionSameCurrency(string operation_amount, string operation_currency, string operation_original_amount, string operation_original_currency);

        /// <summary>
        /// Check if payment is successful. Parameters should be send by external server.
        /// </summary>
        /// <param name="id">Transaction id.</param>
        /// <param name="operation_number">Transaction number.</param>
        /// <param name="operation_type">Transaction type.</param>
        /// <param name="operation_status">Transaction status.</param>
        /// <returns>True if transaction success otherwise false.</returns>
        bool IsPaymentCompleted(int id, string operation_number, string operation_type, string operation_status);

        /// <summary>
        /// Get operation details from dot pay server.
        /// </summary>
        /// <param name="operation_number">Transaction number set by dot pay.</param>
        /// <returns>Transaction details.</returns>
        string GetOperationDetails(string operation_number);
    }
}
