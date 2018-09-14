namespace E_Shop_Engine.Services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ReconfigurationOrdersandCartsrelationship : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Orders", "Cart_Id", "dbo.Carts");
            DropIndex("dbo.Orders", new[] { "Cart_Id" });
            DropIndex("dbo.CartLines", new[] { "Cart_ID" });
            CreateTable(
                "dbo.OrderedCarts",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Order_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Orders", t => t.Order_Id)
                .Index(t => t.Order_Id);
            
            CreateTable(
                "dbo.OrderedCartLines",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Quantity = c.Int(nullable: false),
                        Cart_ID = c.Int(nullable: false),
                        Product_ID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.OrderedCarts", t => t.Cart_ID)
                .ForeignKey("dbo.Products", t => t.Product_ID)
                .Index(t => t.Cart_ID)
                .Index(t => t.Product_ID);
            
            CreateIndex("dbo.CartLines", "Cart_Id");
            DropColumn("dbo.Orders", "Cart_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Orders", "Cart_Id", c => c.Int(nullable: false));
            DropForeignKey("dbo.OrderedCarts", "Order_Id", "dbo.Orders");
            DropForeignKey("dbo.OrderedCartLines", "Product_ID", "dbo.Products");
            DropForeignKey("dbo.OrderedCartLines", "Cart_ID", "dbo.OrderedCarts");
            DropIndex("dbo.OrderedCartLines", new[] { "Product_ID" });
            DropIndex("dbo.OrderedCartLines", new[] { "Cart_ID" });
            DropIndex("dbo.OrderedCarts", new[] { "Order_Id" });
            DropIndex("dbo.CartLines", new[] { "Cart_Id" });
            DropTable("dbo.OrderedCartLines");
            DropTable("dbo.OrderedCarts");
            CreateIndex("dbo.CartLines", "Cart_ID");
            CreateIndex("dbo.Orders", "Cart_Id");
            AddForeignKey("dbo.Orders", "Cart_Id", "dbo.Carts", "ID");
        }
    }
}
