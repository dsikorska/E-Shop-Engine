using System.Web.Mvc;
using E_Shop_Engine.Domain.DomainModel;
using E_Shop_Engine.Domain.Interfaces;

namespace E_Shop_Engine.Website.Extensions
{
    public static class HtmlHelpers
    {
        /// <summary>
        /// Get instance of Settings.
        /// </summary>
        /// <param name="helper"></param>
        /// <returns>Settings.</returns>
        public static Settings Settings(this HtmlHelper helper)
        {
            ISettingsRepository settingsRepository = DependencyResolver.Current.GetService<ISettingsRepository>();
            return settingsRepository.Get();
        }
    }
}