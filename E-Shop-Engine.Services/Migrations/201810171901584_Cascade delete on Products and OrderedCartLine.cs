namespace E_Shop_Engine.Services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CascadedeleteonProductsandOrderedCartLine : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.OrderedCartLines", "ProductId", "dbo.Products");
            AddForeignKey("dbo.OrderedCartLines", "ProductId", "dbo.Products", "ID", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.OrderedCartLines", "ProductId", "dbo.Products");
            AddForeignKey("dbo.OrderedCartLines", "ProductId", "dbo.Products", "ID");
        }
    }
}
