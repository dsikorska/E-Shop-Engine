using System.Collections.ObjectModel;
using System.Web;
using System.Web.Mvc;
using E_Shop_Engine.Domain.DomainModel;
using E_Shop_Engine.Domain.DomainModel.IdentityModel;
using E_Shop_Engine.Services.Data.Identity;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

namespace E_Shop_Engine.Website.CustomFilters
{
    public class IsCartInitializedAttribute : FilterAttribute, IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (context.HttpContext.User.Identity.IsAuthenticated)
            {
                AppUserManager userManager = context.HttpContext.GetOwinContext().GetUserManager<AppUserManager>();
                string userId = context.HttpContext.User.Identity.GetUserId();
                AppUser user = userManager.FindById(userId);
                if (user.Cart == null)
                {
                    user.Cart = new Cart
                    {
                        CartLines = new Collection<CartLine>()
                    };
                }
            }
        }

        public void OnActionExecuted(ActionExecutedContext filterContext)
        {
            //do nothing
        }
    }
}