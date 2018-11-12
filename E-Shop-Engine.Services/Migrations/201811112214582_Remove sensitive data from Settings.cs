namespace E_Shop_Engine.Services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemovesensitivedatafromSettings : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Settings", "SMTPUsername");
            DropColumn("dbo.Settings", "SMTPPassword");
            DropColumn("dbo.Settings", "DotPayPIN");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Settings", "DotPayPIN", c => c.String());
            AddColumn("dbo.Settings", "SMTPPassword", c => c.String());
            AddColumn("dbo.Settings", "SMTPUsername", c => c.String());
        }
    }
}
