using System.Data.Entity;
using E_Shop_Engine.Domain.DomainModel.IdentityModel;
using E_Shop_Engine.Services.Data.Identity;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace E_Shop_Engine.Services.Data
{
    public class IdentityDbContext : IdentityDbContext<AppUser>
    {
        public IdentityDbContext() : base("ShopEngineDb")
        {

        }

        static IdentityDbContext()
        {
            Database.SetInitializer(new IdentityDbInit());
        }

        public static IdentityDbContext Create()
        {
            return new IdentityDbContext();
        }
    }

    public class IdentityDbInit : DropCreateDatabaseIfModelChanges<IdentityDbContext>
    {
        protected override void Seed(IdentityDbContext context)
        {
            PerformInitialSetup(context);
            base.Seed(context);
        }

        public void PerformInitialSetup(IdentityDbContext context)
        {
            AppUserManager userManager = new AppUserManager(new UserStore<AppUser>(context));
            AppRoleManager roleManager = new AppRoleManager(new RoleStore<AppRole>(context));

            string roleName = "Administrators";
            string userName = "Admin";
            string password = "123456";
            string email = "ladyhail@outlook.com";

            if (!roleManager.RoleExists(roleName))
            {
                roleManager.Create(new AppRole(roleName));
            }

            AppUser user = userManager.FindByName(userName);
            if (user == null)
            {
                userManager.Create(new AppUser
                {
                    UserName = userName,
                    Email = email
                }, password);

                user = userManager.FindByName(userName);
                if (!userManager.IsInRole(user.Id, roleName))
                {
                    userManager.AddToRole(user.Id, roleName);
                }
            }
        }
    }
}
