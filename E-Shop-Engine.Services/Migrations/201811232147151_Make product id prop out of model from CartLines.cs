namespace E_Shop_Engine.Services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MakeproductidpropoutofmodelfromCartLines : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.CartLines", "ProductId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.CartLines", "ProductId", c => c.Int());
        }
    }
}
