using System.Linq;
using System.Net;
using System.Net.Mail;
using E_Shop_Engine.Domain.Abstract;
using E_Shop_Engine.Domain.DomainModel;
using E_Shop_Engine.Services.Data;

namespace E_Shop_Engine.Services.Repositories
{
    public class MailingService : IMailingService
    {
        private readonly Settings _settings;

        public MailingService(AppDbContext context)
        {
            _settings = context.Settings.FirstOrDefault();
        }

        /// <summary>
        /// User send message to admin address email (specified in Settings).
        /// </summary>
        /// <param name="sender">Mail sender.</param>
        /// <param name="senderName">Sender name.</param>
        /// <param name="body">Message.</param>
        public void CustomMail(string sender, string senderName, string body)
        {
            string subject = "Contact from " + senderName + " <" + senderName + ">";
            MailMessage mail = GetMessage(_settings.ContactEmailAddress, body, subject);
            SendMail(mail);
        }

        /// <summary>
        /// Send welcome mail.
        /// </summary>
        /// <param name="mailTo">Mail recipient.</param>
        public void WelcomeMail(string mailTo)
        {
            string body = Properties.Resources.OnlyTextTemplateMail
                .Replace("#shopName#", _settings.ShopName)
                .Replace("#title#", "Welcome at " + _settings.ShopName)
                .Replace("#text#", "Thank You for registering at " + _settings.ShopName);

            string subject = "Welcome at " + _settings.ShopName;
            string to = mailTo;
            MailMessage mail = GetMessage(mailTo, body, subject);
            SendMail(mail);
        }

        /// <summary>
        /// Send mail with activation link.
        /// </summary>
        /// <param name="mailTo">Mail recipient.</param>
        /// <param name="url">Activation link.</param>
        public void ActivationMail(string mailTo, string url)
        {
            string body = Properties.Resources.OneButtonTemplateMail
                .Replace("#url#", url)
                .Replace("#shopName#", _settings.ShopName)
                .Replace("#title#", "Activate Your account!")
                .Replace("#text#", "Please click button below to activate Your account: ")
                .Replace("#btnText#", "Activate account");

            string subject = "Activate your account at " + _settings.ShopName;
            string to = mailTo;
            MailMessage mail = GetMessage(mailTo, body, subject);
            SendMail(mail);
        }

        /// <summary>
        /// Send mail with reset password link.
        /// </summary>
        /// <param name="mailTo">Mail recipient.</param>
        /// <param name="url">Reset password link.</param>
        public void ResetPasswordMail(string mailTo, string url)
        {
            string body = Properties.Resources.OneButtonTemplateMail.Replace("#url#", url)
                .Replace("#shopName#", _settings.ShopName)
                .Replace("#title#", "Reset password")
                .Replace("#text#", "Please click button below to reset password: ")
                .Replace("#btnText#", "Reset password");

            string subject = "Reset password at " + _settings.ShopName;
            string to = mailTo;
            MailMessage mail = GetMessage(mailTo, body, subject);
            SendMail(mail);
        }

        /// <summary>
        /// Send mail that informs recipient password changed.
        /// </summary>
        /// <param name="mailTo">Mail recipient.</param>
        public void PasswordChangedMail(string mailTo)
        {
            string body = Properties.Resources.OnlyTextTemplateMail
                .Replace("#shopName#", _settings.ShopName)
                .Replace("#title#", "Password changed")
                .Replace("#text#", "Password to Your account has been changed");

            string subject = "Password changed";
            string to = mailTo;
            MailMessage mail = GetMessage(mailTo, body, subject);
            SendMail(mail);
        }

        /// <summary>
        /// Send mail that informs recipient order status has changed.
        /// </summary>
        /// <param name="mailTo">Mail recipient.</param>
        /// <param name="orderNumber">Property that identifies the order.</param>
        /// <param name="orderStatus">Current order status.</param>
        /// <param name="title">Mail title.</param>
        public void OrderChangedStatusMail(string mailTo, string orderNumber, string orderStatus, string title)
        {
            string body = Properties.Resources.OnlyTextTemplateMail
                .Replace("#shopName#", _settings.ShopName)
                .Replace("#title#", title)
                .Replace("#text#", "Your order " + orderNumber + " status updated: " + orderStatus);

            string subject = title;
            string to = mailTo;
            MailMessage mail = GetMessage(mailTo, body, subject);
            SendMail(mail);
        }

        /// <summary>
        /// Send mail that informs recipient the payment failed.
        /// </summary>
        /// <param name="mailTo">Mail recipient.</param>
        /// <param name="orderNumber">Property that identifies the failed payment.</param>
        public void PaymentFailedMail(string mailTo, string orderNumber)
        {
            string body = Properties.Resources.OnlyTextTemplateMail
                .Replace("#shopName#", _settings.ShopName)
                .Replace("#title#", "Order " + orderNumber + " on hold.")
                .Replace("#text#", "Something went wrong while processing Your payment for order number " + orderNumber +
                ". Your order is on hold. Please contact us at " + _settings.ContactEmailAddress);

            string subject = "Order " + orderNumber + " on hold.";
            string to = mailTo;
            MailMessage mail = GetMessage(mailTo, body, subject);
            SendMail(mail);
        }

        /// <summary>
        /// Send test mail to Contact Email Address (specified in Settings).
        /// </summary>
        public void TestMail()
        {
            string body = Properties.Resources.OnlyTextTemplateMail
                .Replace("#text#", "This is test message. Your SMTP settings are ok.")
                .Replace("#shopName#", _settings.ShopName)
                .Replace("#title#", "Test message");

            string subject = "Test message";
            string to = _settings.ContactEmailAddress;
            MailMessage mail = GetMessage(to, body, subject);
            SendMail(mail);
        }

        /// <summary>
        /// Send specified message.
        /// </summary>
        /// <param name="mail">Send this message.</param>
        private void SendMail(MailMessage mail)
        {
            SmtpClient smtpClient = GetSmtpCLient();
            try
            {
                smtpClient.Send(mail);
            }
            catch
            {
                // Do nothing
            }
        }

        /// <summary>
        /// Get credentials from application settings.
        /// </summary>
        /// <returns>Set up smtp client.</returns>
        private SmtpClient GetSmtpCLient()
        {
            NetworkCredential credentials = new NetworkCredential()
            {
                Password = AppSettings.GetSmtpPassword(),
                UserName = AppSettings.GetSmtpUsername()
            };

            return new SmtpClient(_settings.SMTP, _settings.SMTPPort)
            {
                UseDefaultCredentials = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                EnableSsl = _settings.SMTPEnableSSL,
                Credentials = credentials
            };
        }

        /// <summary>
        /// Setings for message.
        /// </summary>
        /// <param name="mailTo">Mail recipient.</param>
        /// <param name="body">Mail body.</param>
        /// <param name="subject">Mail subject.</param>
        /// <returns>Message ready to send.</returns>
        private MailMessage GetMessage(string mailTo, string body, string subject)
        {
            MailAddress from = new MailAddress(_settings.NotificationReplyEmail, _settings.ShopName);
            MailMessage mail = new MailMessage(_settings.NotificationReplyEmail, mailTo)
            {
                Subject = subject,
                Body = body,
                From = from,
                IsBodyHtml = true
            };
            return mail;
        }
    }
}
