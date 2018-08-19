using System.Data.Entity.ModelConfiguration;
using E_Shop_Engine.Domain.DomainModel;

namespace E_Shop_Engine.Services.Data.EntitiesConfigurations
{
    public class ProductEntityConfig : EntityTypeConfiguration<Product>
    {
        public ProductEntityConfig()
        {
            HasKey(p => p.ID);
            Property(p => p.Name)
                .IsRequired()
                .HasMaxLength(150);
            Property(p => p.Description)
                .IsOptional()
                .HasMaxLength(1000);
            Property(p => p.ImageData)
                .IsOptional();
            Property(p => p.IsSpecialOffer)
                .IsOptional();
            HasRequired(p => p.Category);
            HasOptional(p => p.Subcategory);
        }
    }
}