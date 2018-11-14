using System.Web.Mvc;
using E_Shop_Engine.Services.Data.Identity;
using Microsoft.AspNet.Identity;

namespace E_Shop_Engine.Website.Extensions
{
    public static class IdentityHelpers
    {
        public static MvcHtmlString GetUserName(this HtmlHelper html, string id)
        {
            AppUserManager manager = DependencyResolver.Current.GetService<AppUserManager>();
            string result = manager.FindById(id)?.UserName;
            return new MvcHtmlString(result);
        }
    }
}