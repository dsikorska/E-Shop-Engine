namespace E_Shop_Engine.Services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Switchdatetime2todatetimeatAppUser : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.AppUsers", "Created", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.AppUsers", "Created", c => c.DateTime(nullable: false, precision: 7, storeType: "datetime2"));
        }
    }
}
