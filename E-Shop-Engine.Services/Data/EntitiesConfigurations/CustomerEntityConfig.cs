using System.Data.Entity.ModelConfiguration;
using E_Shop_Engine.Domain.DomainModel.IdentityModel;

namespace E_Shop_Engine.Services.Data.EntitiesConfigurations
{
    public class CustomerEntityConfig : EntityTypeConfiguration<Customer>
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

            HasRequired(c => c.Address)
                .WithRequiredPrincipal(a => a.Customer);

            HasMany(c => c.Orders)
                .WithRequired(o => o.Customer);

            HasRequired(c => c.Cart)
                .WithRequiredPrincipal(o => o.Customer);
        }
    }
}
