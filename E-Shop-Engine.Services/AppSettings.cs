using System.Configuration;

namespace E_Shop_Engine.Services
{
    public static class AppSettings
    {
        /// <summary>
        /// Get dot pay login from application settings.
        /// </summary>
        /// <returns>Dot pay login.</returns>
        public static string GetDotPayLogin()
        {
            return ConfigurationManager.AppSettings["dotPayLogin"];
        }

        /// <summary>
        /// Get dot pay password from application settings.
        /// </summary>
        /// <returns>Dot pay password.</returns>
        public static string GetDotPayPassword()
        {
            return ConfigurationManager.AppSettings["dotPayPassword"];
        }

        /// <summary>
        /// Get dot pay PIN from application settings.
        /// </summary>
        /// <returns>Dot pay PIN.</returns>
        public static string GetDotPayPIN()
        {
            return ConfigurationManager.AppSettings["dotPayPIN"];
        }

        /// <summary>
        /// Get SMTP username from application settings.
        /// </summary>
        /// <returns>SMTP username.</returns>
        public static string GetSmtpUsername()
        {
            return ConfigurationManager.AppSettings["SmtpUsername"];
        }

        /// <summary>
        /// Get SMTP password from application settings.
        /// </summary>
        /// <returns>SMTP password.</returns>
        public static string GetSmtpPassword()
        {
            return ConfigurationManager.AppSettings["SmtpPassword"];
        }
    }
}
