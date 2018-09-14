namespace E_Shop_Engine.Services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateCartandCartLineconfig : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.CartLines", new[] { "Product_ID" });
            DropPrimaryKey("dbo.CartLines");
            AlterColumn("dbo.CartLines", "ID", c => c.Int(nullable: false));
            AddPrimaryKey("dbo.CartLines", new[] { "ID", "Cart_Id" });
            CreateIndex("dbo.CartLines", "Product_Id");
        }
        
        public override void Down()
        {
            DropIndex("dbo.CartLines", new[] { "Product_Id" });
            DropPrimaryKey("dbo.CartLines");
            AlterColumn("dbo.CartLines", "ID", c => c.Int(nullable: false, identity: true));
            AddPrimaryKey("dbo.CartLines", "ID");
            CreateIndex("dbo.CartLines", "Product_ID");
        }
    }
}
