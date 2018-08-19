namespace E_Shop_Engine.Services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddSpecialOfferproptoProduct : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Products", "IsSpecialOffer", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Products", "IsSpecialOffer");
        }
    }
}
