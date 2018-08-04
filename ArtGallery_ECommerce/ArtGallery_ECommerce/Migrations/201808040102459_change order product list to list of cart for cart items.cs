namespace ArtGallery_ECommerce.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changeorderproductlisttolistofcartforcartitems : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Carts", "Order_ProductId", c => c.Int());
            AddColumn("dbo.Carts", "Order_CustomerOrderId", c => c.Int());
            CreateIndex("dbo.Carts", new[] { "Order_ProductId", "Order_CustomerOrderId" });
            AddForeignKey("dbo.Carts", new[] { "Order_ProductId", "Order_CustomerOrderId" }, "dbo.Orders", new[] { "ProductId", "CustomerOrderId" });
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Carts", new[] { "Order_ProductId", "Order_CustomerOrderId" }, "dbo.Orders");
            DropIndex("dbo.Carts", new[] { "Order_ProductId", "Order_CustomerOrderId" });
            DropColumn("dbo.Carts", "Order_CustomerOrderId");
            DropColumn("dbo.Carts", "Order_ProductId");
        }
    }
}
