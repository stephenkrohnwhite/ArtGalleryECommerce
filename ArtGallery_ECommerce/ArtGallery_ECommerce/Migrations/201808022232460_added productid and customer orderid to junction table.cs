namespace ArtGallery_ECommerce.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addedproductidandcustomerorderidtojunctiontable : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("dbo.Orders");
            AddColumn("dbo.Orders", "CustomerOrderId", c => c.Int(nullable: false));
            AddColumn("dbo.Orders", "Quantity", c => c.Int(nullable: false));
            AddPrimaryKey("dbo.Orders", new[] { "ProductId", "CustomerOrderId" });
            DropColumn("dbo.Orders", "OrderID");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Orders", "OrderID", c => c.Int(nullable: false, identity: true));
            DropPrimaryKey("dbo.Orders");
            DropColumn("dbo.Orders", "Quantity");
            DropColumn("dbo.Orders", "CustomerOrderId");
            AddPrimaryKey("dbo.Orders", "OrderID");
        }
    }
}
