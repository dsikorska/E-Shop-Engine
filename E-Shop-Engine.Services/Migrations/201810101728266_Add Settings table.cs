namespace E_Shop_Engine.Services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddSettingstable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Settings",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        ShopName = c.String(),
                        Currency = c.String(),
                        AdminEmailAddress = c.String(),
                        NotificationReplyEmail = c.String(),
                        SMTP = c.String(),
                        SMTPUsername = c.String(),
                        SMTPPassword = c.String(),
                        SMTPPort = c.String(),
                        SMTPEnableSSL = c.Boolean(),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Settings");
        }
    }
}
