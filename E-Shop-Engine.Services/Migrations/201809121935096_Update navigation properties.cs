namespace E_Shop_Engine.Services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Updatenavigationproperties : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Orders", "ID", "dbo.Carts");
            DropForeignKey("dbo.Addresses", "ID", "dbo.Orders");
            DropForeignKey("dbo.Carts", "AppUser_Id", "dbo.AppUsers");
            DropForeignKey("dbo.AspNetUserClaims", "AppUser_Id", "dbo.AppUsers");
            DropForeignKey("dbo.AspNetUserLogins", "AppUser_Id", "dbo.AppUsers");
            DropForeignKey("dbo.Orders", "AppUser_Id", "dbo.AppUsers");
            DropForeignKey("dbo.AspNetUserRoles", "AppUser_Id", "dbo.AppUsers");
            DropIndex("dbo.Addresses", new[] { "ID" });
            DropIndex("dbo.Orders", new[] { "ID" });
            DropPrimaryKey("dbo.Addresses");
            DropPrimaryKey("dbo.AppUsers");
            DropPrimaryKey("dbo.Orders");
            AddColumn("dbo.Addresses", "Order_Id", c => c.Int(nullable: false));
            AddColumn("dbo.Orders", "Cart_Id", c => c.Int(nullable: false));
            AlterColumn("dbo.Addresses", "ID", c => c.Int(nullable: false, identity: true));
            AlterColumn("dbo.AppUsers", "Id", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.Orders", "ID", c => c.Int(nullable: false, identity: true));
            AddPrimaryKey("dbo.Addresses", "ID");
            AddPrimaryKey("dbo.AppUsers", "Id");
            AddPrimaryKey("dbo.Orders", "ID");
            CreateIndex("dbo.Addresses", "Order_Id");
            CreateIndex("dbo.Orders", "Cart_Id");
            AddForeignKey("dbo.Orders", "Cart_Id", "dbo.Carts", "ID");
            AddForeignKey("dbo.Addresses", "Order_Id", "dbo.Orders", "ID");
            AddForeignKey("dbo.Carts", "AppUser_Id", "dbo.AppUsers", "Id");
            AddForeignKey("dbo.AspNetUserClaims", "AppUser_Id", "dbo.AppUsers", "Id");
            AddForeignKey("dbo.AspNetUserLogins", "AppUser_Id", "dbo.AppUsers", "Id");
            AddForeignKey("dbo.Orders", "AppUser_Id", "dbo.AppUsers", "Id");
            AddForeignKey("dbo.AspNetUserRoles", "AppUser_Id", "dbo.AppUsers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "AppUser_Id", "dbo.AppUsers");
            DropForeignKey("dbo.Orders", "AppUser_Id", "dbo.AppUsers");
            DropForeignKey("dbo.AspNetUserLogins", "AppUser_Id", "dbo.AppUsers");
            DropForeignKey("dbo.AspNetUserClaims", "AppUser_Id", "dbo.AppUsers");
            DropForeignKey("dbo.Carts", "AppUser_Id", "dbo.AppUsers");
            DropForeignKey("dbo.Addresses", "Order_Id", "dbo.Orders");
            DropForeignKey("dbo.Orders", "Cart_Id", "dbo.Carts");
            DropIndex("dbo.Orders", new[] { "Cart_Id" });
            DropIndex("dbo.Addresses", new[] { "Order_Id" });
            DropPrimaryKey("dbo.Orders");
            DropPrimaryKey("dbo.AppUsers");
            DropPrimaryKey("dbo.Addresses");
            AlterColumn("dbo.Orders", "ID", c => c.Int(nullable: false));
            AlterColumn("dbo.AppUsers", "Id", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.Addresses", "ID", c => c.Int(nullable: false));
            DropColumn("dbo.Orders", "Cart_Id");
            DropColumn("dbo.Addresses", "Order_Id");
            AddPrimaryKey("dbo.Orders", "ID");
            AddPrimaryKey("dbo.AppUsers", "Id");
            AddPrimaryKey("dbo.Addresses", "ID");
            CreateIndex("dbo.Orders", "ID");
            CreateIndex("dbo.Addresses", "ID");
            AddForeignKey("dbo.AspNetUserRoles", "AppUser_Id", "dbo.AppUsers", "Id");
            AddForeignKey("dbo.Orders", "AppUser_Id", "dbo.AppUsers", "Id");
            AddForeignKey("dbo.AspNetUserLogins", "AppUser_Id", "dbo.AppUsers", "Id");
            AddForeignKey("dbo.AspNetUserClaims", "AppUser_Id", "dbo.AppUsers", "Id");
            AddForeignKey("dbo.Carts", "AppUser_Id", "dbo.AppUsers", "Id");
            AddForeignKey("dbo.Addresses", "ID", "dbo.Orders", "ID");
            AddForeignKey("dbo.Orders", "ID", "dbo.Carts", "ID");
        }
    }
}
