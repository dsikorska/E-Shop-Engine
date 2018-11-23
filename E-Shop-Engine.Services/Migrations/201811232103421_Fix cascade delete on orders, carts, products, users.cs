namespace E_Shop_Engine.Services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Fixcascadedeleteonorderscartsproductsusers : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Addresses", "AppUser_Id", "dbo.AppUsers");
            DropForeignKey("dbo.CartLines", "Cart_Id", "dbo.Carts");
            DropForeignKey("dbo.Orders", "Cart_Id", "dbo.Carts");
            AddForeignKey("dbo.Addresses", "AppUser_Id", "dbo.AppUsers", "Id", cascadeDelete: true);
            AddForeignKey("dbo.CartLines", "Cart_Id", "dbo.Carts", "ID", cascadeDelete: true);
            AddForeignKey("dbo.Orders", "Cart_Id", "dbo.Carts", "ID", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Orders", "Cart_Id", "dbo.Carts");
            DropForeignKey("dbo.CartLines", "Cart_Id", "dbo.Carts");
            DropForeignKey("dbo.Addresses", "AppUser_Id", "dbo.AppUsers");
            AddForeignKey("dbo.Orders", "Cart_Id", "dbo.Carts", "ID");
            AddForeignKey("dbo.CartLines", "Cart_Id", "dbo.Carts", "ID");
            AddForeignKey("dbo.Addresses", "AppUser_Id", "dbo.AppUsers", "Id");
        }
    }
}
