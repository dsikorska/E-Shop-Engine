namespace E_Shop_Engine.Services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddSandboxbooleantoSettings : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Settings", "IsDotPaySandbox", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Settings", "IsDotPaySandbox");
        }
    }
}
