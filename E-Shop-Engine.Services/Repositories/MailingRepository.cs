﻿using System;
using System.Linq;
using System.Net;
using System.Net.Mail;
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

        public void CustomMail(string sender, string senderName, string body)
        {
            string subject = "Contact from " + senderName + " <" + senderName + ">";
            MailMessage mail = GetMessage(_settings.ContactEmailAddress, body, subject);
            SendMail(mail);
        }

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

        public void PaymentFailedMail(string mailTo, string orderNumber)
        {
            string body = Properties.Resources.OnlyTextTemplateMail
                .Replace("#shopName#", _settings.ShopName)
                .Replace("#title#", "Order " + orderNumber + "on hold.")
                .Replace("#text#", "Something went wrong while processing Your payment for order number " + orderNumber +
                ". Your order is on hold. Please contact us at " + _settings.ContactEmailAddress);

            string subject = "Order " + orderNumber + "on hold.";
            string to = mailTo;
            MailMessage mail = GetMessage(mailTo, body, subject);
            SendMail(mail);
        }

        private void SendMail(MailMessage mail)
        {
            SmtpClient smtpClient = GetSmtpCLient();
            try
            {
                smtpClient.Send(mail);
            }
            catch (Exception e)
            {

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
