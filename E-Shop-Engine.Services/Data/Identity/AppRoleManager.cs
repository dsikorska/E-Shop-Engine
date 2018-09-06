using System;
using E_Shop_Engine.Domain.DomainModel.IdentityModel;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;

namespace E_Shop_Engine.Services.Data.Identity
{
    public class AppRoleManager : RoleManager<AppRole>, IDisposable
    {
        public AppRoleManager(RoleStore<AppRole> store) : base(store)
        {

        }

        public static AppRoleManager Create(IdentityFactoryOptions<AppRoleManager> options, IOwinContext context)
        {
            IdentityDbContext db = context.Get<IdentityDbContext>();
            AppRoleManager manager = new AppRoleManager(new RoleStore<AppRole>(db));

            return manager;
        }
    }
}
