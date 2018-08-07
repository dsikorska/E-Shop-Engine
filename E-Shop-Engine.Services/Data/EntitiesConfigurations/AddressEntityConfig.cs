using System.Data.Entity.ModelConfiguration;
using E_Shop_Engine.Domain.DomainModel;

namespace E_Shop_Engine.Services.Data.EntitiesConfigurations
{
    public class AddressEntityConfig : EntityTypeConfiguration<Address>
    {
        public AddressEntityConfig()
        {
            HasKey(a => a.ID);
            HasRequired(a => a.Customer);
            HasRequired(a => a.Street);
            HasRequired(a => a.Line1);
            HasRequired(a => a.ZipCode);
            HasRequired(a => a.Country);
            HasRequired(a => a.City);
            HasOptional(a => a.Line2);
            HasOptional(a => a.State);
            Property(a => a.City)
                .HasMaxLength(100);
            Property(a => a.Street)
                .HasMaxLength(100);
            Property(a => a.Line1)
                .HasMaxLength(100);
            Property(a => a.Line2)
                .HasMaxLength(100);
            Property(a => a.State)
                .HasMaxLength(100);
            Property(a => a.Country)
                .HasMaxLength(100);
        }
    }
}
