namespace E_Shop_Engine.Services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateOrderedCartLinetable : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.OrderedCartLines", new[] { "Product_ID" });
            RenameColumn(table: "dbo.OrderedCartLines", name: "Product_ID", newName: "ProductId");
            AlterColumn("dbo.OrderedCartLines", "ProductId", c => c.Int());
            CreateIndex("dbo.OrderedCartLines", "ProductId");
        }
        
        public override void Down()
        {
            DropIndex("dbo.OrderedCartLines", new[] { "ProductId" });
            AlterColumn("dbo.OrderedCartLines", "ProductId", c => c.Int(nullable: false));
            RenameColumn(table: "dbo.OrderedCartLines", name: "ProductId", newName: "Product_ID");
            CreateIndex("dbo.OrderedCartLines", "Product_ID");
        }
    }
}
