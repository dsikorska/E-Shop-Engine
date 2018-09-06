using System.Data.Entity.ModelConfiguration;
using E_Shop_Engine.Domain.DomainModel.IdentityModel;

namespace E_Shop_Engine.Services.Data.EntitiesConfigurations
{
    internal class UserEntityConfig : EntityTypeConfiguration<AppUser>
    {
        public UserEntityConfig()
        {
            HasKey(c => c.Id);

            Property(c => c.Created)
                .IsRequired()
                .HasColumnType("datetime");
        }
    }
}
