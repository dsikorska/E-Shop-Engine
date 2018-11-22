namespace E_Shop_Engine.Domain.Interfaces
{
    public interface IMailingRepository
    {
        void WelcomeMail(string mailTo);
        void ActivationMail(string mailTo, string url);
        void ResetPasswordMail(string mailTo, string url);
        void PasswordChangedMail(string mailTo);
        void CustomMail(string sender, string senderName, string body);
        void PaymentFailedMail(string mailTo, string orderNumber);
        void OrderChangedStatusMail(string mailTo, string orderNumber, string orderStatus, string title);
        void TestMail();
    }
}
