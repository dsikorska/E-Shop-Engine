namespace E_Shop_Engine.Services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddPaymentpropatOrder : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Orders", "Payment", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Orders", "Payment");
        }
    }
}
