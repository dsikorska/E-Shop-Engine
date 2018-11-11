using System.Configuration;

namespace E_Shop_Engine.Services
{
    public static class AppSettings
    {
        public static string GetDotPayLogin()
        {
            return ConfigurationManager.AppSettings["dotPayLogin"];
        }

        public static string GetDotPayPassword()
        {
            return ConfigurationManager.AppSettings["dotPayPassword"];
        }

        public static string GetDotPayPIN()
        {
            return ConfigurationManager.AppSettings["dotPayPIN"];
        }

        public static string GetSmtpUsername()
        {
            return ConfigurationManager.AppSettings["SmtpUsername"];
        }

        public static string GetSmtpPassword()
        {
            return ConfigurationManager.AppSettings["SmtpPassword"];
        }
    }
}
