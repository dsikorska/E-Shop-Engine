using System.Linq;
using System.Text.RegularExpressions;
using E_Shop_Engine.Domain.DomainModel;
using E_Shop_Engine.Domain.Interfaces;
using E_Shop_Engine.Domain.Models;

namespace E_Shop_Engine.Services.Services.Payment.DotPay
{
    public class DotPayPaymentService : PaymentService
    {
        private static Settings settings;

        public DotPayPaymentService(ISettingsRepository settingsRepository)
        {
            settings = settingsRepository.Get();
        }

        /// <summary>
        /// Make validation on the same currency transaction. Compare data sent by external server with data saved at Order instance.
        /// </summary>
        /// <param name="operation_amount">Send by external server operation amount.</param>
        /// <param name="operation_currency">Send by external server operation currency.</param>
        /// <param name="control">Send by external server control sum.</param>
        /// <param name="order">The order instance.</param>
        /// <returns>True if valid. False if no valid.</returns>
        public override bool ValidateSameCurrencyTransaction(string transactionValue, string transactionCurrency, string control, Order order)
        {
            return order.OrderNumber == control &&
                    order.Cart.CartLines.Sum(x => x.Product.Price * x.Quantity).ToString() == transactionValue &&
                    settings.Currency == transactionCurrency;
        }

        /// <summary>
        /// If transaction is made with currency conversion, validate data saved on external server.
        /// </summary>
        /// <param name="order">The order instance.</param>
        /// <param name="data">Data sent by external server.</param>
        /// <returns>True if valid. False if no valid.</returns>
        public override bool ValidateDataSavedAtExternalServer(Order order, PaymentDetails externalData)
        {
            return externalData.Control == order.OrderNumber &&
                    externalData.OriginalAmount == order.Cart.CartLines.Sum(x => x.Product.Price * x.Quantity) &&
                    externalData.OriginalCurrency == settings.Currency &&
                    externalData.OperationStatus == "completed" &&
                    externalData.OperationType == "payment";
        }

        /// <summary>
        /// Check if transaction is without conversion.
        /// </summary>
        /// <param name="operation_amount">Transaction amount at external server.</param>
        /// <param name="operation_currency">Transaction currency at external server.</param>
        /// <param name="operation_original_amount">Transaction amount sent to external server.</param>
        /// <param name="operation_original_currency">Transaction currency sent to external server.</param>
        /// <returns>True if the same currency otherwise false.</returns>
        public override bool IsTransactionSameCurrency(string transactionValue, string transactionCurrency, string transactionOriginalValue, string transactionOriginalCurrency)
        {
            return transactionValue == transactionOriginalValue && transactionCurrency == transactionOriginalCurrency;
        }

        /// <summary>
        /// Check if payment is successful. Parameters should be send by external server.
        /// </summary>
        /// <param name="id">Transaction id.</param>
        /// <param name="operation_number">Transaction number.</param>
        /// <param name="operation_type">Transaction type.</param>
        /// <param name="operation_status">Transaction status.</param>
        /// <returns>True if transaction success otherwise false.</returns>
        public override bool IsPaymentCompleted(int id, string operation_number, string operation_type, string operation_status)
        {
            return id.ToString() == settings.DotPayId &&
                    operation_type == "payment" &&
                    operation_status == "completed" &&
                    Regex.IsMatch(operation_number, @"^M\d{4,5}\-\d{4,5}$");
        }
    }
}
