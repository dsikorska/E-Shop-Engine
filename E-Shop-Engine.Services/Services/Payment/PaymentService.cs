using E_Shop_Engine.Domain.Abstract;
using E_Shop_Engine.Domain.DomainModel;
using E_Shop_Engine.Domain.Models;

namespace E_Shop_Engine.Services.Services.Payment
{
    public abstract class PaymentService : IPaymentService
    {
        public abstract bool IsPaymentCompleted(int id, string operation_number, string operation_type, string operation_status);

        public abstract bool IsTransactionSameCurrency(string operation_amount, string operation_currency, string operation_original_amount, string operation_original_currency);

        public abstract bool ValidateDataSavedAtExternalServer(Order order, PaymentDetails data);

        public abstract bool ValidateSameCurrencyTransaction(string operation_amount, string operation_currency, string control, Order order);

        public abstract string GetOperationDetails(string operation_number);
    }
}
