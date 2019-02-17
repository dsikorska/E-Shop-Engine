namespace E_Shop_Engine.Domain.Abstract
{
    public interface IMailingService
    {
        /// <summary>
        /// Send welcome mail.
        /// </summary>
        /// <param name="mailTo">Mail recipient.</param>
        void WelcomeMail(string mailTo);

        /// <summary>
        /// Send mail with activation link.
        /// </summary>
        /// <param name="mailTo">Mail recipient.</param>
        /// <param name="url">Activation link.</param>
        void ActivationMail(string mailTo, string url);

        /// <summary>
        /// Send mail with reset password link.
        /// </summary>
        /// <param name="mailTo">Mail recipient.</param>
        /// <param name="url">Reset password link.</param>
        void ResetPasswordMail(string mailTo, string url);

        /// <summary>
        /// Send mail that informs recipient password changed.
        /// </summary>
        /// <param name="mailTo">Mail recipient.</param>
        void PasswordChangedMail(string mailTo);

        /// <summary>
        /// User send message to admin address email (specified in Settings).
        /// </summary>
        /// <param name="sender">Mail sender.</param>
        /// <param name="senderName">Sender name.</param>
        /// <param name="body">Message.</param>
        void CustomMail(string sender, string senderName, string body);

        /// <summary>
        /// Send mail that informs recipient the payment failed.
        /// </summary>
        /// <param name="mailTo">Mail recipient.</param>
        /// <param name="orderNumber">Property that identifies the failed payment.</param>
        void PaymentFailedMail(string mailTo, string orderNumber);

        /// <summary>
        /// Send mail that informs recipient order status has changed.
        /// </summary>
        /// <param name="mailTo">Mail recipient.</param>
        /// <param name="orderNumber">Property that identifies the order.</param>
        /// <param name="orderStatus">Current order status.</param>
        /// <param name="title">Mail title.</param>
        void OrderChangedStatusMail(string mailTo, string orderNumber, string orderStatus, string title);

        /// <summary>
        /// Send test mail to Contact Email Address (specified in Settings).
        /// </summary>
        void TestMail();
    }
}
