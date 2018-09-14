using System.Data.Entity.ModelConfiguration;
using E_Shop_Engine.Domain.DomainModel;

namespace E_Shop_Engine.Services.Data.EntitiesConfigurations
{
    public class OrderedCartEntityConfig : EntityTypeConfiguration<OrderedCart>
    {
        public OrderedCartEntityConfig()
        {
            HasKey(c => c.ID);
        }
    }
}
