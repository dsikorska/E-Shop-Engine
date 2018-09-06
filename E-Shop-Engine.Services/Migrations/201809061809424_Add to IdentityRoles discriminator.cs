namespace E_Shop_Engine.Services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddtoIdentityRolesdiscriminator : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.IdentityRoles", "Discriminator", c => c.String(nullable: false, maxLength: 128));
        }
        
        public override void Down()
        {
            DropColumn("dbo.IdentityRoles", "Discriminator");
        }
    }
}
