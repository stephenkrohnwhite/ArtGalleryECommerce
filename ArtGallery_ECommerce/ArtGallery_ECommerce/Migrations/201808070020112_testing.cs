namespace ArtGallery_ECommerce.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class testing : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Carts", "ProductId", "dbo.Products");
            DropForeignKey("dbo.OrderItems", "ProductId", "dbo.Products");
            DropIndex("dbo.Carts", new[] { "ProductId" });
            DropIndex("dbo.OrderItems", new[] { "ProductId" });
        }
        
        public override void Down()
        {
            CreateIndex("dbo.OrderItems", "ProductId");
            CreateIndex("dbo.Carts", "ProductId");
            AddForeignKey("dbo.OrderItems", "ProductId", "dbo.Products", "ProductId", cascadeDelete: true);
            AddForeignKey("dbo.Carts", "ProductId", "dbo.Products", "ProductId", cascadeDelete: true);
        }
    }
}
