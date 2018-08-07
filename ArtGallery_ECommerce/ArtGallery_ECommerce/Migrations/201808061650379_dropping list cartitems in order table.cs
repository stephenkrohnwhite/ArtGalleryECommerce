namespace ArtGallery_ECommerce.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class droppinglistcartitemsinordertable : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Products", "Order_OrderId", "dbo.Orders");
            DropIndex("dbo.Products", new[] { "Order_OrderId" });
            DropColumn("dbo.Products", "Order_OrderId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Products", "Order_OrderId", c => c.Int());
            CreateIndex("dbo.Products", "Order_OrderId");
            AddForeignKey("dbo.Products", "Order_OrderId", "dbo.Orders", "OrderId");
        }
    }
}
