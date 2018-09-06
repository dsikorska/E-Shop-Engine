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

            Property(o => o.PaymentMethod)
                .IsRequired();

            Property(o => o.OrderStatus)
                .IsRequired();
        }
    }
}
