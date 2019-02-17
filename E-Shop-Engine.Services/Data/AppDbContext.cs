using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using E_Shop_Engine.Domain.Abstract;
using E_Shop_Engine.Domain.DomainModel;
using E_Shop_Engine.Domain.DomainModel.IdentityModel;
using E_Shop_Engine.Services.Data.EntitiesConfigurations;
using Microsoft.AspNet.Identity.EntityFramework;

namespace E_Shop_Engine.Services.Data
{
    public class AppDbContextInit : NullDatabaseInitializer<AppDbContext>
    {

    }

    public class AppDbContext : IdentityDbContext<AppUser>, IAppDbContext
    {
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<CartLine> CartLines { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Subcategory> Subcategories { get; set; }
        public DbSet<Settings> Settings { get; set; }

        public AppDbContext() : base("ShopEngineDb")
        {

        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Conventions.Remove<ManyToManyCascadeDeleteConvention>();
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();

            modelBuilder.Entity<IdentityUserLogin>().HasKey<string>(l => l.UserId);
            modelBuilder.Entity<IdentityRole>().HasKey<string>(r => r.Id);
            modelBuilder.Entity<IdentityUserRole>().HasKey(r => new { r.RoleId, r.UserId });
            modelBuilder.Entity<AppRole>().ToTable("AppRoles");

            modelBuilder.Configurations.Add(new AddressEntityConfig());
            modelBuilder.Configurations.Add(new CategoryEntityConfig());
            modelBuilder.Configurations.Add(new UserEntityConfig());
            modelBuilder.Configurations.Add(new OrderEntityConfig());
            modelBuilder.Configurations.Add(new CartEntityConfig());
            modelBuilder.Configurations.Add(new CartLineEntityConfig());
            modelBuilder.Configurations.Add(new ProductEntityConfig());
            modelBuilder.Configurations.Add(new SubcategoryEntityConfig());
        }
    }
}
