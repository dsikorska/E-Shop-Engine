namespace E_Shop_Engine.Services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Adddotpaysettings : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Settings", "DotPayPIN", c => c.String());
            AddColumn("dbo.Settings", "DotPayId", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Settings", "DotPayId");
            DropColumn("dbo.Settings", "DotPayPIN");
        }
    }
}
