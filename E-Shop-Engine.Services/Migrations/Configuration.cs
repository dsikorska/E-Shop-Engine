namespace E_Shop_Engine.Services.Migrations
{
    using System;
    using System.Collections.ObjectModel;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using E_Shop_Engine.Domain.DomainModel;
    using E_Shop_Engine.Domain.DomainModel.IdentityModel;
    using E_Shop_Engine.Services.Data;
    using E_Shop_Engine.Services.Data.Identity;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;

    internal sealed class Configuration : DbMigrationsConfiguration<AppDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(AppDbContext context)
        {
            Settings currentSettings = context.Settings.FirstOrDefault();
            if (currentSettings == null)
            {
                Settings settings = new Settings()
                {
                    Currency = "USD",
                    ShopName = "My Demo Shop",
                    ContactEmailAddress = "my@email.com",
                    NotificationReplyEmail = "noreply@email.com",
                    SMTPEnableSSL = false
                };

                context.Settings.Add(settings);
            }

            AppUserManager userManager = new AppUserManager(new UserStore<AppUser>(context), null);
            AppRoleManager roleManager = new AppRoleManager(new RoleStore<AppRole>(context));

            string roleName = "Administrators";
            string userName = "Admin";
            string password = "Qwerty1!";
            string email = "my@email.com";

            if (!roleManager.RoleExists(roleName))
            {
                roleManager.Create(new AppRole(roleName));
            }

            if (!roleManager.RoleExists("Staff"))
            {
                roleManager.Create(new AppRole("Staff"));
            }

            AppUser user = userManager.FindByEmail(email);
            if (user == null)
            {
                AppUser newUser = new AppUser
                {
                    Name = userName,
                    Surname = userName,
                    UserName = email,
                    Email = email,
                    Created = DateTime.UtcNow,
                    EmailConfirmed = true
                };

                newUser.Carts = new Collection<Cart>
                {
                    new Cart(newUser)
                };

                IdentityResult result = userManager.Create(newUser, password);
                user = userManager.Find(email, password);
            }

            if (!userManager.IsInRole(user.Id, roleName))
            {
                userManager.AddToRole(user.Id, roleName);
            }
            context.SaveChanges();
        }
    }
}
