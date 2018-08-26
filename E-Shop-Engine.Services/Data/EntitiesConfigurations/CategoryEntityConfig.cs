using System.Data.Entity.ModelConfiguration;
using E_Shop_Engine.Domain.DomainModel;

namespace E_Shop_Engine.Services.Data.EntitiesConfigurations
{
    public class CategoryEntityConfig : EntityTypeConfiguration<Category>
    {
        public CategoryEntityConfig()
        {
            HasKey(cat => cat.ID);
            Property(cat => cat.Name)
                .IsRequired()
                .HasMaxLength(50);
            Property(cat => cat.Description)
                .IsOptional()
                .HasMaxLength(100);
            HasOptional(cat => cat.Subcategories);
            HasOptional(cat => cat.Products);
            HasMany(cat => cat.Subcategories)
                .WithRequired(s => s.Category)
                .HasForeignKey(s => s.CategoryID);
            HasMany(cat => cat.Products)
                .WithRequired(p => p.Category)
                .HasForeignKey(p => p.CategoryID);
        }
    }
}
