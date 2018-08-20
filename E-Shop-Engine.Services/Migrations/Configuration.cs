namespace E_Shop_Engine.Services.Migrations
{
    using System.Data.Entity.Migrations;
    using E_Shop_Engine.Domain.DomainModel;

    internal sealed class Configuration : DbMigrationsConfiguration<Data.AppDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(Data.AppDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.
            context.Categories.AddOrUpdate(
                new Category { Name = "All", Description = "Contains all products" },
                new Category { Name = "Test", Description = "Test" }
                );

            context.Subcategories.AddOrUpdate(
                new Subcategory { Name = "Subcategory", CategoryID = 1 }
                );

            context.Products.AddOrUpdate(
                new Product { Name = "Pierwszy produkt", Description = "Info o produkcie pierwszym", CategoryID = 1, ShowAsSpecialOffer = true, NumberInStock = 5, Price = 11.47M, SubcategoryID = 0 },
                new Product { Name = "Drugi produkt", Description = "Info o produkcie drugim", CategoryID = 1, ShowAsSpecialOffer = true, NumberInStock = 7, Price = 15.99M, SubcategoryID = 0 },
                new Product { Name = "Drugi produkt", Description = "Info o produkcie drugim", CategoryID = 1, ShowAtMainPage = true, NumberInStock = 7, Price = 15.99M, SubcategoryID = 0 },
                new Product { Name = "Drugi produkt", Description = "Info o produkcie drugim", CategoryID = 1, ShowAtMainPage = true, NumberInStock = 7, Price = 15.99M, SubcategoryID = 0 }
                );
        }
    }
}
