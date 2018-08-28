namespace E_Shop_Engine.Services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ConnectCartwithCustomer : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Orders", "ID", "dbo.Carts");
            DropForeignKey("dbo.CartLines", "CartID", "dbo.Carts");
            DropPrimaryKey("dbo.Carts");
            AddColumn("dbo.Carts", "CustomerID", c => c.Int(nullable: false));
            AddColumn("dbo.Customers", "CartID", c => c.Int(nullable: false));
            AlterColumn("dbo.Carts", "ID", c => c.Int(nullable: false));
            AddPrimaryKey("dbo.Carts", "ID");
            CreateIndex("dbo.Carts", "ID");
            AddForeignKey("dbo.Carts", "ID", "dbo.Customers", "ID");
            AddForeignKey("dbo.Orders", "ID", "dbo.Carts", "ID");
            AddForeignKey("dbo.CartLines", "CartID", "dbo.Carts", "ID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.CartLines", "CartID", "dbo.Carts");
            DropForeignKey("dbo.Orders", "ID", "dbo.Carts");
            DropForeignKey("dbo.Carts", "ID", "dbo.Customers");
            DropIndex("dbo.Carts", new[] { "ID" });
            DropPrimaryKey("dbo.Carts");
            AlterColumn("dbo.Carts", "ID", c => c.Int(nullable: false, identity: true));
            DropColumn("dbo.Customers", "CartID");
            DropColumn("dbo.Carts", "CustomerID");
            AddPrimaryKey("dbo.Carts", "ID");
            AddForeignKey("dbo.CartLines", "CartID", "dbo.Carts", "ID");
            AddForeignKey("dbo.Orders", "ID", "dbo.Carts", "ID");
        }
    }
}
