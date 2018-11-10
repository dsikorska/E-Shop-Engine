namespace E_Shop_Engine.Services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateOrder : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Orders", "OrderNumber", c => c.String());
            AddColumn("dbo.Orders", "TransactionNumber", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Orders", "TransactionNumber");
            DropColumn("dbo.Orders", "OrderNumber");
        }
    }
}
