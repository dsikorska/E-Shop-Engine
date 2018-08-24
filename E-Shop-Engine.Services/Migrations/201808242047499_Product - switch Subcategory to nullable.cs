namespace E_Shop_Engine.Services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ProductswitchSubcategorytonullable : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Products", new[] { "SubcategoryID" });
            AlterColumn("dbo.Products", "SubcategoryID", c => c.Int());
            CreateIndex("dbo.Products", "SubcategoryID");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Products", new[] { "SubcategoryID" });
            AlterColumn("dbo.Products", "SubcategoryID", c => c.Int(nullable: false));
            CreateIndex("dbo.Products", "SubcategoryID");
        }
    }
}
