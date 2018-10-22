using System;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using E_Shop_Engine.Domain.DomainModel;
using E_Shop_Engine.Domain.Interfaces;
using E_Shop_Engine.Services.Data;

namespace E_Shop_Engine.Services.Repositories
{
    public class MailingRepository : IMailingRepository
    {
        private readonly Settings _settings;

        public MailingRepository(AppDbContext context)
        {
            _settings = context.Settings.FirstOrDefault();
        }

        public async Task WelcomeMail(string mailTo)
        {
            string body = Properties.Resources.WelcomeTemplateMail.Replace("#shopName#", _settings.ShopName);
            string subject = "Welcome at " + _settings.ShopName;
            string to = mailTo;
            MailMessage mail = GetMessage(mailTo, body, subject);
            await SendMail(mail);
        }

        public async Task ActivationMail(string mailTo, string url)
        {
            string body = Properties.Resources.ActivationTemplateMail.Replace("#url#", url).Replace("#shopName#", _settings.ShopName);
            string subject = "Activate your account at " + _settings.ShopName;
            string to = mailTo;
            MailMessage mail = GetMessage(mailTo, body, subject);
            await SendMail(mail);
        }

        public async Task ResetPasswordMail(string mailTo, string url)
        {
            string body = Properties.Resources.ResetPasswordTemplateMail.Replace("#url#", url).Replace("#shopName#", _settings.ShopName);
            string subject = "Reset password at " + _settings.ShopName;
            string to = mailTo;
            MailMessage mail = GetMessage(mailTo, body, subject);
            await SendMail(mail);
        }

        private async Task SendMail(MailMessage mail)
        {

            SmtpClient smtpClient = GetSmtpCLient();

            try
            {
                await smtpClient.SendMailAsync(mail);
            }
            catch (Exception e)
            {
                //TODO
            }
        }

        private SmtpClient GetSmtpCLient()
        {
            NetworkCredential credentials = new NetworkCredential()
            {
                Password = _settings.SMTPPassword,
                UserName = _settings.SMTPUsername
            };

            return new SmtpClient(_settings.SMTP, _settings.SMTPPort)
            {
                UseDefaultCredentials = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                EnableSsl = _settings.SMTPEnableSSL,
                Credentials = credentials
            };
        }

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
