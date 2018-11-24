using System.Data.Entity.ModelConfiguration;
using E_Shop_Engine.Domain.DomainModel;

namespace E_Shop_Engine.Services.Data.EntitiesConfigurations
{
    public class OrderEntityConfig : EntityTypeConfiguration<Order>
    {
        public OrderEntityConfig()
        {
            HasKey(o => o.ID);

            Property(o => o.Created)
                .IsRequired()
                .HasColumnType("datetime");

            Property(o => o.Finished)
                .IsOptional()
                .HasColumnType("datetime");

            Property(o => o.OrderStatus)
                .IsRequired();

            HasRequired(c => c.AppUser)
                .WithOptional()
                .Map(c => c.MapKey("AppUser_Id"))
                .WillCascadeOnDelete(true);

            HasRequired(c => c.Cart)
                .WithOptional(c => c.Order)
                .Map(c => c.MapKey("Cart_Id"))
                .WillCascadeOnDelete(true);
        }
    }
}
