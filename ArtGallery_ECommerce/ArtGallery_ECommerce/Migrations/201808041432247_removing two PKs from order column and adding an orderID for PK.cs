namespace ArtGallery_ECommerce.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class removingtwoPKsfromordercolumnandaddinganorderIDforPK : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Carts", new[] { "Order_ProductId", "Order_CustomerOrderId" }, "dbo.Orders");
            DropIndex("dbo.Carts", new[] { "Order_ProductId", "Order_CustomerOrderId" });
            RenameColumn(table: "dbo.Carts", name: "Order_ProductId", newName: "Order_OrderId");
            DropPrimaryKey("dbo.Orders");
            AddColumn("dbo.Orders", "OrderId", c => c.Int(nullable: false, identity: true));
            AddPrimaryKey("dbo.Orders", "OrderId");
            CreateIndex("dbo.Carts", "Order_OrderId");
            AddForeignKey("dbo.Carts", "Order_OrderId", "dbo.Orders", "OrderId");
            DropColumn("dbo.Carts", "Order_CustomerOrderId");
            DropColumn("dbo.Orders", "ProductId");
            DropColumn("dbo.Orders", "CustomerOrderId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Orders", "CustomerOrderId", c => c.Int(nullable: false));
            AddColumn("dbo.Orders", "ProductId", c => c.Int(nullable: false));
            AddColumn("dbo.Carts", "Order_CustomerOrderId", c => c.Int());
            DropForeignKey("dbo.Carts", "Order_OrderId", "dbo.Orders");
            DropIndex("dbo.Carts", new[] { "Order_OrderId" });
            DropPrimaryKey("dbo.Orders");
            DropColumn("dbo.Orders", "OrderId");
            AddPrimaryKey("dbo.Orders", new[] { "ProductId", "CustomerOrderId" });
            RenameColumn(table: "dbo.Carts", name: "Order_OrderId", newName: "Order_ProductId");
            CreateIndex("dbo.Carts", new[] { "Order_ProductId", "Order_CustomerOrderId" });
            AddForeignKey("dbo.Carts", new[] { "Order_ProductId", "Order_CustomerOrderId" }, "dbo.Orders", new[] { "ProductId", "CustomerOrderId" });
        }
    }
}
