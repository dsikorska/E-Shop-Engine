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

            Property(c => c.Name)
                .IsRequired()
                .HasMaxLength(100);

            Property(c => c.Surname)
                .IsRequired()
                .HasMaxLength(100);

            HasMany(c => c.Orders)
                .WithRequired(o => o.AppUser);

            HasOptional(c => c.Address)
                .WithRequired(c => c.AppUser)
                .Map(c => c.MapKey("AppUser_Id"))
                .WillCascadeOnDelete(true);
        }
    }
}
