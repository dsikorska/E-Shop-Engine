namespace E_Shop_Engine.Services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddIsORderedpropatCarts : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Carts", "IsOrdered", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Carts", "IsOrdered");
        }
    }
}
