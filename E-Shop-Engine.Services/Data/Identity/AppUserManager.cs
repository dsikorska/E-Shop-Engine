using System.Collections.Generic;
using E_Shop_Engine.Domain.DomainModel.IdentityModel;
using E_Shop_Engine.Services.Data.Identity.Abstraction;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security.DataProtection;

namespace E_Shop_Engine.Services.Data.Identity
{
    public class AppUserManager : UserManager<AppUser>, IAppUserManager
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

        public AppUser FindById(string id)
        {
            return FindById(id);
        }

        public IEnumerable<AppUser> FindUsersByEmail(string term)
        {
            return FindUsersByEmail(term);
        }

        public IEnumerable<AppUser> FindUsersByName(string term)
        {
            return FindUsersByName(term);
        }

        public IEnumerable<AppUser> FindUsersBySurname(string term)
        {
            return FindUsersBySurname(term);
        }
    }
}
