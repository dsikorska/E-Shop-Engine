using System.Data.Entity.ModelConfiguration;
using E_Shop_Engine.Domain.DomainModel;

namespace E_Shop_Engine.Services.Data.EntitiesConfigurations
{
    public class CartEntityConfig : EntityTypeConfiguration<Cart>
    {
        public CartEntityConfig()
        {
            HasKey(c => c.ID);

            HasRequired(c => c.AppUser)
                .WithMany(c => c.Carts)
                .Map(c => c.MapKey("AppUser_Id"))
                .WillCascadeOnDelete(true);
        }
    }
}
