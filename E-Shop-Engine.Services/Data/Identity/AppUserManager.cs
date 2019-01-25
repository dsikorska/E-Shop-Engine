using System.Collections.Generic;
using System.Linq;
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
            return UserManagerExtensions.FindById(this, id);
        }

        /// <summary>
        /// Get all AppUsers that email address contains search term.
        /// </summary>
        /// <param name="manager">User Manager.</param>
        /// <param name="searchTerm">Search term.</param>
        /// <returns>AppUsers that email address contains search term.</returns>
        public IEnumerable<AppUser> FindUsersByEmail(string searchTerm)
        {
            return Users.Where(x => x.Email.Contains(searchTerm)).Select(x => x);
        }

        /// <summary>
        /// Get all AppUsers that name contains search term.
        /// </summary>
        /// <param name="manager">User Manager.</param>
        /// <param name="searchTerm">Search term.</param>
        /// <returns>AppUsers that name contains search term.</returns>
        public IEnumerable<AppUser> FindUsersByName(string searchTerm)
        {
            return Users.Where(x => x.Name.Contains(searchTerm)).Select(x => x);
        }

        /// <summary>
        /// Get all AppUsers that surname contains search term.
        /// </summary>
        /// <param name="manager">User Manager.</param>
        /// <param name="searchTerm">Search term.</param>
        /// <returns>AppUsers that surname contains search term.</returns>
        public IEnumerable<AppUser> FindUsersBySurname(string searchTerm)
        {
            return Users.Where(x => x.Surname.Contains(searchTerm)).Select(x => x);
        }
    }
}
