namespace E_Shop_Engine.Services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EditSpecialOfferpropatProduct : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Products", "ShowAsSpecialOffer", c => c.Boolean(nullable: false));
            AddColumn("dbo.Products", "ShowAtMainPage", c => c.Boolean(nullable: false));
            DropColumn("dbo.Products", "IsSpecialOffer");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Products", "IsSpecialOffer", c => c.Boolean(nullable: false));
            DropColumn("dbo.Products", "ShowAtMainPage");
            DropColumn("dbo.Products", "ShowAsSpecialOffer");
        }
    }
}
