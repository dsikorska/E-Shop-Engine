namespace E_Shop_Engine.Services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangePaymentMethodtostringatOrderstable : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Orders", "PaymentMethod", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Orders", "PaymentMethod", c => c.Int(nullable: false));
        }
    }
}
