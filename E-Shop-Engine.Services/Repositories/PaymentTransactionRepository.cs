using System.Linq;
using System.Text.RegularExpressions;
using E_Shop_Engine.Domain.DomainModel;
using E_Shop_Engine.Domain.Interfaces;
using E_Shop_Engine.Domain.TempModel;

namespace E_Shop_Engine.Services.Repositories
{
    public class PaymentTransactionRepository : IPaymentTransactionRepository
    {
        private static Settings settings;

        public PaymentTransactionRepository(ISettingsRepository settingsRepository)
        {
            settings = settingsRepository.Get();
        }

        public bool ValidateSameCurrencyTransaction(string transactionValue, string transactionCurrency, string control, Order order)
        {
            return order.OrderNumber == control &&
                    order.OrderedCart.CartLines.Sum(x => x.Product.Price * x.Quantity).ToString() == transactionValue &&
                    settings.Currency == transactionCurrency;
        }

        public bool ValidateDataSavedAtExternalServer(Order order, DotPayOperationDetails externalData)
        {
            return externalData.Control == order.OrderNumber &&
                    externalData.OriginalAmount == order.OrderedCart.CartLines.Sum(x => x.Product.Price * x.Quantity) &&
                    externalData.OriginalCurrency == settings.Currency &&
                    externalData.OperationStatus == "completed" &&
                    externalData.OperationType == "payment";
        }

        public bool IsTransactionSameCurrency(string transactionValue, string transactionCurrency, string transactionOriginalValue, string transactionOriginalCurrency)
        {
            return transactionValue == transactionOriginalValue && transactionCurrency == transactionOriginalCurrency;
        }

        public bool IsPaymentCompleted(int id, string operation_number, string operation_type, string operation_status)
        {
            return id.ToString() == settings.DotPayId &&
                    operation_type == "payment" &&
                    operation_status == "completed" &&
                    Regex.IsMatch(operation_number, @"^M\d{4,5}\-\d{4,5}$");
        }
    }
}
