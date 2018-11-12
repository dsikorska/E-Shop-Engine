using E_Shop_Engine.Domain.DomainModel;
using E_Shop_Engine.Domain.TempModel;

namespace E_Shop_Engine.Domain.Interfaces
{
    public interface IPaymentTransactionRepository
    {
        bool ValidateSameCurrencyTransaction(string operation_amount, string operation_currency, string control, Order order);
        bool ValidateDataSavedAtExternalServer(Order order, DotPayOperationDetails data);
        bool IsTransactionSameCurrency(string operation_amount, string operation_currency, string operation_original_amount, string operation_original_currency);
        bool IsPaymentCompleted(int id, string operation_number, string operation_type, string operation_status);
    }
}
