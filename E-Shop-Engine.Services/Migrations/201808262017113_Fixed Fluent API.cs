namespace E_Shop_Engine.Services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FixedFluentAPI : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Addresses", "Street", c => c.String(nullable: false, maxLength: 100));
            AlterColumn("dbo.Addresses", "Line1", c => c.String(nullable: false, maxLength: 100));
            AlterColumn("dbo.Addresses", "Line2", c => c.String(maxLength: 100));
            AlterColumn("dbo.Addresses", "City", c => c.String(nullable: false, maxLength: 100));
            AlterColumn("dbo.Addresses", "State", c => c.String(maxLength: 100));
            AlterColumn("dbo.Addresses", "Country", c => c.String(nullable: false, maxLength: 100));
            AlterColumn("dbo.Categories", "Name", c => c.String(nullable: false, maxLength: 50));
            AlterColumn("dbo.Categories", "Description", c => c.String(maxLength: 100));
            AlterColumn("dbo.Products", "Name", c => c.String(nullable: false, maxLength: 200));
            AlterColumn("dbo.Products", "Description", c => c.String(maxLength: 3990));
            AlterColumn("dbo.Products", "Created", c => c.DateTime(nullable: false, precision: 7, storeType: "datetime2"));
            AlterColumn("dbo.Products", "Edited", c => c.DateTime(precision: 7, storeType: "datetime2"));
            AlterColumn("dbo.Subcategories", "Name", c => c.String(nullable: false, maxLength: 50));
            AlterColumn("dbo.Subcategories", "Description", c => c.String(maxLength: 100));
            AlterColumn("dbo.Customers", "Email", c => c.String(nullable: false, maxLength: 150));
            AlterColumn("dbo.Customers", "Password", c => c.String(nullable: false, maxLength: 250));
            AlterColumn("dbo.Customers", "Name", c => c.String(nullable: false, maxLength: 100));
            AlterColumn("dbo.Customers", "Surname", c => c.String(nullable: false, maxLength: 100));
            AlterColumn("dbo.Customers", "PhoneNumber", c => c.String(nullable: false, maxLength: 50));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Customers", "PhoneNumber", c => c.String());
            AlterColumn("dbo.Customers", "Surname", c => c.String());
            AlterColumn("dbo.Customers", "Name", c => c.String());
            AlterColumn("dbo.Customers", "Password", c => c.String());
            AlterColumn("dbo.Customers", "Email", c => c.String());
            AlterColumn("dbo.Subcategories", "Description", c => c.String());
            AlterColumn("dbo.Subcategories", "Name", c => c.String());
            AlterColumn("dbo.Products", "Edited", c => c.DateTime());
            AlterColumn("dbo.Products", "Created", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Products", "Description", c => c.String());
            AlterColumn("dbo.Products", "Name", c => c.String());
            AlterColumn("dbo.Categories", "Description", c => c.String());
            AlterColumn("dbo.Categories", "Name", c => c.String());
            AlterColumn("dbo.Addresses", "Country", c => c.String());
            AlterColumn("dbo.Addresses", "State", c => c.String());
            AlterColumn("dbo.Addresses", "City", c => c.String());
            AlterColumn("dbo.Addresses", "Line2", c => c.String());
            AlterColumn("dbo.Addresses", "Line1", c => c.String());
            AlterColumn("dbo.Addresses", "Street", c => c.String());
        }
    }
}
