namespace E_Shop_Engine.Services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FixAppUserIdgeneration : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Addresses", "AppUser_Id", "dbo.AppUsers");
            DropForeignKey("dbo.Carts", "AppUser_Id", "dbo.AppUsers");
            DropForeignKey("dbo.AspNetUserClaims", "AppUser_Id", "dbo.AppUsers");
            DropForeignKey("dbo.AspNetUserLogins", "AppUser_Id", "dbo.AppUsers");
            DropForeignKey("dbo.Orders", "AppUser_Id", "dbo.AppUsers");
            DropForeignKey("dbo.AspNetUserRoles", "AppUser_Id", "dbo.AppUsers");
            DropPrimaryKey("dbo.AppUsers");
            AlterColumn("dbo.AppUsers", "Id", c => c.String(nullable: false, maxLength: 128));
            AddPrimaryKey("dbo.AppUsers", "Id");
            AddForeignKey("dbo.Addresses", "AppUser_Id", "dbo.AppUsers", "Id");
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
            DropForeignKey("dbo.Addresses", "AppUser_Id", "dbo.AppUsers");
            DropPrimaryKey("dbo.AppUsers");
            AlterColumn("dbo.AppUsers", "Id", c => c.String(nullable: false, maxLength: 128));
            AddPrimaryKey("dbo.AppUsers", "Id");
            AddForeignKey("dbo.AspNetUserRoles", "AppUser_Id", "dbo.AppUsers", "Id");
            AddForeignKey("dbo.Orders", "AppUser_Id", "dbo.AppUsers", "Id");
            AddForeignKey("dbo.AspNetUserLogins", "AppUser_Id", "dbo.AppUsers", "Id");
            AddForeignKey("dbo.AspNetUserClaims", "AppUser_Id", "dbo.AppUsers", "Id");
            AddForeignKey("dbo.Carts", "AppUser_Id", "dbo.AppUsers", "Id");
            AddForeignKey("dbo.Addresses", "AppUser_Id", "dbo.AppUsers", "Id");
        }
    }
}
