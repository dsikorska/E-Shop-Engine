namespace E_Shop_Engine.Services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MigrateAddressfromAppUsertoOrder : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Addresses", "AppUser_Id", "dbo.AppUsers");
            DropIndex("dbo.Addresses", new[] { "AppUser_Id" });
            DropPrimaryKey("dbo.Addresses");
            AlterColumn("dbo.Addresses", "ID", c => c.Int(nullable: false));
            AddPrimaryKey("dbo.Addresses", "ID");
            CreateIndex("dbo.Addresses", "ID");
            AddForeignKey("dbo.Addresses", "ID", "dbo.Orders", "ID");
            DropColumn("dbo.Addresses", "AppUser_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Addresses", "AppUser_Id", c => c.String(nullable: false, maxLength: 128));
            DropForeignKey("dbo.Addresses", "ID", "dbo.Orders");
            DropIndex("dbo.Addresses", new[] { "ID" });
            DropPrimaryKey("dbo.Addresses");
            AlterColumn("dbo.Addresses", "ID", c => c.Int(nullable: false, identity: true));
            AddPrimaryKey("dbo.Addresses", "ID");
            CreateIndex("dbo.Addresses", "AppUser_Id");
            AddForeignKey("dbo.Addresses", "AppUser_Id", "dbo.AppUsers", "Id");
        }
    }
}
