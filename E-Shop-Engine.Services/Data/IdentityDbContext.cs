using System.Data.Entity;
using E_Shop_Engine.Domain.DomainModel.IdentityModel;
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

    public class IdentityDbInit : NullDatabaseInitializer<IdentityDbContext>
    {

    }
}
