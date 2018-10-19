using E_Shop_Engine.Domain.DomainModel.IdentityModel;
using E_Shop_Engine.Domain.Interfaces;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;

namespace E_Shop_Engine.Services.Data.Identity
{
    public class AppUserManager : UserManager<AppUser>
    {
        //TODO make it more elegant
        public AppUserManager(IUserStore<AppUser> store, IAppDbContext dbContext) : base(store)
        {
            AppDbContext db = dbContext as AppDbContext;
            Store = new UserStore<AppUser>(db);
            this.PasswordValidator = new PasswordValidator()
            {
                RequireDigit = true,
                RequiredLength = 6,
                RequireLowercase = true,
                RequireUppercase = true,
                RequireNonLetterOrDigit = true
            };
        }

        public static AppUserManager Create(IdentityFactoryOptions<AppUserManager> options, IOwinContext context)
        {
            AppDbContext db = context.Get<AppDbContext>();
            AppUserManager manager = new AppUserManager(new UserStore<AppUser>(), db)
            {
                PasswordValidator = new PasswordValidator()
                {
                    RequireDigit = true,
                    RequiredLength = 6,
                    RequireLowercase = true,
                    RequireUppercase = true,
                    RequireNonLetterOrDigit = true
                }
            };

            return manager;
        }
    }
}
