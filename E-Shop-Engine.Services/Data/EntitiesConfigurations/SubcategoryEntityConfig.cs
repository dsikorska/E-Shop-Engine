using System.Data.Entity.ModelConfiguration;
using E_Shop_Engine.Domain.DomainModel;

namespace E_Shop_Engine.Services.Data.EntitiesConfigurations
{
    public class SubcategoryEntityConfig : EntityTypeConfiguration<Subcategory>
    {
        public SubcategoryEntityConfig()
        {
            HasKey(s => s.ID);
            HasRequired(s => s.Name);
            HasRequired(s => s.Category);
            HasOptional(s => s.Description);
            HasOptional(s => s.Products);
            HasMany(s => s.Products)
                .WithOptional(p => p.Subcategory)
                .HasForeignKey(p => p.SubcategoryID)
                .WillCascadeOnDelete(false);
            Property(s => s.Name)
                .HasMaxLength(50);
            Property(s => s.Description)
                .HasMaxLength(100);
        }
    }
}
