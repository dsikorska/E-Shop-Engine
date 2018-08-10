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
        }
    }
}
