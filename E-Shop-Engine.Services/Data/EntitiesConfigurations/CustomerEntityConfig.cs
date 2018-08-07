using System.Data.Entity.ModelConfiguration;
using E_Shop_Engine.Domain.DomainModel;

namespace E_Shop_Engine.Services.Data.EntitiesConfigurations
{
    class CustomerEntityConfig : EntityTypeConfiguration<Customer>
    {
        public CustomerEntityConfig()
        {
            HasKey(c => c.ID);
            HasRequired(c => c.Email);
            HasRequired(c => c.Password);
            HasRequired(c => c.Name);
            HasRequired(c => c.Surname);
            HasRequired(c => c.PhoneNumber);
            HasOptional(c => c.Orders);
            HasRequired(c => c.Address)
                .WithRequiredPrincipal(a => a.Customer)
                .WillCascadeOnDelete(true);
            HasMany(c => c.Orders)
                .WithRequired(o => o.Customer)
                .HasForeignKey(o => o.CustomerID)
                .WillCascadeOnDelete(true);
            Property(c => c.Name)
                .HasMaxLength(100);
            Property(c => c.Surname)
                .HasMaxLength(100);
            Property(c => c.Password)
                .HasMaxLength(250);
            Property(c => c.Email)
                .HasMaxLength(150);
            Property(c => c.PhoneNumber)
                .HasMaxLength(50);
        }
    }
}
