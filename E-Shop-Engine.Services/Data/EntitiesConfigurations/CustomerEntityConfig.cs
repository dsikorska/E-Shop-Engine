using System.Data.Entity.ModelConfiguration;
using E_Shop_Engine.Domain.DomainModel;

namespace E_Shop_Engine.Services.Data.EntitiesConfigurations
{
    class CustomerEntityConfig : EntityTypeConfiguration<Customer>
    {
        public CustomerEntityConfig()
        {
            HasKey(c => c.ID);
            Property(c => c.Name)
                .IsRequired()
                .HasMaxLength(100);
            Property(c => c.Surname)
                .IsRequired()
                .HasMaxLength(100);
            Property(c => c.Password)
                .IsRequired()
                .HasMaxLength(250);
            Property(c => c.Email)
                .IsRequired()
                .HasMaxLength(150);
            Property(c => c.PhoneNumber)
                .IsRequired()
                .HasMaxLength(50);
            HasOptional(c => c.Orders);
            HasRequired(c => c.Address)
                .WithMany()
                .HasForeignKey(c => c.AddressID);
            HasMany(c => c.Orders)
                .WithRequired(o => o.Customer)
                .HasForeignKey(o => o.CustomerID);
            Property(c => c.Created)
                .IsRequired()
                .HasColumnType("datetime2");
        }
    }
}
