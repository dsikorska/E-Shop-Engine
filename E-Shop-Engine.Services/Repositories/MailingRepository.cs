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
            MailMessage mail = new MailMessage(_settings.NotificationReplyEmail, mailTo)
            {
                Subject = "Registering at " + _settings.ShopName,
                Body = "<p>Thank you for registering at " + _settings.ShopName + "</p>",
            };
            await SendMail(mail);
        }

        public async Task ActivationMail(string mailTo, string url)
        {
            MailMessage mail = new MailMessage(_settings.NotificationReplyEmail, mailTo)
            {
                Subject = "Activate account at " + _settings.ShopName,
                Body = "<p>Please confirm your account by clicking <a href=\"" + url + "\">here</a></p>",
            };
            await SendMail(mail);
        }

        public async Task ResetPasswordMail(string mailTo, string url)
        {
            MailMessage mail = new MailMessage(_settings.NotificationReplyEmail, mailTo)
            {
                Subject = "Reset password at " + _settings.ShopName,
                Body = "<p>Reset password by clicking <a href=\"" + url + "\">here</a></p>",
            };
            await SendMail(mail);
        }

        private async Task SendMail(MailMessage mail)
        {
            mail.IsBodyHtml = true;
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
    }
}
