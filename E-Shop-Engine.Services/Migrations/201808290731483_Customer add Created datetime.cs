namespace E_Shop_Engine.Services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CustomeraddCreateddatetime : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Customers", "Created", c => c.DateTime(nullable: false, precision: 7, storeType: "datetime2"));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Customers", "Created");
        }
    }
}
