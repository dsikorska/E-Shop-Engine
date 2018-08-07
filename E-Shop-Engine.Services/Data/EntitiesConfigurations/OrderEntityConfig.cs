using System.Data.Entity.ModelConfiguration;
using E_Shop_Engine.Domain.DomainModel;

namespace E_Shop_Engine.Services.Data.EntitiesConfigurations
{
    public class OrderEntityConfig : EntityTypeConfiguration<Order>
    {
        public OrderEntityConfig()
        {
            HasKey(o => o.ID);
            HasRequired(o => o.Customer);
            HasRequired(o => o.OrderLines);
            HasMany(o => o.OrderLines)
                .WithRequired(ol => ol.Order)
                .HasForeignKey(ol => ol.OrderID)
                .WillCascadeOnDelete(true);
        }
    }
}
