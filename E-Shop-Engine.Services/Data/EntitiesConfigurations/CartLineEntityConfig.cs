using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using E_Shop_Engine.Domain.DomainModel;

namespace E_Shop_Engine.Services.Data.EntitiesConfigurations
{
    public class CartLineEntityConfig : EntityTypeConfiguration<CartLine>
    {
        public CartLineEntityConfig()
        {
            HasKey(c => new { c.ID, c.Cart_Id });

            Property(c => c.ID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Property(c => c.Cart_Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            HasRequired(c => c.Product)
                .WithMany(c => c.CartLines)
                .Map(c => c.MapKey("Product_Id"))
                .WillCascadeOnDelete(true);

            HasRequired(c => c.Cart)
                .WithMany(c => c.CartLines)
                .HasForeignKey(c => c.Cart_Id)
                .WillCascadeOnDelete(true);

            Property(c => c.Quantity)
                .IsRequired();
        }
    }
}
