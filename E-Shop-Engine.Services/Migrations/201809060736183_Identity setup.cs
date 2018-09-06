namespace E_Shop_Engine.Services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Identitysetup : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Customers", "AddressID", "dbo.Addresses");
            DropForeignKey("dbo.Orders", "ID", "dbo.Carts");
            DropForeignKey("dbo.Orders", "CustomerID", "dbo.Customers");
            DropForeignKey("dbo.Carts", "ID", "dbo.Customers");
            DropForeignKey("dbo.CartLines", "CartID", "dbo.Carts");
            DropForeignKey("dbo.CartLines", "Product_ID", "dbo.Products");
            DropIndex("dbo.CartLines", new[] { "CartID" });
            DropIndex("dbo.CartLines", new[] { "Product_ID" });
            DropIndex("dbo.Carts", new[] { "ID" });
            DropIndex("dbo.Customers", new[] { "AddressID" });
            DropIndex("dbo.Orders", new[] { "ID" });
            DropIndex("dbo.Orders", new[] { "CustomerID" });
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Created = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        Email = c.String(),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.IdentityUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.UserId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.IdentityUserLogins",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        LoginProvider = c.String(),
                        ProviderKey = c.String(),
                        User_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.UserId)
                .ForeignKey("dbo.Users", t => t.User_Id)
                .Index(t => t.User_Id);
            
            CreateTable(
                "dbo.IdentityUserRoles",
                c => new
                    {
                        RoleId = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                        IdentityRole_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => new { t.RoleId, t.UserId })
                .ForeignKey("dbo.Users", t => t.UserId)
                .ForeignKey("dbo.IdentityRoles", t => t.IdentityRole_Id)
                .Index(t => t.UserId)
                .Index(t => t.IdentityRole_Id);
            
            CreateTable(
                "dbo.IdentityRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            DropTable("dbo.Addresses");
            DropTable("dbo.CartLines");
            DropTable("dbo.Carts");
            DropTable("dbo.Customers");
            DropTable("dbo.Orders");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Orders",
                c => new
                    {
                        ID = c.Int(nullable: false),
                        Created = c.DateTime(nullable: false),
                        Finished = c.DateTime(),
                        CustomerID = c.Int(nullable: false),
                        CartID = c.Int(nullable: false),
                        IsPaid = c.Boolean(nullable: false),
                        PaymentMethod = c.Int(nullable: false),
                        OrderStatus = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Customers",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Created = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        Email = c.String(nullable: false, maxLength: 150),
                        Password = c.String(nullable: false, maxLength: 250),
                        Name = c.String(nullable: false, maxLength: 100),
                        Surname = c.String(nullable: false, maxLength: 100),
                        PhoneNumber = c.String(nullable: false, maxLength: 50),
                        AddressID = c.Int(nullable: false),
                        CartID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Carts",
                c => new
                    {
                        ID = c.Int(nullable: false),
                        OrderID = c.Int(),
                        CustomerID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.CartLines",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Quantity = c.Int(nullable: false),
                        CartID = c.Int(nullable: false),
                        Product_ID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Addresses",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Street = c.String(nullable: false, maxLength: 100),
                        Line1 = c.String(nullable: false, maxLength: 100),
                        Line2 = c.String(maxLength: 100),
                        City = c.String(nullable: false, maxLength: 100),
                        State = c.String(maxLength: 100),
                        ZipCode = c.String(),
                        Country = c.String(nullable: false, maxLength: 100),
                    })
                .PrimaryKey(t => t.ID);
            
            DropForeignKey("dbo.IdentityUserRoles", "IdentityRole_Id", "dbo.IdentityRoles");
            DropForeignKey("dbo.IdentityUserRoles", "UserId", "dbo.Users");
            DropForeignKey("dbo.IdentityUserLogins", "User_Id", "dbo.Users");
            DropForeignKey("dbo.IdentityUserClaims", "UserId", "dbo.Users");
            DropIndex("dbo.IdentityUserRoles", new[] { "IdentityRole_Id" });
            DropIndex("dbo.IdentityUserRoles", new[] { "UserId" });
            DropIndex("dbo.IdentityUserLogins", new[] { "User_Id" });
            DropIndex("dbo.IdentityUserClaims", new[] { "UserId" });
            DropTable("dbo.IdentityRoles");
            DropTable("dbo.IdentityUserRoles");
            DropTable("dbo.IdentityUserLogins");
            DropTable("dbo.IdentityUserClaims");
            DropTable("dbo.Users");
            CreateIndex("dbo.Orders", "CustomerID");
            CreateIndex("dbo.Orders", "ID");
            CreateIndex("dbo.Customers", "AddressID");
            CreateIndex("dbo.Carts", "ID");
            CreateIndex("dbo.CartLines", "Product_ID");
            CreateIndex("dbo.CartLines", "CartID");
            AddForeignKey("dbo.CartLines", "Product_ID", "dbo.Products", "ID");
            AddForeignKey("dbo.CartLines", "CartID", "dbo.Carts", "ID");
            AddForeignKey("dbo.Carts", "ID", "dbo.Customers", "ID");
            AddForeignKey("dbo.Orders", "CustomerID", "dbo.Customers", "ID");
            AddForeignKey("dbo.Orders", "ID", "dbo.Carts", "ID");
            AddForeignKey("dbo.Customers", "AddressID", "dbo.Addresses", "ID");
        }
    }
}
