namespace E_Shop_Engine.Services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MoveAddresstoAppUser : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Addresses", "Order_Id", "dbo.Orders");
            DropIndex("dbo.Addresses", new[] { "Order_Id" });
            AddColumn("dbo.Addresses", "AppUser_Id", c => c.String(nullable: false, maxLength: 128));
            CreateIndex("dbo.Addresses", "AppUser_Id");
            AddForeignKey("dbo.Addresses", "AppUser_Id", "dbo.AppUsers", "Id");
            DropColumn("dbo.Addresses", "Order_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Addresses", "Order_Id", c => c.Int(nullable: false));
            DropForeignKey("dbo.Addresses", "AppUser_Id", "dbo.AppUsers");
            DropIndex("dbo.Addresses", new[] { "AppUser_Id" });
            DropColumn("dbo.Addresses", "AppUser_Id");
            CreateIndex("dbo.Addresses", "Order_Id");
            AddForeignKey("dbo.Addresses", "Order_Id", "dbo.Orders", "ID");
        }
    }
}
