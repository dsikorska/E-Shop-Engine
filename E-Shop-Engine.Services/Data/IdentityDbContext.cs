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

    public class IdentityDbInit : DropCreateDatabaseIfModelChanges<IdentityDbContext>
    {
        protected override void Seed(IdentityDbContext context)
        {
            PerformInitialSetup(context);
            base.Seed(context);
        }

        public void PerformInitialSetup(IdentityDbContext context)
        {

        }
    }
}
