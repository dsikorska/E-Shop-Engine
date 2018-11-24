namespace E_Shop_Engine.Services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SimplifyCartandOrder : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.OrderedCartLines", "Cart_ID", "dbo.OrderedCarts");
            DropForeignKey("dbo.OrderedCartLines", "ProductId", "dbo.Products");
            DropForeignKey("dbo.OrderedCarts", "Order_Id", "dbo.Orders");
            DropForeignKey("dbo.Carts", "AppUser_Id", "dbo.AppUsers");
            DropForeignKey("dbo.CartLines", "Product_Id", "dbo.Products");
            DropIndex("dbo.OrderedCarts", new[] { "Order_Id" });
            DropIndex("dbo.OrderedCartLines", new[] { "ProductId" });
            DropIndex("dbo.OrderedCartLines", new[] { "Cart_ID" });
            AddColumn("dbo.CartLines", "ProductId", c => c.Int());
            AddColumn("dbo.Orders", "Cart_Id", c => c.Int(nullable: false));
            CreateIndex("dbo.Orders", "Cart_Id");
            AddForeignKey("dbo.Orders", "Cart_Id", "dbo.Carts", "ID");
            AddForeignKey("dbo.Carts", "AppUser_Id", "dbo.AppUsers", "Id", cascadeDelete: true);
            AddForeignKey("dbo.CartLines", "Product_Id", "dbo.Products", "ID", cascadeDelete: true);
            DropTable("dbo.OrderedCarts");
            DropTable("dbo.OrderedCartLines");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.OrderedCartLines",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        ProductId = c.Int(),
                        Quantity = c.Int(nullable: false),
                        Cart_ID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.OrderedCarts",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Order_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            DropForeignKey("dbo.CartLines", "Product_Id", "dbo.Products");
            DropForeignKey("dbo.Carts", "AppUser_Id", "dbo.AppUsers");
            DropForeignKey("dbo.Orders", "Cart_Id", "dbo.Carts");
            DropIndex("dbo.Orders", new[] { "Cart_Id" });
            DropColumn("dbo.Orders", "Cart_Id");
            DropColumn("dbo.CartLines", "ProductId");
            CreateIndex("dbo.OrderedCartLines", "Cart_ID");
            CreateIndex("dbo.OrderedCartLines", "ProductId");
            CreateIndex("dbo.OrderedCarts", "Order_Id");
            AddForeignKey("dbo.CartLines", "Product_Id", "dbo.Products", "ID");
            AddForeignKey("dbo.Carts", "AppUser_Id", "dbo.AppUsers", "Id");
            AddForeignKey("dbo.OrderedCarts", "Order_Id", "dbo.Orders", "ID");
            AddForeignKey("dbo.OrderedCartLines", "ProductId", "dbo.Products", "ID", cascadeDelete: true);
            AddForeignKey("dbo.OrderedCartLines", "Cart_ID", "dbo.OrderedCarts", "ID");
        }
    }
}
