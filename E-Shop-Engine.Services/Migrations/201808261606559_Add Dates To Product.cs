namespace E_Shop_Engine.Services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddDatesToProduct : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Products", "Created", c => c.DateTime(nullable: false));
            AddColumn("dbo.Products", "Edited", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Products", "Edited");
            DropColumn("dbo.Products", "Created");
        }
    }
}
