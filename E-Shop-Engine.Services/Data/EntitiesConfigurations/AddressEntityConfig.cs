using System.Data.Entity.ModelConfiguration;
using E_Shop_Engine.Domain.DomainModel;

namespace E_Shop_Engine.Services.Data.EntitiesConfigurations
{
    public class AddressEntityConfig : EntityTypeConfiguration<Address>
    {
        public AddressEntityConfig()
        {
            HasKey(a => a.ID);

            Property(a => a.City)
                .IsRequired()
                .HasMaxLength(100);

            Property(a => a.Street)
                .HasMaxLength(100)
                .IsRequired();

            Property(a => a.Line1)
                .HasMaxLength(100)
                .IsRequired();

            Property(a => a.Line2)
                .HasMaxLength(100);

            Property(a => a.State)
                .HasMaxLength(100);

            Property(a => a.Country)
                .HasMaxLength(100)
                .IsRequired();
        }
    }
}
