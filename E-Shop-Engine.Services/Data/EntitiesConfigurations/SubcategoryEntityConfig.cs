using System.Data.Entity.ModelConfiguration;
using E_Shop_Engine.Domain.DomainModel;

namespace E_Shop_Engine.Services.Data.EntitiesConfigurations
{
    public class SubcategoryEntityConfig : EntityTypeConfiguration<Subcategory>
    {
        public SubcategoryEntityConfig()
        {
            HasKey(s => s.ID);
            Property(s => s.Name)
                .IsRequired()
                .HasMaxLength(50);
            Property(s => s.Description)
                .IsOptional()
                .HasMaxLength(100);
            HasRequired(s => s.Category);
            HasOptional(s => s.Products);
            HasMany(s => s.Products)
                .WithOptional(p => p.Subcategory)
                .HasForeignKey(p => p.SubcategoryID);
        }
    }
}
