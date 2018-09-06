namespace E_Shop_Engine.Services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SetupCustomerOrderAddressCartCartLine : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Addresses",
                c => new
                    {
                        ID = c.Int(nullable: false),
                        Street = c.String(nullable: false, maxLength: 100),
                        Line1 = c.String(nullable: false, maxLength: 100),
                        Line2 = c.String(maxLength: 100),
                        City = c.String(nullable: false, maxLength: 100),
                        State = c.String(maxLength: 100),
                        ZipCode = c.String(),
                        Country = c.String(nullable: false, maxLength: 100),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Customers", t => t.ID)
                .Index(t => t.ID);
            
            CreateTable(
                "dbo.Customers",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 100),
                        Surname = c.String(nullable: false, maxLength: 100),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Carts",
                c => new
                    {
                        ID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Customers", t => t.ID)
                .Index(t => t.ID);
            
            CreateTable(
                "dbo.CartLines",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Quantity = c.Int(nullable: false),
                        Cart_ID = c.Int(nullable: false),
                        Product_ID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Carts", t => t.Cart_ID)
                .ForeignKey("dbo.Products", t => t.Product_ID)
                .Index(t => t.Cart_ID)
                .Index(t => t.Product_ID);
            
            CreateTable(
                "dbo.Orders",
                c => new
                    {
                        ID = c.Int(nullable: false),
                        Created = c.DateTime(nullable: false),
                        Finished = c.DateTime(),
                        IsPaid = c.Boolean(nullable: false),
                        PaymentMethod = c.Int(nullable: false),
                        OrderStatus = c.Int(nullable: false),
                        Customer_ID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Carts", t => t.ID)
                .ForeignKey("dbo.Customers", t => t.Customer_ID)
                .Index(t => t.ID)
                .Index(t => t.Customer_ID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Orders", "Customer_ID", "dbo.Customers");
            DropForeignKey("dbo.Carts", "ID", "dbo.Customers");
            DropForeignKey("dbo.Orders", "ID", "dbo.Carts");
            DropForeignKey("dbo.CartLines", "Product_ID", "dbo.Products");
            DropForeignKey("dbo.CartLines", "Cart_ID", "dbo.Carts");
            DropForeignKey("dbo.Addresses", "ID", "dbo.Customers");
            DropIndex("dbo.Orders", new[] { "Customer_ID" });
            DropIndex("dbo.Orders", new[] { "ID" });
            DropIndex("dbo.CartLines", new[] { "Product_ID" });
            DropIndex("dbo.CartLines", new[] { "Cart_ID" });
            DropIndex("dbo.Carts", new[] { "ID" });
            DropIndex("dbo.Addresses", new[] { "ID" });
            DropTable("dbo.Orders");
            DropTable("dbo.CartLines");
            DropTable("dbo.Carts");
            DropTable("dbo.Customers");
            DropTable("dbo.Addresses");
        }
    }
}
