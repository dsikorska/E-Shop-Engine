using System.Data.Entity.ModelConfiguration;
using E_Shop_Engine.Domain.DomainModel;

namespace E_Shop_Engine.Services.Data.EntitiesConfigurations
{
    public class CategoryConfigEntity : EntityTypeConfiguration<Category>
    {
        public CategoryConfigEntity()
        {
            HasKey(cat => cat.ID);
            HasRequired(cat => cat.Name);
            HasOptional(cat => cat.Subcategories);
            HasOptional(cat => cat.Products);
            HasOptional(cat => cat.Description);
            HasMany(cat => cat.Subcategories)
                .WithRequired(s => s.Category)
                .HasForeignKey(s => s.CategoryID)
                .WillCascadeOnDelete(true);
            HasMany(cat => cat.Products)
                .WithRequired(p => p.Category)
                .HasForeignKey(p => p.CategoryID)
                .WillCascadeOnDelete(false);
            Property(cat => cat.Name)
                .HasMaxLength(50);
            Property(cat => cat.Description)
                .HasMaxLength(100);
        }
    }
}
