using System.Data.Entity.ModelConfiguration;
using E_Shop_Engine.Domain.DomainModel;

namespace E_Shop_Engine.Services.Data.EntitiesConfigurations
{
    public class CartLineEntityConfig : EntityTypeConfiguration<CartLine>
    {
        public CartLineEntityConfig()
        {
            HasKey(c => c.ID);
            HasRequired(c => c.Product);

            HasRequired(c => c.Cart)
                .WithMany(c => c.CartLines);

            Property(c => c.Quantity)
                .IsRequired();
        }
    }
}
