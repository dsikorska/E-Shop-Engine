namespace E_Shop_Engine.Services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateSettings : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Settings", "ContactEmailAddress", c => c.String());
            AlterColumn("dbo.Settings", "SMTPPort", c => c.Int(nullable: false));
            AlterColumn("dbo.Settings", "SMTPEnableSSL", c => c.Boolean(nullable: false));
            DropColumn("dbo.Settings", "AdminEmailAddress");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Settings", "AdminEmailAddress", c => c.String());
            AlterColumn("dbo.Settings", "SMTPEnableSSL", c => c.Boolean());
            AlterColumn("dbo.Settings", "SMTPPort", c => c.String());
            DropColumn("dbo.Settings", "ContactEmailAddress");
        }
    }
}
