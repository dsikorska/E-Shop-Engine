using System.Data.Entity;
using System.Data.Entity.ModelConfiguration;
using System.Data.Entity.ModelConfiguration.Conventions;
using E_Shop_Engine.Domain.DomainModel;
using E_Shop_Engine.Services.Data.EntitiesConfigurations;

namespace E_Shop_Engine.Services.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext() : base("ShopEngineDb")
        {

        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<ManyToManyCascadeDeleteConvention>();
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();

            modelBuilder.Configurations.Add(new EntityTypeConfiguration<AddressEntityConfig>());
            modelBuilder.Configurations.Add(new EntityTypeConfiguration<CategoryConfigEntity>());
            modelBuilder.Configurations.Add(new EntityTypeConfiguration<CustomerEntityConfig>());
            modelBuilder.Configurations.Add(new EntityTypeConfiguration<OrderEntityConfig>());
            modelBuilder.Configurations.Add(new EntityTypeConfiguration<OrderLineEntityConfig>());
            modelBuilder.Configurations.Add(new EntityTypeConfiguration<ProductEntityConfig>());
            modelBuilder.Configurations.Add(new EntityTypeConfiguration<SubcategoryEntityConfig>());
        }

        public DbSet<Address> Addresses { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderLine> OrderLines { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Subcategory> Subcategories { get; set; }
    }
}
