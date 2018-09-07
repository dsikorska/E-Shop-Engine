namespace E_Shop_Engine.Services.Migrations.Identity
{
    using System;
    using System.Data.Entity.Migrations;
    using E_Shop_Engine.Domain.DomainModel.IdentityModel;
    using E_Shop_Engine.Services.Data.Identity;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;

    internal sealed class Configuration : DbMigrationsConfiguration<Data.IdentityDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            MigrationsDirectory = @"Migrations\Identity";
            ContextKey = "E_Shop_Engine.Services.Data.IdentityDbContext";
        }

        protected override void Seed(Data.IdentityDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.

            AppUserManager userManager = new AppUserManager(new UserStore<AppUser>(context));
            AppRoleManager roleManager = new AppRoleManager(new RoleStore<AppRole>(context));
            //TODO sensitive data
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
                    Email = email,
                    Created = DateTime.UtcNow
                }, password);

                user = userManager.FindByName(userName);
            }
            if (!userManager.IsInRole(user.Id, roleName))
            {
                userManager.AddToRole(user.Id, roleName);
            }
            context.SaveChanges();
        }
    }
}
