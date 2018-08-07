using System.Data.Entity.ModelConfiguration;
using E_Shop_Engine.Domain.DomainModel;

namespace E_Shop_Engine.Services.Data.EntitiesConfigurations
{
    public class OrderLineEntityConfig : EntityTypeConfiguration<OrderLine>
    {
        public OrderLineEntityConfig()
        {
            HasKey(ol => ol.ID);
            HasRequired(ol => ol.Product);
            HasRequired(ol => ol.Order);
        }
    }
}
