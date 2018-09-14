namespace E_Shop_Engine.Services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SetupCompositeKeyforCartLines : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("dbo.CartLines");
            AlterColumn("dbo.CartLines", "ID", c => c.Int(nullable: false, identity: true));
            AddPrimaryKey("dbo.CartLines", new[] { "ID", "Cart_Id" });
        }
        
        public override void Down()
        {
            DropPrimaryKey("dbo.CartLines");
            AlterColumn("dbo.CartLines", "ID", c => c.Int(nullable: false));
            AddPrimaryKey("dbo.CartLines", new[] { "ID", "Cart_Id" });
        }
    }
}
