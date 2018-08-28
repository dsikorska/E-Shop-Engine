namespace E_Shop_Engine.Services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SetupCartandOrders : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.OrderLines", "ProductID", "dbo.Products");
            DropForeignKey("dbo.OrderLines", "OrderID", "dbo.Orders");
            DropIndex("dbo.OrderLines", new[] { "ProductID" });
            DropIndex("dbo.OrderLines", new[] { "OrderID" });
            DropPrimaryKey("dbo.Orders");
            CreateTable(
                "dbo.CartLines",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Quantity = c.Int(nullable: false),
                        CartID = c.Int(nullable: false),
                        Product_ID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Carts", t => t.CartID)
                .ForeignKey("dbo.Products", t => t.Product_ID)
                .Index(t => t.CartID)
                .Index(t => t.Product_ID);
            
            CreateTable(
                "dbo.Carts",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        OrderID = c.Int(),
                    })
                .PrimaryKey(t => t.ID);
            
            AddColumn("dbo.Orders", "Created", c => c.DateTime(nullable: false));
            AddColumn("dbo.Orders", "Finished", c => c.DateTime());
            AddColumn("dbo.Orders", "CartID", c => c.Int(nullable: false));
            AddColumn("dbo.Orders", "IsPaid", c => c.Boolean(nullable: false));
            AddColumn("dbo.Orders", "PaymentMethod", c => c.Int(nullable: false));
            AlterColumn("dbo.Orders", "ID", c => c.Int(nullable: false));
            AddPrimaryKey("dbo.Orders", "ID");
            CreateIndex("dbo.Orders", "ID");
            AddForeignKey("dbo.Orders", "ID", "dbo.Carts", "ID");
            DropColumn("dbo.Orders", "CreatedDate");
            DropTable("dbo.OrderLines");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.OrderLines",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Quantity = c.Int(nullable: false),
                        ProductID = c.Int(nullable: false),
                        OrderID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            AddColumn("dbo.Orders", "CreatedDate", c => c.DateTime(nullable: false));
            DropForeignKey("dbo.CartLines", "Product_ID", "dbo.Products");
            DropForeignKey("dbo.CartLines", "CartID", "dbo.Carts");
            DropForeignKey("dbo.Orders", "ID", "dbo.Carts");
            DropIndex("dbo.Orders", new[] { "ID" });
            DropIndex("dbo.CartLines", new[] { "Product_ID" });
            DropIndex("dbo.CartLines", new[] { "CartID" });
            DropPrimaryKey("dbo.Orders");
            AlterColumn("dbo.Orders", "ID", c => c.Int(nullable: false, identity: true));
            DropColumn("dbo.Orders", "PaymentMethod");
            DropColumn("dbo.Orders", "IsPaid");
            DropColumn("dbo.Orders", "CartID");
            DropColumn("dbo.Orders", "Finished");
            DropColumn("dbo.Orders", "Created");
            DropTable("dbo.Carts");
            DropTable("dbo.CartLines");
            AddPrimaryKey("dbo.Orders", "ID");
            CreateIndex("dbo.OrderLines", "OrderID");
            CreateIndex("dbo.OrderLines", "ProductID");
            AddForeignKey("dbo.OrderLines", "OrderID", "dbo.Orders", "ID");
            AddForeignKey("dbo.OrderLines", "ProductID", "dbo.Products", "ID");
        }
    }
}
