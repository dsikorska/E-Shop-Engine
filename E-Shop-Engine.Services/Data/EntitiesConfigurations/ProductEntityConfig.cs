using System.Data.Entity.ModelConfiguration;
using E_Shop_Engine.Domain.DomainModel;

namespace E_Shop_Engine.Services.Data.EntitiesConfigurations
{
    public class ProductEntityConfig : EntityTypeConfiguration<Product>
    {
        public ProductEntityConfig()
        {
            HasKey(p => p.ID);
            HasRequired(p => p.Name);
            HasRequired(p => p.Category);
            HasOptional(p => p.Subcategory);
            HasOptional(p => p.CatalogNumber);
            HasOptional(p => p.Description);
            HasOptional(p => p.ImageData);
            HasOptional(p => p.ImageMimeType);
            Property(p => p.Name)
                .HasMaxLength(150);
            Property(p => p.Description)
                .HasMaxLength(1000);
        }
    }
}