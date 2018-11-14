using E_Shop_Engine.Domain.DomainModel.IdentityModel;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security.DataProtection;

namespace E_Shop_Engine.Services.Data.Identity
{
    public class AppUserManager : UserManager<AppUser>
    {
        public AppUserManager(IUserStore<AppUser> store, IDataProtectionProvider dataProtectionProvider) : base(store)
        {
            UserValidator = new UserValidator<AppUser>(this)
            {
                AllowOnlyAlphanumericUserNames = false,
                RequireUniqueEmail = true
            };

            PasswordValidator = new PasswordValidator
            {
                RequireDigit = true,
                RequiredLength = 6,
                RequireLowercase = true,
                RequireUppercase = true,
                RequireNonLetterOrDigit = true
            };

            UserLockoutEnabledByDefault = false;
            //DefaultAccountLockoutTimeSpan = TimeSpan.FromMinutes(5);
            //MaxFailedAccessAttemptsBeforeLockout = 5;
            if (dataProtectionProvider != null)
            {
                UserTokenProvider = new DataProtectorTokenProvider<AppUser>(dataProtectionProvider.Create("ASP.NET Identity"));
            }
        }
    }
}
