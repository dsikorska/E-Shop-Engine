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
                .HasMaxLength(200);

            Property(p => p.Description)
                .IsOptional()
                .HasMaxLength(3990);

            Property(p => p.ImageData)
                .IsOptional();

            HasRequired(p => p.Category);

            HasOptional(p => p.Subcategory);

            Property(p => p.Created)
                .HasColumnType("datetime2");

            Property(p => p.Edited)
                .HasColumnType("datetime2");
        }
    }
}