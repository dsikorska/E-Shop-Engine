namespace E_Shop_Engine.Services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RenameUsertoAppUser : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.Users", newName: "AppUsers");
            DropForeignKey("dbo.IdentityUserClaims", "UserId", "dbo.Users");
            DropForeignKey("dbo.IdentityUserRoles", "UserId", "dbo.Users");
            DropIndex("dbo.IdentityUserClaims", new[] { "UserId" });
            DropIndex("dbo.IdentityUserRoles", new[] { "UserId" });
            RenameColumn(table: "dbo.IdentityUserLogins", name: "User_Id", newName: "AppUser_Id");
            RenameIndex(table: "dbo.IdentityUserLogins", name: "IX_User_Id", newName: "IX_AppUser_Id");
            AddColumn("dbo.IdentityUserClaims", "AppUser_Id", c => c.String(maxLength: 128));
            AddColumn("dbo.IdentityUserRoles", "AppUser_Id", c => c.String(maxLength: 128));
            AlterColumn("dbo.IdentityUserClaims", "UserId", c => c.String());
            CreateIndex("dbo.IdentityUserClaims", "AppUser_Id");
            CreateIndex("dbo.IdentityUserRoles", "AppUser_Id");
            AddForeignKey("dbo.IdentityUserClaims", "AppUser_Id", "dbo.AppUsers", "Id");
            AddForeignKey("dbo.IdentityUserRoles", "AppUser_Id", "dbo.AppUsers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.IdentityUserRoles", "AppUser_Id", "dbo.AppUsers");
            DropForeignKey("dbo.IdentityUserClaims", "AppUser_Id", "dbo.AppUsers");
            DropIndex("dbo.IdentityUserRoles", new[] { "AppUser_Id" });
            DropIndex("dbo.IdentityUserClaims", new[] { "AppUser_Id" });
            AlterColumn("dbo.IdentityUserClaims", "UserId", c => c.String(maxLength: 128));
            DropColumn("dbo.IdentityUserRoles", "AppUser_Id");
            DropColumn("dbo.IdentityUserClaims", "AppUser_Id");
            RenameIndex(table: "dbo.IdentityUserLogins", name: "IX_AppUser_Id", newName: "IX_User_Id");
            RenameColumn(table: "dbo.IdentityUserLogins", name: "AppUser_Id", newName: "User_Id");
            CreateIndex("dbo.IdentityUserRoles", "UserId");
            CreateIndex("dbo.IdentityUserClaims", "UserId");
            AddForeignKey("dbo.IdentityUserRoles", "UserId", "dbo.Users", "Id");
            AddForeignKey("dbo.IdentityUserClaims", "UserId", "dbo.Users", "Id");
            RenameTable(name: "dbo.AppUsers", newName: "Users");
        }
    }
}
