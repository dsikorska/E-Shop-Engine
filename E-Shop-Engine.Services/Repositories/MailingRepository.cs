using System;
using System.Linq;
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
                Body = "Thank you for registering"
            };
            await SendMail(mail);
        }

        public async Task ActivationMail(string mailTo, string url)
        {
            MailMessage mail = new MailMessage(_settings.NotificationReplyEmail, mailTo)
            {
                Subject = "Activate account at " + _settings.ShopName,
                Body = "Please confirm your account by clicking <a href=\"" + url + "\">here</a>"
            };
            await SendMail(mail);
        }

        public async Task ResetPasswordMail(string mailTo, string url)
        {
            MailMessage mail = new MailMessage(_settings.NotificationReplyEmail, mailTo)
            {
                Subject = "Reset password at " + _settings.ShopName,
                Body = "Reset password by clicking <a href=\"" + url + "\">here</a>"
            };
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

        //TODO port to int
        private SmtpClient GetSmtpCLient()
        {
            return new SmtpClient(_settings.SMTP, int.Parse(_settings.SMTPPort))
            {
                Credentials = new System.Net.NetworkCredential(_settings.NotificationReplyEmail, _settings.SMTPPassword),
                UseDefaultCredentials = false,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                EnableSsl = _settings.SMTPEnableSSL.Value
            };
        }
    }
}
