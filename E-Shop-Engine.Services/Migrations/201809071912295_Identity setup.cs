namespace E_Shop_Engine.Services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Identitysetup : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Customers", "AddressID", "dbo.Addresses");
            DropForeignKey("dbo.Orders", "CustomerID", "dbo.Customers");
            DropForeignKey("dbo.Carts", "ID", "dbo.Customers");
            DropForeignKey("dbo.Orders", "ID", "dbo.Carts");
            DropForeignKey("dbo.CartLines", "CartID", "dbo.Carts");
            DropIndex("dbo.Carts", new[] { "ID" });
            DropIndex("dbo.Customers", new[] { "AddressID" });
            DropIndex("dbo.Orders", new[] { "CustomerID" });
            RenameColumn(table: "dbo.CartLines", name: "CartID", newName: "Cart_ID");
            RenameIndex(table: "dbo.CartLines", name: "IX_CartID", newName: "IX_Cart_ID");
            DropPrimaryKey("dbo.Carts");
            CreateTable(
                "dbo.AppUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Created = c.DateTime(nullable: false),
                        Name = c.String(nullable: false, maxLength: 100),
                        Surname = c.String(nullable: false, maxLength: 100),
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
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                        AppUser_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AppUsers", t => t.AppUser_Id)
                .Index(t => t.AppUser_Id);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        LoginProvider = c.String(),
                        ProviderKey = c.String(),
                        AppUser_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.UserId)
                .ForeignKey("dbo.AppUsers", t => t.AppUser_Id)
                .Index(t => t.AppUser_Id);
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        RoleId = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                        AppUser_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => new { t.RoleId, t.UserId })
                .ForeignKey("dbo.AppUsers", t => t.AppUser_Id)
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId)
                .Index(t => t.RoleId)
                .Index(t => t.AppUser_Id);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
            AddColumn("dbo.Addresses", "AppUser_Id", c => c.String(nullable: false, maxLength: 128));
            AddColumn("dbo.Carts", "AppUser_Id", c => c.String(nullable: false, maxLength: 128));
            AddColumn("dbo.Orders", "AppUser_Id", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.Carts", "ID", c => c.Int(nullable: false, identity: true));
            AddPrimaryKey("dbo.Carts", "ID");
            CreateIndex("dbo.Addresses", "AppUser_Id");
            CreateIndex("dbo.Carts", "AppUser_Id");
            CreateIndex("dbo.Orders", "AppUser_Id");
            AddForeignKey("dbo.Addresses", "AppUser_Id", "dbo.AppUsers", "Id");
            AddForeignKey("dbo.Carts", "AppUser_Id", "dbo.AppUsers", "Id");
            AddForeignKey("dbo.Orders", "AppUser_Id", "dbo.AppUsers", "Id");
            AddForeignKey("dbo.CartLines", "Cart_ID", "dbo.Carts", "ID");
            AddForeignKey("dbo.Orders", "ID", "dbo.Carts", "ID");
            DropColumn("dbo.Carts", "OrderID");
            DropColumn("dbo.Carts", "CustomerID");
            DropColumn("dbo.Orders", "CustomerID");
            DropColumn("dbo.Orders", "CartID");
            DropTable("dbo.Customers");
        }
        
        public override void Down()
        {
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
            
            AddColumn("dbo.Orders", "CartID", c => c.Int(nullable: false));
            AddColumn("dbo.Orders", "CustomerID", c => c.Int(nullable: false));
            AddColumn("dbo.Carts", "CustomerID", c => c.Int(nullable: false));
            AddColumn("dbo.Carts", "OrderID", c => c.Int());
            DropForeignKey("dbo.Orders", "ID", "dbo.Carts");
            DropForeignKey("dbo.CartLines", "Cart_ID", "dbo.Carts");
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.AspNetUserRoles", "AppUser_Id", "dbo.AppUsers");
            DropForeignKey("dbo.Orders", "AppUser_Id", "dbo.AppUsers");
            DropForeignKey("dbo.AspNetUserLogins", "AppUser_Id", "dbo.AppUsers");
            DropForeignKey("dbo.AspNetUserClaims", "AppUser_Id", "dbo.AppUsers");
            DropForeignKey("dbo.Carts", "AppUser_Id", "dbo.AppUsers");
            DropForeignKey("dbo.Addresses", "AppUser_Id", "dbo.AppUsers");
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.AspNetUserRoles", new[] { "AppUser_Id" });
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserLogins", new[] { "AppUser_Id" });
            DropIndex("dbo.AspNetUserClaims", new[] { "AppUser_Id" });
            DropIndex("dbo.Orders", new[] { "AppUser_Id" });
            DropIndex("dbo.Carts", new[] { "AppUser_Id" });
            DropIndex("dbo.Addresses", new[] { "AppUser_Id" });
            DropPrimaryKey("dbo.Carts");
            AlterColumn("dbo.Carts", "ID", c => c.Int(nullable: false));
            DropColumn("dbo.Orders", "AppUser_Id");
            DropColumn("dbo.Carts", "AppUser_Id");
            DropColumn("dbo.Addresses", "AppUser_Id");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AppUsers");
            AddPrimaryKey("dbo.Carts", "ID");
            RenameIndex(table: "dbo.CartLines", name: "IX_Cart_ID", newName: "IX_CartID");
            RenameColumn(table: "dbo.CartLines", name: "Cart_ID", newName: "CartID");
            CreateIndex("dbo.Orders", "CustomerID");
            CreateIndex("dbo.Customers", "AddressID");
            CreateIndex("dbo.Carts", "ID");
            AddForeignKey("dbo.CartLines", "CartID", "dbo.Carts", "ID");
            AddForeignKey("dbo.Orders", "ID", "dbo.Carts", "ID");
            AddForeignKey("dbo.Carts", "ID", "dbo.Customers", "ID");
            AddForeignKey("dbo.Orders", "CustomerID", "dbo.Customers", "ID");
            AddForeignKey("dbo.Customers", "AddressID", "dbo.Addresses", "ID");
        }
    }
}
